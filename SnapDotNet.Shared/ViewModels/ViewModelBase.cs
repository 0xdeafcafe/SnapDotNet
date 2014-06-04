using System.Windows.Input;
using SnapDotNet.Apps.Common;
using SnapDotNet.Apps.Pages.SignedIn;
using SnapDotNet.Core.Miscellaneous.Helpers;
using SnapDotNet.Core.Snapchat.Api;
using SnapDotNet.Core.Snapchat.Models;

// ReSharper disable ConvertToLambdaExpression
namespace SnapDotNet.Apps.ViewModels
{
	public class ViewModelBase
		: NotifyPropertyChangedBase
	{
		public ViewModelBase()
		{
			GoToSettingsCommand = new RelayCommand(() =>
			{
#if WINDOWS_PHONE_APP
				App.CurrentFrame.Navigate((typeof (SettingsPage)));
#endif
			});
			RefreshCommand = new RelayCommand(App.UpdateSnapchatData);
			LogoutCommand = new RelayCommand(async () => await App.LogoutAsync());
		}

		public SnapchatManager Manager
		{
			get { return App.SnapchatManager; }
		}

		public Account Account
		{
			get { return App.SnapchatManager.Account; }
		}

		public ApplicationSettings ApplicationSettings
		{
			get { return App.Settings; }
		}

		public ICommand RefreshCommand
		{
			get { return _refreshCommand; }
			set { SetField(ref _refreshCommand, value); }
		}
		private ICommand _refreshCommand;

		public ICommand GoToSettingsCommand
		{
			get { return _goToSettingsCommand; }
			set { SetField(ref _goToSettingsCommand, value); }
		}
		private ICommand _goToSettingsCommand;

		public ICommand LogoutCommand
		{
			get { return _logoutCommand; }
			set { SetField(ref _logoutCommand, value); }
		}
		private ICommand _logoutCommand;
	}
}
