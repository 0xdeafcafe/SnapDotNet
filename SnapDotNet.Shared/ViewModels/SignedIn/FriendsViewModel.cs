using System;
using System.Windows.Input;
using Windows.System;
using Windows.UI.Popups;
using Windows.UI.Xaml.Data;
using SnapDotNet.Apps.Common;
using SnapDotNet.Apps.Helpers;
using SnapDotNet.Apps.Pages.SignedIn;
using SnapDotNet.Core.Snapchat.Models;
#if WINDOWS_PHONE_APP
using SnapDotNet.Apps.Dialogs;
using Windows.UI.Xaml.Controls;
#endif

namespace SnapDotNet.Apps.ViewModels.SignedIn
{
	public class FriendsViewModel 
		: ViewModelBase
	{
		public FriendsViewModel(CollectionViewSource friends, CollectionViewSource pending,
			CollectionViewSource blocked)
		{
			ChangeDisplayNameCommand = new RelayCommand<Friend>(ChangeDisplayName);
			BlockFriendCommand = new RelayCommand<Friend>(BlockFriend);
			UnBlockFriendCommand = new RelayCommand<Friend>(UnblockFriend);
			RemoveFriendCommand = new RelayCommand<Friend>(RemoveFriend);
			GetFriendsCommand = new RelayCommand(GetFriends);
			GoToFriendCommand = new RelayCommand<Friend>(GoToFriend);

			// Set up Collection View Sources
			FriendsViewSource = friends;
			PendingFriendsViewSource = pending;
			BlockedFriendsViewSource = blocked;
		}

		public CollectionViewSource FriendsViewSource
		{
			get { return _friendsViewSource; }
			set { SetField(ref _friendsViewSource, value); }
		}
		private CollectionViewSource _friendsViewSource;

		public CollectionViewSource PendingFriendsViewSource
		{
			get { return _pendingFriendsViewSource; }
			set { SetField(ref _pendingFriendsViewSource, value); }
		}
		private CollectionViewSource _pendingFriendsViewSource;

		public CollectionViewSource BlockedFriendsViewSource
		{
			get { return _blockedFriendsViewSource; }
			set { SetField(ref _blockedFriendsViewSource, value); }
		}
		private CollectionViewSource _blockedFriendsViewSource;

		public ICommand ChangeDisplayNameCommand
		{
			get { return _changeDisplayNameCommand; }
			set { SetField(ref _changeDisplayNameCommand, value); }
		}
		private ICommand _changeDisplayNameCommand;

		public ICommand BlockFriendCommand
		{
			get { return _blockFriendCommand; }
			set { SetField(ref _blockFriendCommand, value); }
		}
		private ICommand _blockFriendCommand;

		public ICommand UnBlockFriendCommand
		{
			get { return _unBlockFriendCommand; }
			set { SetField(ref _unBlockFriendCommand, value); }
		}
		private ICommand _unBlockFriendCommand;

		public ICommand RemoveFriendCommand
		{
			get { return _removeFriendCommand; }
			set { SetField(ref _removeFriendCommand, value); }
		}
		private ICommand _removeFriendCommand;

		public ICommand GetFriendsCommand
		{
			get { return _getFriendsCommand; }
			set { SetField(ref _getFriendsCommand, value); }
		}
		private ICommand _getFriendsCommand;

		public ICommand GoToFriendCommand
		{
			get { return _goToFriendCommand; }
			set { SetField(ref _goToFriendCommand, value); }
		}
		private ICommand _goToFriendCommand;

		private static async void ChangeDisplayName(Friend friend)
		{
			await ProgressHelper.ShowStatusBar(App.Loader.GetString("StatusBarChangeDisplay"));

#if WINDOWS_PHONE_APP
			var contentDialog = new ChangeDisplayNameDialog(friend.FriendlyName);
			var result = await contentDialog.ShowAsync();
			if (result != ContentDialogResult.Primary) return;
			await App.SnapchatManager.Endpoints.ChangeFriendDisplayNameAsync(friend.Name, contentDialog.NewDisplayName);
			friend.DisplayName = contentDialog.NewDisplayName;
#else
			// TODO: Windows 8 App logic
#endif
			friend.NotifyPropertyChanged("DisplayName");
			friend.NotifyPropertyChanged("Name");
			friend.NotifyPropertyChanged("FriendlyName");
			
			await ProgressHelper.HideStatusBar();
			App.UpdateSnapchatData();
		}

		private static async void BlockFriend(Friend friend)
		{
			await ProgressHelper.ShowStatusBar(App.Loader.GetString("StatusBarBlocking"));

			var dialog = new MessageDialog(App.Loader.GetString("BlockingDialogBody"), App.Loader.GetString("GenericCautionDialogHeader"));
			dialog.Commands.Add(new UICommand(App.Loader.GetString("Yes")));
			dialog.Commands.Add(new UICommand(App.Loader.GetString("Cancel"), command => ProgressHelper.HideStatusBar().Wait()));
			var result = await dialog.ShowAsync();
			if (result.Label != App.Loader.GetString("Yes")) return;
			await App.SnapchatManager.Endpoints.SendFriendActionAsync(friend.Name, FriendAction.Block);

			friend.NotifyPropertyChanged("FriendRequestState");
			await ProgressHelper.HideStatusBar();
			App.UpdateSnapchatData();
		}

		private static async void UnblockFriend(Friend friend)
		{
			await ProgressHelper.ShowStatusBar(App.Loader.GetString("StatusBarUnblocking"));

			var dialog = new MessageDialog(App.Loader.GetString("UnblockingDialogBody"), App.Loader.GetString("GenericCautionDialogHeader"));
			dialog.Commands.Add(new UICommand(App.Loader.GetString("Yes")));
			dialog.Commands.Add(new UICommand(App.Loader.GetString("Cancel"), command => ProgressHelper.HideStatusBar().Wait()));
			var result = await dialog.ShowAsync();
			if (result.Label != App.Loader.GetString("Yes")) return;
			await App.SnapchatManager.Endpoints.SendFriendActionAsync(friend.Name, FriendAction.Unblock);

			friend.NotifyPropertyChanged("FriendRequestState");
			await ProgressHelper.HideStatusBar();
			App.UpdateSnapchatData();
		}

		private static async void RemoveFriend(Friend friend)
		{
			await ProgressHelper.ShowStatusBar(App.Loader.GetString("StatusBarRemoving"));

			var dialog = new MessageDialog(App.Loader.GetString("RemovingDialogBody"), App.Loader.GetString("GenericCautionDialogHeader"));
			dialog.Commands.Add(new UICommand(App.Loader.GetString("Yes")));
			dialog.Commands.Add(new UICommand(App.Loader.GetString("Cancel"), command => ProgressHelper.HideStatusBar().Wait()));
			var result = await dialog.ShowAsync();
			if (result.Label != App.Loader.GetString("Yes")) return;
			await App.SnapchatManager.Endpoints.SendFriendActionAsync(friend.Name, FriendAction.Delete);

			friend.NotifyPropertyChanged("FriendRequestState");
			await ProgressHelper.HideStatusBar();
			App.UpdateSnapchatData();
		}

		private static async void GetFriends()
		{
			if (App.SnapchatManager.Account.CanViewMatureContent)
				await Launcher.LaunchUriAsync(new Uri("http://www.reddit.com/r/DirtySnapchat"));
			else
				await Launcher.LaunchUriAsync(new Uri("http://www.reddit.com/r/Snapchat"));
		}

		private static void GoToFriend(Friend friend)
		{
			App.CurrentFrame.Navigate(typeof (FriendPage), friend);
		}
	}
}
