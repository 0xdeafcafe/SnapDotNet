using System.Windows.Input;
using ColdSnap.Common;
using ColdSnap.Helpers;
using SnapDotNet;

namespace ColdSnap.ViewModels
{
	public sealed class MainPageViewModel
		: BaseViewModel
	{
		public MainPageViewModel()
		{
			RefreshCommand = new RelayCommand(RefreshContent);
		}

		public ICommand RefreshCommand
		{
			get { return _refreshCommand; }
			set { SetValue(ref _refreshCommand, value); }
		}
		private ICommand _refreshCommand;

		public async void RefreshContent()
		{
			await ProgressHelper.ShowStatusBarAsync(App.Strings.GetString("StatusBarUpdating"));

			await Account.UpdateAccountAsync(); 
			await StateManager.Local.SaveAccountStateAsync(Account);

			await ProgressHelper.HideStatusBarAsync();
		}
	}
}
