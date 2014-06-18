using System;
using System.Windows.Input;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;
using Snapchat.Common;
using Snapchat.Dialogs;
using Snapchat.Helpers;
using SnapDotNet.Core.Miscellaneous.Helpers;
using SnapDotNet.Core.Snapchat.Models.New;

namespace Snapchat.ViewModels.PageContents
{
	public class FriendsViewModel
		 : BaseViewModel
	{
		public FriendsViewModel()
		{
			ChangeFriendDisplayNameCommand = new RelayCommand<Friend>(ChangeFriendDisplayName);
		}

		public ICommand ChangeFriendDisplayNameCommand
		{
			get { return _changeFriendDisplayNameCommand; }
			set { TryChangeValue(ref _changeFriendDisplayNameCommand, value); }
		}
		private ICommand _changeFriendDisplayNameCommand;

		private static async void ChangeFriendDisplayName(Friend friend)
		{
			var contentDialog = new ChangeFriendDisplayNameDialog(friend.FriendlyName);
			var result = await contentDialog.ShowAsync();
			if (result != ContentDialogResult.Primary) return;
			var oldDisplayName = friend.Display;
			friend.Display = contentDialog.NewDisplayName;

			await ProgressHelper.ShowStatusBarAsync(App.Strings.GetString("StatusBarChangeDisplay"));
			try
			{
				await App.SnapchatManager.Endpoints.ChangeFriendDisplayNameAsync(friend.Name, contentDialog.NewDisplayName);
			}
			catch (Exception ex)
			{
				SnazzyDebug.WriteLine(ex);
				friend.Display = oldDisplayName;

				new MessageDialog(String.Format(App.Strings.GetString("ChangeFriendDisplayNameErrorContent"), friend.FriendlyName),
					App.Strings.GetString("ChangeFriendDisplayNameErrorTitle")).ShowAsync();
			}

			await ProgressHelper.HideStatusBarAsync();
		}
	}
}
