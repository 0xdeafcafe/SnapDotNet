using System;
using System.Windows.Input;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using SnapDotNet.Apps.Common;
using SnapDotNet.Apps.Dialogs;
using SnapDotNet.Apps.Helpers;
using SnapDotNet.Core.Snapchat.Models;

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

		private static async void ChangeDisplayName(Friend friend)
		{
			await ProgressHelper.ShowStatusBar("Changing Display Name...");

#if WINDOWS_PHONE_APP
			var contentDialog = new ChangeDisplayNameDialog(friend.FriendlyName);
			var result = await contentDialog.ShowAsync();
			if (result != ContentDialogResult.Primary) return;
			await App.SnapChatManager.Endpoints.ChangeFriendDisplayNameAsync(friend.Name, contentDialog.NewDisplayName);
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
			await ProgressHelper.ShowStatusBar("Blocking...");

			var dialog = new MessageDialog("You will no longer recieve snaps from this person. But you can undo this action at any time.", 
				"Are you sure?");
			dialog.Commands.Add(new UICommand("Yes"));
			dialog.Commands.Add(new UICommand("Cancel", command => ProgressHelper.HideStatusBar().Wait()));
			var result = await dialog.ShowAsync();
			if (result.Label != "Yes") return;
			await App.SnapChatManager.Endpoints.SendFriendActionAsync(friend.Name, FriendAction.Block);

			friend.NotifyPropertyChanged("FriendRequestState");
			await ProgressHelper.HideStatusBar();
			App.UpdateSnapchatData();
		}

		private static async void UnblockFriend(Friend friend)
		{
			await ProgressHelper.ShowStatusBar("Unblocking...");

			var dialog = new MessageDialog("You will now recieve snaps from this person. But you can redo this action at any time.",
				"Are you sure?");
			dialog.Commands.Add(new UICommand("Yes"));
			dialog.Commands.Add(new UICommand("Cancel", command => ProgressHelper.HideStatusBar().Wait()));
			var result = await dialog.ShowAsync();
			if (result.Label != "Yes") return;
			await App.SnapChatManager.Endpoints.SendFriendActionAsync(friend.Name, FriendAction.Unblock);

			friend.NotifyPropertyChanged("FriendRequestState");
			await ProgressHelper.HideStatusBar();
			App.UpdateSnapchatData();
		}

		private static async void RemoveFriend(Friend friend)
		{
			await ProgressHelper.ShowStatusBar("Removing...");

			var dialog = new MessageDialog("This person will no longer appear on your friends list, but you might still get snaps from them depending on your privacy settings.",
				"Are you sure?");
			dialog.Commands.Add(new UICommand("Yes"));
			dialog.Commands.Add(new UICommand("Cancel", command => ProgressHelper.HideStatusBar().Wait()));
			var result = await dialog.ShowAsync();
			if (result.Label != "Yes") return;
			await App.SnapChatManager.Endpoints.SendFriendActionAsync(friend.Name, FriendAction.Delete);

			friend.NotifyPropertyChanged("FriendRequestState");
			await ProgressHelper.HideStatusBar();
			App.UpdateSnapchatData();
		}
	}
}
