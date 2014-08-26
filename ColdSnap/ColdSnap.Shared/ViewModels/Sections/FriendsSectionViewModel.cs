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
		}

		public ICommand ChangeDisplayNameCommand
		{
			get { return _changeDisplayNameCommand; }
			set { SetValue(ref _changeDisplayNameCommand, value); }
		}
		private ICommand _changeDisplayNameCommand;

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
	}
}
