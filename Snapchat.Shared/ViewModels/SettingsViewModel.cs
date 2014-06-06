using System.Windows.Input;
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
			get { return AppSettings.Get<bool>("LiveTileEnabled", defaultValue: true); }
			set
			{
				AppSettings.Set("LiveTileEnabled", value);
				OnNotifyPropertyChanged();
			}
		}

		/// <summary>
		/// Gets or sets whether toast notifications should be enabled.
		/// </summary>
		public bool ToastsEnabled
		{
			get { return AppSettings.Get<bool>("ToastsEnabled", defaultValue: true); }
			set
			{
				AppSettings.Set("ToastsEnabled", value);
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
			get { return AppSettings.Get<bool>("FrontCameraMirrorEffect", defaultValue: true); }
			set
			{
				AppSettings.Set("FrontCameraMirrorEffect", value);
				OnNotifyPropertyChanged();
			}
		}

		public AutomaticallyDownloadSnapsMode DownloadSnapsMode
		{
			get { return AppSettings.Get<AutomaticallyDownloadSnapsMode>("AutomaticallyDownloadSnapsMode", defaultValue: AutomaticallyDownloadSnapsMode.WiFi); }
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
			}
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