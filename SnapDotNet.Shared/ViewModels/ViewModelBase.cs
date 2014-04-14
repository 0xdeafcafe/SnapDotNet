using System.Windows.Input;
using SnapDotNet.Apps.Common;
using SnapDotNet.Apps.Pages.SignedIn;
using SnapDotNet.Core.Miscellaneous.Helpers;
using SnapDotNet.Core.Snapchat.Api;
using SnapDotNet.Core.Snapchat.Models;

namespace SnapDotNet.Apps.ViewModels
{
	public class ViewModelBase
		: NotifyPropertyChangedBase
	{
		public ViewModelBase()
		{
			GoToSettingsCommand = new RelayCommand(() => App.CurrentFrame.Navigate((typeof(SettingsPage))));
			RefreshCommand = new RelayCommand(App.UpdateSnapchatData);
		}

		public SnapChatManager Manager
		{
			get { return App.SnapChatManager; }
		}

		public Account Account
		{
			get { return App.SnapChatManager.Account; }
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
	}
}
