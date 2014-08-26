using System.Linq;
using Windows.UI.Xaml.Controls;
using ColdSnap.Common;
using ColdSnap.Controls;
using ColdSnap.Helpers;
using ColdSnap.ViewModels.Sections;
using SnapDotNet;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;

namespace ColdSnap.Pages.Sections
{
	public sealed partial class FriendsSection
	{
		private ListView _friendsListView;

		public FriendsSection()
		{
			InitializeComponent();
			DataContext = new FriendsSectionViewModel();
		}

		/// <summary>
		/// Gets the view model of this section.
		/// </summary>
		public FriendsSectionViewModel ViewModel
		{
			get { return DataContext as FriendsSectionViewModel; }
		}

		public override void LoadState(LoadStateEventArgs e)
		{
			if (e.NavigationParameter is Account)
				ViewModel.Account = e.NavigationParameter as Account;
		}

		public override void SaveState(SaveStateEventArgs e) { }

		private void AddFriendsButton_Tapped(object sender, TappedRoutedEventArgs e)
		{
			// TODO: Fix the COM-Exception this causes xox
			Window.Current.Navigate(typeof(ManageFriendsPage), ViewModel.Account);
		}

		private void GoToCameraButton_Tapped(object sender, TappedRoutedEventArgs e)
		{
			MainPage.Singleton.GoToHubSection(HubContent.Camera);
		}

		private void ExpanderView_OnExpanded(object sender, bool e)
		{
			var expander = sender as ExpanderView;
			if (expander == null) return;

			if (_friendsListView.Items == null) return;

			// Hide the others
			foreach (
				var itemExpander in
					_friendsListView.Items.Select(item => _friendsListView.ContainerFromItem(item))
						.Select(VariousHelpers.FindVisualChild<ExpanderView>)
						.Where(itemExpander => itemExpander.Tag != expander.Tag))
				itemExpander.Contract();
		}

		private void EditFriendButton_OnClick(object sender, RoutedEventArgs e)
		{
			var button = sender as Button;
			if (button == null) return;
			var context = button.DataContext as Friend;
			if (context == null) return;

			var flyout = new MenuFlyout();
			if (flyout.Items == null) return;
			flyout.Items.Add(new MenuFlyoutItem { Text = App.Strings.GetString("FriendContextRemove"), CommandParameter = context });
			flyout.Items.Add(new MenuFlyoutItem { Text = App.Strings.GetString("FriendContextChangeName"), Command = ViewModel.ChangeDisplayNameCommand, CommandParameter = context });
			flyout.Items.Add(context.FriendRequestState == FriendRequestState.Blocked
				? new MenuFlyoutItem { Text = App.Strings.GetString("FriendContextUnblock"), CommandParameter = context }
				: new MenuFlyoutItem { Text = App.Strings.GetString("FriendContextBlock"), CommandParameter = context });

			flyout.ShowAt(button);
		}

		private void FriendsList_OnLoaded(object sender, RoutedEventArgs e)
		{
			// This is a hack to gain access to the ListView from outside the DataTemplate ;_;
			_friendsListView = (ListView) sender;
		}
	}
}