using System.Windows.Input;
using ColdSnap.Common;
using ColdSnap.Helpers;
using SnapDotNet;
using Windows.UI.Xaml;
using ColdSnap.Pages;
using System.Threading.Tasks;
using Windows.UI.Core;

namespace ColdSnap.ViewModels
{
	public sealed class MainPageViewModel
		: BaseViewModel
	{
		public MainPageViewModel()
		{
			RefreshCommand = new RelayCommand(() => RefreshContentAsync());
		}

		public ICommand RefreshCommand
		{
			get { return _refreshCommand; }
			set { SetValue(ref _refreshCommand, value); }
		}
		private ICommand _refreshCommand;

		public async Task RefreshContentAsync()
		{
			await ProgressHelper.ShowStatusBarAsync(App.Strings.GetString("StatusBarUpdating"));

			try
			{
				Window.Current.Dispatcher.RunAsync(CoreDispatcherPriority.Low, async delegate
				{
					await Account.UpdateAccountAsync();
				});
				await Task.Run(() => StateManager.Local.SaveAccountStateAsync(Account));
			}
			catch (InvalidCredentialsException)
			{
				Window.Current.Navigate(typeof(StartPage), Account);
			}
#if !DEBUG
			catch { }
#endif
			finally
			{
				var hideTask = ProgressHelper.HideStatusBarAsync();
			}
		}
	}
}
