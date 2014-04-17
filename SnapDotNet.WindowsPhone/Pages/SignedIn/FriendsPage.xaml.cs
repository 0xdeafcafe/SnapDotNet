using Windows.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;
using SnapDotNet.Apps.Attributes;
using SnapDotNet.Apps.Common;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using SnapDotNet.Apps.ViewModels.SignedIn;
using SnapDotNet.Core.Snapchat.Models;

namespace SnapDotNet.Apps.Pages.SignedIn
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	[RequiresAuthentication]
	public sealed partial class FriendsPage
	{
		public FriendsViewModel ViewModel { get; private set; }

		private readonly NavigationHelper _navigationHelper;

		public FriendsPage()
		{
			InitializeComponent();

			DataContext = ViewModel = new FriendsViewModel(FriendsViewSource, PendingFriendsViewSource, 
				BlockedFriendsViewSource);

			_navigationHelper = new NavigationHelper(this);
			_navigationHelper.LoadState += NavigationHelper_LoadState;
			_navigationHelper.SaveState += NavigationHelper_SaveState;

#if !DEBUG
			Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().IsScreenCaptureEnabled = false;
#endif
		}

		#region NavigationHelper registration

		public NavigationHelper NavigationHelper
		{
			get { return _navigationHelper; }
		}

		private static void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
		{
		}

		private static void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
		{
		}

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
			flyout.Items.Add(new MenuFlyoutItem { Text = "Remove", Command = ViewModel.RemoveFriendCommand, CommandParameter = button.DataContext });
			flyout.Items.Add(new MenuFlyoutItem { Text = "Change Display Name", Command = ViewModel.ChangeDisplayNameCommand, CommandParameter = button.DataContext });
			flyout.Items.Add(((Friend) button.DataContext).FriendRequestState == FriendRequestState.Blocked
				? new MenuFlyoutItem {Text = "Unblock", Command = ViewModel.UnBlockFriendCommand, CommandParameter = button.DataContext}
				: new MenuFlyoutItem {Text = "Block", Command = ViewModel.BlockFriendCommand, CommandParameter = button.DataContext});
			flyout.ShowAt((FrameworkElement)sender);

			// TODO: Refresh CollectionViewSource's after these commands have been executed... Not sure how though. Matt?
		}

		private void FriendPerson_OnClick(object sender, RoutedEventArgs e)
		{
			var button = sender as Button;
			if (button == null) return;

			var friend = button.DataContext as Friend;
			if (friend == null) return;

			ViewModel.GoToFriendCommand.Execute(friend);
		}
	}
}
