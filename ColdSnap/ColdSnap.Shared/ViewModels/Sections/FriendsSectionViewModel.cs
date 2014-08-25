using System;
using System.Windows.Input;
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

			await ProgressHelper.ShowStatusBarAsync("Updating Display Name...");
			var newName = dialog.NewDisplayName;

			await ProgressHelper.HideStatusBarAsync();
		}
	}
}
