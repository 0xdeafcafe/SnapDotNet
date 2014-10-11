using System;
using System.Collections.Generic;
using System.Linq;
using Windows.Graphics.Display;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
using ColdSnap.Common;
using ColdSnap.Controls;
using ColdSnap.Converters.SnapDotNet;
using ColdSnap.ViewModels;
using SnapDotNet.Data;

namespace ColdSnap.Pages
{
	public sealed partial class MainPage
	{
		private ListView _friendsListView;

		public MainPage()
		{
			InitializeComponent();
			DataContext = ViewModel = new MainPageViewModel();

			ApplicationView.GetForCurrentView().SetDesiredBoundsMode(ApplicationViewBoundsMode.UseCoreWindow);
			DisplayInformation.AutoRotationPreferences = DisplayOrientations.Portrait;

			NavigationHelper = new NavigationHelper(this);
			NavigationHelper.LoadState += NavigationHelper_LoadState;
			NavigationHelper.SaveState += NavigationHelper_SaveState;
		}

		/// <summary>
		/// Gets the <see cref="NavigationHelper"/> associated with this <see cref="Page"/>.
		/// </summary>
		public NavigationHelper NavigationHelper { get; private set; }

		/// <summary>
		/// Gets the view model for this page.
		/// </summary>
		public MainPageViewModel ViewModel { get; private set; }

		/// <summary>
		/// Populates the page with content passed during navigation.  Any saved state is also
		/// provided when recreating a page from a prior session.
		/// </summary>
		/// <param name="sender">
		/// The source of the event; typically <see cref="NavigationHelper"/>
		/// </param>
		/// <param name="e">Event data that provides both the navigation parameter passed to
		/// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested and
		/// a dictionary of state preserved by this page during an earlier
		/// session.  The state will be null the first time a page is visited.</param>
		private void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
		{
			if (e.NavigationParameter is Account)
				ViewModel.Account = e.NavigationParameter as Account;

			// Clear backstack and refresh automatically if navigated from StartPage.
			var backstack = (Window.Current.Content as Frame).BackStack;
			if (backstack.Any(entry => entry.SourcePageType == typeof(StartPage)))
			{
				backstack.Clear();
				ViewModel.RefreshCommand.Execute(null);
			}
		}

		/// <summary>
		/// Preserves state associated with this page in case the application is suspended or the
		/// page is discarded from the navigation cache.  Values must conform to the serialization
		/// requirements of <see cref="SuspensionManager.SessionState"/>.
		/// </summary>
		/// <param name="sender">The source of the event; typically <see cref="NavigationHelper"/></param>
		/// <param name="e">Event data that provides an empty dictionary to be populated with
		/// serializable state.</param>
		private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
		{
		}

		#region NavigationHelper registration

		/// <summary>
		/// The methods provided in this section are simply used to allow
		/// NavigationHelper to respond to the page's navigation methods.
		/// <para>
		/// Page specific logic should be placed in event handlers for the  
		/// <see cref="NavigationHelper.LoadState"/>
		/// and <see cref="NavigationHelper.SaveState"/>.
		/// The navigation parameter is available in the LoadState method 
		/// in addition to page state preserved during an earlier session.
		/// </para>
		/// </summary>
		/// <param name="e">Provides data for navigation methods and event
		/// handlers that cannot cancel the navigation request.</param>
		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			NavigationHelper.OnNavigatedTo(e);
		}

		protected override void OnNavigatedFrom(NavigationEventArgs e)
		{
			NavigationHelper.OnNavigatedFrom(e);
		}

		#endregion

		private readonly List<ExpanderView> _friendsListItems = new List<ExpanderView>();

		private void ExpanderView_OnExpanded(object sender, bool e)
		{
			var expander = sender as ExpanderView;
			if (expander == null) return;

			// Update action text.
			var friend = (from f in ViewModel.Account.Friends where f.Username == expander.Tag select f).First();
			var converter = new StoryActionTextConverter();
			expander.SubHeaderText = converter.Convert(friend, typeof (string), null, "") as string;

			// Hide the others (this one works with virtualization unlike Alex's original code)
			if (!_friendsListItems.Contains(expander))
				_friendsListItems.Add(expander);
			foreach (var itemExpander in _friendsListItems.Where(itemExpander => itemExpander != expander))
				itemExpander.Contract();
		}

		private void FriendsListView_OnLoaded(object sender, RoutedEventArgs e)
		{
			_friendsListView = sender as ListView;
		}

		#region Story viewing and stuff

		private DispatcherTimer _isStillHoldingTimer;
		private bool _pointerDown;
		private Button _selectedButton;
		private Friend _selectedFriend;

		private void FriendItem_OnPointerEntered(object sender, PointerRoutedEventArgs e)
		{
			if (_isStillHoldingTimer != null)
				_isStillHoldingTimer.Stop();

			if (_pointerDown)
				return;

			var button = sender as Button;
			if (button == null) return;
			var friend = button.DataContext as Friend;

			if (friend == null || friend.Stories == null || !friend.Stories.Any() || friend.Stories.All(s => s.IsExpired || friend.Stories.Any(story => !story.IsCached)))
				return;

			_isStillHoldingTimer = new DispatcherTimer {Interval = new TimeSpan(0, 0, 0, 0, 300)};
			_isStillHoldingTimer.Tick += async delegate
			{
				_isStillHoldingTimer.Stop();

				if (!_pointerDown || friend.Username != _selectedFriend.Username)
					return;

				// Prepare manipulation mode.
				if (_selectedButton != null)
					_selectedButton.ManipulationMode = ManipulationModes.None;

				// Prepare story.
				ViewModel.ShowStory(friend);

				// Hide command bar.
				if (BottomCommandBar != null)
					BottomCommandBar.Visibility = Visibility.Collapsed;
			};
			_selectedButton = button;
			_selectedFriend = friend;
			_pointerDown = true;
			_isStillHoldingTimer.Start();
		}

		private void FriendItem_OnPointerCaptureLost(object sender, PointerRoutedEventArgs e)
		{
			_pointerDown = false;

			if (_selectedButton != null)
				_selectedButton.ManipulationMode = ManipulationModes.All;

			// Hide story.
			ViewModel.HideStories();

			// Show command bar.
			if (BottomCommandBar != null)
				BottomCommandBar.Visibility = Visibility.Visible;
		}

		#endregion

		private async void FriendItem_OnTapped(object sender, TappedRoutedEventArgs e)
		{
			var button = sender as Button;
			if (button == null) return;
			var friend = button.DataContext as Friend;

			foreach (var itemExpander in _friendsListItems)
			{
				if (itemExpander.Tag == friend.Username && friend.Stories.Any() && itemExpander.IsExpanded)
				{
					var downloadStoriesTask = friend.DownloadStoriesAsync();

					// Update action text
					var converter = new StoryActionTextConverter();
					itemExpander.SubHeaderText = converter.Convert(friend, typeof(string), null, "") as string;

					e.Handled = true;
					await downloadStoriesTask;

					break;
				}
			}
		}
	}
}
