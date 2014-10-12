using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using ColdSnap.Common;
using ColdSnap.Pages;
using SnapDotNet.Utilities;

namespace ColdSnap.ViewModels
{
    public sealed class SettingsPageViewModel
		: BaseViewModel
    {
	    public SettingsPageViewModel()
	    {
			UpgradeProCommand = new RelayCommand(UpgradeToPro);
			LogoutCommand = new RelayCommand(async () => await LogoutAsync());
	    }

		/// <summary>
		/// Gets the command to upgrade to Pro status.
		/// </summary>
		public ICommand UpgradeProCommand
		{
			get { return _upgradeProCommand; }
			private set { SetValue(ref _upgradeProCommand, value); }
		}
		private ICommand _upgradeProCommand;

		/// <summary>
		/// Gets the command to log out.
		/// </summary>
		public ICommand LogoutCommand
		{
			get { return _logoutCommand; }
			private set { SetValue(ref _logoutCommand, value); }
		}
		private ICommand _logoutCommand;

		/// <summary>
		/// Gets or sets whether the app tile should be live.
		/// </summary>
		public bool IsLiveTileEnabled
		{
			get { return AppSettings.Get("IsLiveTileEnabled", true); }
			set
			{
				AppSettings.Set("IsLiveTileEnabled", value);
				OnPropertyChanged();
			}
		}

		/// <summary>
		/// Gets or sets whether the app tile should be transparent should be enabled.
		/// </summary>
		public bool IsTileTransparencyEnabled
		{
			get { return AppSettings.Get("IsTileTransparencyEnabled", true); }
			set
			{
				AppSettings.Set("IsTileTransparencyEnabled", value);
				OnPropertyChanged();
				OnPropertyChanged(() => TileTransparencyUiExampleBrush);
			}
		}

		/// <summary>
		/// Gets or sets whether toast notifications are enabled.
		/// </summary>
		public bool IsToastNotificationsEnabled
		{
			get { return AppSettings.Get("IsToastNotificationsEnabled", true); }
			set
			{
				AppSettings.Set("IsToastNotificationsEnabled", value);
				OnPropertyChanged();
			}
		}

		/// <summary>
		/// Gets the example colour of the app tile for the phone example
		/// </summary>
		public SolidColorBrush TileTransparencyUiExampleBrush
		{
			get
			{
				return IsTileTransparencyEnabled
					? Application.Current.Resources["PhoneAccentBrush"] as SolidColorBrush
					: new SolidColorBrush(Color.FromArgb(0xFF, 0x3C, 0xB2, 0xE2));
			}
		}

		public string NextReplay
		{
			get
			{
				var nextReplay = DateTime.Now - Account.LastReplayed;
				if (nextReplay.TotalHours >= 24)
				{
					return App.Strings.GetString("ReplayAvailableText");
				}
				else
				{
					return string.Format(App.Strings.GetString("ReplayFormattedText"), nextReplay.Hours, nextReplay.Minutes);
				}
			}
		}

		private void UpgradeToPro()
		{
			// TODO: To be implemented
		}

		private async Task LogoutAsync()
		{
			var logoutTask = Account.LogoutAsync();
			var deleteFilesTask = StorageManager.Local.EmptyFolderAsync();
			Window.Current.Navigate(typeof(StartPage));
			await logoutTask;
			await deleteFilesTask;
		}
    }
}
