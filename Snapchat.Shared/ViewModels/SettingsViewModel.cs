using System.Linq;
using System.Windows.Input;
using Windows.UI;
using Windows.UI.Xaml.Media;
using Snapchat.Common;
using System;
using SnapDotNet.Core.Miscellaneous.Models.Atlas;
using SnapDotNet.Core.Snapchat.Models.New;
using Windows.UI.Xaml;
using Snapchat.Helpers;

namespace Snapchat.ViewModels
{
	public sealed class SettingsViewModel
		: BaseViewModel
	{
		public SettingsViewModel()
		{
			UpgradeProCommand = new RelayCommand(UpgradeToPro);

			//var timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
			//timer.Tick += delegate { ExplicitOnNotifyPropertyChanged("NextReplay"); };
			//timer.Start();
		}

		/// <summary>
		/// Gets the command to upgrade to Pro status.
		/// </summary>
		public ICommand UpgradeProCommand
		{
			get { return _upgradeProCommand; }
			private set { TryChangeValue(ref _upgradeProCommand, value); }
		}
		private ICommand _upgradeProCommand;

		/// <summary>
		/// Gets or sets whether the current user has upgraded to Pro status.
		/// </summary>
		public bool IsProUser
		{
			get { return _isProUser; }
			set { TryChangeValue(ref _isProUser, value); }
		}

		private bool _isProUser = true;

		/// <summary>
		/// Gets or sets whether the app tile should be live.
		/// </summary>
		public bool LiveTileEnabled
		{
			get { return AppSettings.Get("LiveTileEnabled", true); }
			set
			{
				AppSettings.Set("LiveTileEnabled", value);
				OnNotifyPropertyChanged();
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
				OnNotifyPropertyChanged();
				ExplicitOnNotifyPropertyChanged("TileTransparencyUiExampleBrush");
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

		#region Notifications

		/// <summary>
		/// Gets or sets whether snap notifications should be enabled.
		/// </summary>
		public bool SnapNotifications
		{
			get { return AppSettings.Get("SnapNotifications", true); }
			set
			{
				UpdateInstantNotifications(InstantNotifications, (() =>
				{
					AppSettings.Set("SnapNotifications", value);
					ExplicitOnNotifyPropertyChanged("SnapNotifications");
				}));
			}
		}

		/// <summary>
		/// Gets or sets whether chat notifications should be enabled.
		/// </summary>
		public bool ChatNotifications
		{
			get { return AppSettings.Get("ChatNotifications", true); }
			set
			{
				UpdateInstantNotifications(InstantNotifications, (() =>
				{
					AppSettings.Set("ChatNotifications", value);
					ExplicitOnNotifyPropertyChanged("ChatNotifications");
				}));
			}
		}

		/// <summary>
		/// Gets or sets whether screenshot notifications should be enabled.
		/// </summary>
		public bool ScreenshotNotifications
		{
			get { return AppSettings.Get("ScreenshotNotifications", true); }
			set
			{
				UpdateInstantNotifications(InstantNotifications, (() =>
				{
					AppSettings.Set("ScreenshotNotifications", value);
					ExplicitOnNotifyPropertyChanged("ScreenshotNotifications");
				}));
			}
		}
		
		/// <summary>
		/// Gets or sets whether instant notifications should be enabled.
		/// </summary>
		public bool InstantNotifications
		{
			get { return AppSettings.Get("InstantNotifications", false); }
			set
			{
				UpdateInstantNotifications(value, (() =>
				{
					AppSettings.Set("InstantNotifications", value);
					ExplicitOnNotifyPropertyChanged("InstantNotifications");
				}));
			}
		}

		/// <summary>
		/// Gets or sets whether notification UI elements should be enabled.
		/// </summary>
		public Boolean NotificationSettingsEnabled
		{
			get { return _notificationSettingsEnabled; }
			set { TryChangeValue(ref _notificationSettingsEnabled, value); }
		}
		private Boolean _notificationSettingsEnabled = true;

		private async void UpdateInstantNotifications(bool enabled, Action saveSettings)
		{
			// Tell UI we're updating
			await ProgressHelper.ShowStatusBarAsync("Saving...");

			// Disable the checkbox
			NotificationSettingsEnabled = false;

			// Get the user object (or create it), update the settings in it, and send it back on its way
			var create = false;
			var user = (await App.MobileService.GetTable<User>()
				.Where(
					u =>
						u.DeviceId == App.DeviceIdent && u.SnapchatUsername == App.SnapchatManager.Username &&
						u.AuthToken == App.SnapchatManager.AuthToken).ToListAsync()).FirstOrDefault();

			if (user == null)
			{
				user = new User
				{
					DeviceId = App.DeviceIdent,
					Probation = false,
					LastPushServed = DateTime.UtcNow,
					Subscribed = true,
					SnapchatUsername = App.SnapchatManager.Username
				};
				create = true;
			}
			user.AuthToken = App.SnapchatManager.AuthToken;
			user.AuthTokenExpired = false;
			user.Subscribed = enabled;
			user.ChatNotify = ChatNotifications;
			user.ScreenshotNotify = ScreenshotNotifications;
			user.SnapNotify = SnapNotifications;
			user.NewUser = true;
			user.NextUpdate = DateTime.UtcNow;
			if (create) await App.MobileService.GetTable<User>().InsertAsync(user);
			else await App.MobileService.GetTable<User>().UpdateAsync(user);

			// Set App Settings
			saveSettings();
			
			// Enable the checkbox
			NotificationSettingsEnabled = true;

			// Hide Updating UI
			await ProgressHelper.HideStatusBarAsync();
		}

		#endregion

		/// <summary>
		/// Gets or sets whether snaps are visible as long a finger is placed on the screen.
		/// </summary>
		public bool HoldToViewSnap
		{
			get { return AppSettings.Get<bool>("HoldToViewSnap"); }
			set
			{
				AppSettings.Set("HoldToViewSnap", value);
				OnNotifyPropertyChanged();
			}
		}

		/// <summary>
		/// Gets or sets whether the front camera should be flipped horizontally.
		/// </summary>
		public bool FrontCameraMirrorEffect
		{
			get { return AppSettings.Get("FrontCameraMirrorEffect", true); }
			set
			{
				AppSettings.Set("FrontCameraMirrorEffect", value);
				OnNotifyPropertyChanged();
			}
		}

		public AutomaticallyDownloadSnapsMode DownloadSnapsMode
		{
			get { return AppSettings.Get("AutomaticallyDownloadSnapsMode", AutomaticallyDownloadSnapsMode.WiFi); }
			set
			{
				AppSettings.Set("AutomaticallyDownloadSnapsMode", (int) value);
				OnNotifyPropertyChanged();
			}
		}

		/// <summary>
		/// Gets or sets the number of best friends.
		/// </summary>
		public int NumberOfBestFriends
		{
			get { return Account.NumberOfBestFriends; }
			set
			{
				Account.NumberOfBestFriends = value;
				OnNotifyPropertyChanged();

				UpdateBff(value);
			}
		}

		private async void UpdateBff(int value)
		{
			// Tell UI we're updating
			await ProgressHelper.ShowStatusBarAsync("Saving...");

			// Update
			await App.SnapchatManager.Endpoints.SetBestFriendCountAsync(value);

			// Hide Updating UI
			await ProgressHelper.HideStatusBarAsync();
		}

		/// <summary>
		/// Gets or sets the username of the account.
		/// </summary>
		public string Username
		{
			get { return Account.Username; }
			set
			{
				Account.Username = value;
				OnNotifyPropertyChanged();
			}
		}

		public string Email
		{
			get { return Account.Email; }
			set
			{
				Account.Email = value;
				OnNotifyPropertyChanged();
			}
		}

		public string Phone
		{
			get { return Account.SnapchatPhoneNumber; }
			set
			{
				Account.SnapchatPhoneNumber = value;
				OnNotifyPropertyChanged();
			}
		}

		public StoryPrivacy StoryPrivacy
		{
			get { return Account.StoryPrivacy; }
			set
			{
				Account.StoryPrivacy = value;
				OnNotifyPropertyChanged();
			}
		}

		public AccountPrivacy AccountPrivacy
		{
			get { return Account.AccountPrivacy; }
			set
			{
				Account.AccountPrivacy = value;
				OnNotifyPropertyChanged();
			}
		}

		public TimeSpan NextReplay
		{
			get { return DateTime.Now - Account.LastReplayedSnap; }
		}

		private void UpgradeToPro()
		{
			// TODO: Go to microtransaction page
		}
	}

	public enum AutomaticallyDownloadSnapsMode
	{
		Disabled,
		WiFi,
		Always
	}
}