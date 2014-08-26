using System;
using System.Windows.Input;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;
using ColdSnap.Common;
using ColdSnap.Dialogs;
using ColdSnap.Helpers;
using SnapDotNet;

namespace ColdSnap.ViewModels.Sections
{
	public class FriendsSectionViewModel
		: BaseViewModel
	{
		public FriendsSectionViewModel()
		{
			ChangeDisplayNameCommand = new RelayCommand<Friend>(ChangeDisplayName);
			BlockFriendCommand = new RelayCommand<Friend>(BlockFriend);
			UnBlockFriendCommand = new RelayCommand<Friend>(UnBlockFriend);
		}

		public ICommand ChangeDisplayNameCommand
		{
			get { return _changeDisplayNameCommand; }
			set { SetValue(ref _changeDisplayNameCommand, value); }
		}
		private ICommand _changeDisplayNameCommand;

		public ICommand BlockFriendCommand
		{
			get { return _blockFriendCommand; }
			set { SetValue(ref _blockFriendCommand, value); }
		}
		private ICommand _blockFriendCommand;

		public ICommand UnBlockFriendCommand
		{
			get { return _unBlockFriendCommand; }
			set { SetValue(ref _unBlockFriendCommand, value); }
		}
		private ICommand _unBlockFriendCommand;

		public async void ChangeDisplayName(Friend friend)
		{
			var dialog = new ChangeDisplayNameDialog(friend.FriendlyName);
			var response = await dialog.ShowAsync();
			if (response != ContentDialogResult.Primary || dialog.NewDisplayName == null)
				return;

			await ProgressHelper.ShowStatusBarAsync(App.Strings.GetString("StatusBarChangeDisplayName"));
			var success = await friend.UpdateDisplayName(dialog.NewDisplayName, Account);
			await ProgressHelper.HideStatusBarAsync();
			if (!success)
			{
				// tell user it went tits up
				var alertDialog = new MessageDialog(App.Strings.GetString("MessageDialogChangeDisplayFailedTitle"),
					App.Strings.GetString("MessageDialogChangeDisplayFailedContent"));
				await alertDialog.ShowAsync();
			}
			else
			{
				// Save
				await StateManager.Local.SaveAccountStateAsync(Account);
			}

			Account.UpdateSortedFriends();
		}

		public async void BlockFriend(Friend friend)
		{
			await ProgressHelper.ShowStatusBarAsync(App.Strings.GetString("StatusBarBlockFriend"));
			var success = await friend.UpdateFriend(FriendAction.Block, Account);
			await ProgressHelper.HideStatusBarAsync();
			if (!success)
			{
				// tell user it went tits up
				var alertDialog = new MessageDialog(App.Strings.GetString("MessageDialogBlockFriendFailedTitle"),
					App.Strings.GetString("MessageDialogBlockFriendFailedContent"));
				await alertDialog.ShowAsync();
			}
			else
			{
				// Save
				friend.FriendRequestState = FriendRequestState.Blocked;
				await StateManager.Local.SaveAccountStateAsync(Account);
			}

			Account.UpdateSortedFriends();
		}

		public async void UnBlockFriend(Friend friend)
		{
			await ProgressHelper.ShowStatusBarAsync(App.Strings.GetString("StatusBarUnBlockFriend"));
			var success = await friend.UpdateFriend(FriendAction.Unblock, Account);
			await ProgressHelper.HideStatusBarAsync();
			if (!success)
			{
				// tell user it went tits up
				var alertDialog = new MessageDialog(App.Strings.GetString("MessageDialogUnBlockFriendFailedTitle"), 
					App.Strings.GetString("MessageDialogUnBlockFriendFailedContent"));
				await alertDialog.ShowAsync();
			}
			else
			{
				// Save
				friend.FriendRequestState = FriendRequestState.Accepted;
				await StateManager.Local.SaveAccountStateAsync(Account);
			}
			Account.UpdateSortedFriends();
		}
	}
}
