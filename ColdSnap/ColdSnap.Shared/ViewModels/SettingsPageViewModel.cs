using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using ColdSnap.Atlas;
using ColdSnap.Common;
using ColdSnap.Helpers;
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

				if (!value) IsInstantNotificationsEnabled = false;
			}
		}

		/// <summary>
		/// Gets or sets whether instant notifications are enabled.
		/// </summary>
		public bool IsInstantNotificationsEnabled
		{
			get { return AppSettings.Get("IsInstantNotificationsEnabled", false); }
			set
			{
				AppSettings.Set("IsInstantNotificationsEnabled", value);
				OnPropertyChanged();

				ToggleInstantNotificationsAsync(value);
			}
		}

		/// <summary>
		/// Gets a boolean value indicating whether the instant notifications setting was
		/// saved to the cloud.
		/// </summary>
		public bool IsInstantNotificationsSettingSaved
		{
			get { return _isInstantNotificationsSettingSaved; }
			private set { SetValue(ref _isInstantNotificationsSettingSaved, value); }
		}
	    private bool _isInstantNotificationsSettingSaved = true;

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

				return string.Format(App.Strings.GetString("ReplayFormattedText"), nextReplay.Hours, nextReplay.Minutes);
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

	    private async Task ToggleInstantNotificationsAsync(bool enabled)
	    {
		    IsInstantNotificationsSettingSaved = false;
		    await StatusBarHelper.ShowStatusBarAsync("");

			// Get or create user object, update the settings and send it back on its way.
		    bool create = false;
		    var user = (await App.MobileService.GetTable<AtlasUser>()
			    .Where(
				    u =>
					    u.DeviceId == App.DeviceIdent && u.SnapchatUsername == Account.Username &&
					    u.AuthToken == Account.AuthToken).ToListAsync()).FirstOrDefault();

		    if (user == null)
		    {
			    user = new AtlasUser
			    {
				    DeviceId = App.DeviceIdent,
					Probation = false,
					LastPushServed = DateTime.UtcNow,
					Subscribed = true,
					SnapchatUsername = Account.Username
			    };
			    create = true;
		    }
			user.AuthToken = Account.AuthToken;
			user.AuthTokenExpired = false;
			user.Subscribed = enabled;
			user.ChatNotify = true;
			user.ScreenshotNotify = true;
			user.SnapNotify = true;
			user.NewUser = true;
			user.NextUpdate = DateTime.UtcNow;
		    if (create) await App.MobileService.GetTable<AtlasUser>().InsertAsync(user);

			IsInstantNotificationsSettingSaved = true;
		    await StatusBarHelper.HideStatusBarAsync();

		    Debug.WriteLine("[SettingsPageViewModel] Updated user info on Atlas");
	    }
    }
}
