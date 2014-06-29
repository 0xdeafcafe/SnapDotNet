using System.Linq;
using Windows.Phone.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Snapchat.Controls;
using Snapchat.Helpers;
using Snapchat.ViewModels.PageContents;
using SnapDotNet.Core.Snapchat.Models.New;

namespace Snapchat.Pages.PageContents
{
	public sealed partial class FriendsPageContent
	{
		public FriendsViewModel ViewModel { get; private set; }

		public FriendsPageContent()
		{
			InitializeComponent();
			DataContext = ViewModel = new FriendsViewModel();

			HardwareButtons.CameraPressed += delegate
			{
				// Remove focus from SearchBox
				MainPage.Singleton.CapturePhotoButton.Focus(FocusState.Programmatic);
			};
		}

		private void SearchBox_GotFocus(object sender, RoutedEventArgs e)
		{
			MainPage.Singleton.HideBottomAppBar();
		}

		private void SearchBox_LostFocus(object sender, RoutedEventArgs e)
		{
			MainPage.Singleton.RestoreBottomAppBar();
		}

		private void AddFriendsIcon_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
		{
			MainPage.Singleton.GoToAddFriendsPage();
		}

		private void ExpanderView_OnExpanded(object sender, bool e)
		{
			var expander = sender as ExpanderView;
			if (expander == null) return;

			// Hide the others
			foreach (var itemExpander in FriendsListView.Items.Select(
				item => VariousHelpers.FindVisualChild<ExpanderView>(FriendsListView.ItemContainerGenerator.ContainerFromItem(item)))
				.Where(itemExpander => itemExpander.Tag != expander.Tag))
			{
				itemExpander.Contract();
			}
		}

		private void EditFriendButton_OnClick(object sender, RoutedEventArgs e)
		{
			var button = sender as Button;
			if (button == null) return;
			var context = button.DataContext as Friend;
			if (context == null) return;

			var flyout = new MenuFlyout();
			if (flyout.Items == null) return;
			flyout.Items.Add(new MenuFlyoutItem { Text = App.Strings.GetString("RemoveFriend"), CommandParameter = context }); // Command = ViewModel.RemoveFriendCommand, 
			flyout.Items.Add(new MenuFlyoutItem { Text = App.Strings.GetString("ChangeFriendsDisplay"), Command = ViewModel.ChangeFriendDisplayNameCommand, CommandParameter = context });
			flyout.Items.Add(context.Type == FriendRequestState.Blocked
				? new MenuFlyoutItem { Text = App.Strings.GetString("UnblockFriend"), CommandParameter = context } // Command = ViewModel.UnBlockFriendCommand, 
				: new MenuFlyoutItem { Text = App.Strings.GetString("BlockFriend"), CommandParameter = context }); // Command = ViewModel.BlockFriendCommand, 
			flyout.Opening += delegate
			{
				VisualStateManager.GoToState(button, "FlyoutOpening", false);
			};
			flyout.Closed += delegate
			{
				VisualStateManager.GoToState(button, "FlyoutClosed", false);
			};
			flyout.ShowAt(button);
		}

		private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			ViewModel.FilterText = (sender as TextBox).Text;
		}
	}
}
