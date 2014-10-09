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
		public bool LiveTileEnabled
		{
			get { return AppSettings.Get("LiveTileEnabled", true); }
			set
			{
				AppSettings.Set("LiveTileEnabled", value);
				OnPropertyChanged();
			}
		}

		/// <summary>
		/// Gets or sets whether the app tile should be transparent should be enabled.
		/// </summary>
		public bool TileTransparencyEnabled
		{
			get { return AppSettings.Get("TileTransparencyEnabled", true); }
			set
			{
				AppSettings.Set("TileTransparencyEnabled", value);
				OnPropertyChanged();
				OnPropertyChanged(() => TileTransparencyUiExampleBrush);
			}
		}

		/// <summary>
		/// Gets the example colour of the app tile for the phone example
		/// </summary>
		public SolidColorBrush TileTransparencyUiExampleBrush
		{
			get
			{
				return TileTransparencyEnabled
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
