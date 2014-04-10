using System;
using System.Windows.Input;
using Windows.UI.Xaml.Controls;
using SnapDotNet.Apps.Common;
using SnapDotNet.Apps.Dialogs;
using SnapDotNet.Core.Snapchat.Models;

namespace SnapDotNet.Apps.ViewModels.SignedIn
{
	public class FriendsViewModel 
		: ViewModelBase
	{
		public FriendsViewModel()
		{
			ChangeDisplayNameCommand = new RelayCommand<Friend>(ChangeDisplayName);
		}

		public ICommand ChangeDisplayNameCommand
		{
			get { return _changeDisplayNameCommand; }
			set { SetField(ref _changeDisplayNameCommand, value); }
		}
		private ICommand _changeDisplayNameCommand;

		private static async void ChangeDisplayName(Friend friend)
		{
#if WINDOWS_PHONE_APP
			var contentDialog = new ChangeDisplayNameDialog(friend.FriendlyName);
			var result = await contentDialog.ShowAsync();
			if (result != ContentDialogResult.Primary) return;
			await App.SnapChatManager.Endpoints.ChangeFriendDisplayNameAsync(friend.Name, contentDialog.NewDisplayName);

			App.UpdateSnapchatData();
#endif
		}
	}
}
