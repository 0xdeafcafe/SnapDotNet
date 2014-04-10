using Windows.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;
using SnapDotNet.Apps.Common;
using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using SnapDotNet.Apps.ViewModels.SignedIn;

namespace SnapDotNet.Apps.Pages.SignedIn
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class FriendsPage
	{
		public FriendsViewModel ViewModel { get; private set; }

		private readonly NavigationHelper _navigationHelper;

		public FriendsPage()
		{
			InitializeComponent();

			DataContext = ViewModel = new FriendsViewModel();

			_navigationHelper = new NavigationHelper(this);
			_navigationHelper.LoadState += NavigationHelper_LoadState;
			_navigationHelper.SaveState += NavigationHelper_SaveState;
		}

		#region NavigationHelper registration

		/// <summary>
		/// Gets the <see cref="NavigationHelper"/> associated with this <see cref="Page"/>.
		/// </summary>
		public NavigationHelper NavigationHelper
		{
			get { return _navigationHelper; }
		}

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
			_navigationHelper.OnNavigatedTo(e);
		}

		protected override void OnNavigatedFrom(NavigationEventArgs e)
		{
			_navigationHelper.OnNavigatedFrom(e);
		}

		#endregion

		private void FriendPerson_OnHolding(object sender, HoldingRoutedEventArgs e)
		{
			if (e.HoldingState != HoldingState.Started) return;

			var button = sender as Button;
			if (button == null) return;
			var flyout = new MenuFlyout();
			if (flyout.Items == null) return;
			flyout.Items.Add(new MenuFlyoutItem { Text = "Remove" });
			flyout.Items.Add(new MenuFlyoutItem { Text = "Change Display Name", Command = ViewModel.ChangeDisplayNameCommand, CommandParameter = button.DataContext });
			flyout.Items.Add(new MenuFlyoutItem { Text = "Block" });
			flyout.ShowAt((FrameworkElement) sender);
		}

		private void BlockedPerson_OnHolding(object sender, HoldingRoutedEventArgs e)
		{
			if (e.HoldingState != HoldingState.Started) return;

			var button = sender as Button;
			if (button == null) return;
			var flyout = new MenuFlyout();
			if (flyout.Items == null) return;
			flyout.Items.Add(new MenuFlyoutItem { Text = "Remove" });
			flyout.Items.Add(new MenuFlyoutItem { Text = "Change Display Name", Command = ViewModel.ChangeDisplayNameCommand, CommandParameter = button.DataContext });
			flyout.Items.Add(new MenuFlyoutItem { Text = "Unblock" });
			flyout.ShowAt((FrameworkElement)sender);
		}
	}
}
