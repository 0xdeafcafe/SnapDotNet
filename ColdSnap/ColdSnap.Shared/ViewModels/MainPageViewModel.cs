using System.Windows.Input;
using ColdSnap.Common;
using ColdSnap.Helpers;

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
			await ProgressHelper.ShowStatusBarAsync("Updating...");

			await Account.UpdateAccountAsync();

			await ProgressHelper.HideStatusBarAsync();
		}
	}
}
