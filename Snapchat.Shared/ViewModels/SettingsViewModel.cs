using System.Windows.Input;
using Windows.UI;
using Windows.UI.Xaml.Media;
using Snapchat.Common;
using System;
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

			var timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
			timer.Tick += delegate { OnNotifyPropertyChanged("NextReplay"); };
			timer.Start();
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
		private bool _isProUser;

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

		/// <summary>
		/// Gets or sets whether snap notifications should be enabled.
		/// </summary>
		public bool SnapNotificationsEnabled
		{
			get { return AppSettings.Get("SnapNotificationsEnabled", true); }
			set
			{
				AppSettings.Set("SnapNotificationsEnabled", value);
				OnNotifyPropertyChanged();
			}
		}

		/// <summary>
		/// Gets or sets whether chat notifications should be enabled.
		/// </summary>
		public bool ChatNotificationsEnabled
		{
			get { return AppSettings.Get("ChatNotificationsEnabled", true); }
			set
			{
				AppSettings.Set("ChatNotificationsEnabled", value);
				OnNotifyPropertyChanged();
			}
		}

		/// <summary>
		/// Gets or sets whether screenshot notifications should be enabled.
		/// </summary>
		public bool ScreenshotNotificationsEnabled
		{
			get { return AppSettings.Get("ScreenshotNotificationsEnabled", true); }
			set
			{
				AppSettings.Set("ScreenshotNotificationsEnabled", value);
				OnNotifyPropertyChanged();
			}
		}

		/// <summary>
		/// Gets or sets whether instant notifications should be enabled.
		/// </summary>
		public bool InstantNotifications
		{
			get { return AppSettings.Get<bool>("InstantNotifications"); }
			set
			{
				AppSettings.Set("InstantNotifications", value);
				OnNotifyPropertyChanged();
			}
		}

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
			await ProgressHelper.ShowStatusBarAsync("Updating...");

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