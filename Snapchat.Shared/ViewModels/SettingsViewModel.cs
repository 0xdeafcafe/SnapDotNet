using System.Windows.Input;
using Snapchat.Common;
using System;
using SnapDotNet.Core.Snapchat.Models.New;
using Windows.UI.Xaml;

namespace Snapchat.ViewModels
{
	public sealed class SettingsViewModel
		: BaseViewModel
	{
		public SettingsViewModel()
		{
			// TODO: Load from roaming settings
			LiveTileEnabled = true;
			ToastsEnabled = true;
			FrontCameraMirrorEffect = true;
			DownloadSnapsMode = AutomaticallyDownloadSnapsMode.WiFi;

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
		/// Gets or sets whether the app tile should be live.
		/// </summary>
		public bool LiveTileEnabled
		{
			get { return _liveTile; }
			set { TryChangeValue(ref _liveTile, value); }
		}
		private bool _liveTile;

		/// <summary>
		/// Gets or sets whether toast notifications should be enabled.
		/// </summary>
		public bool ToastsEnabled
		{
			get { return _toastNotifications; }
			set { TryChangeValue(ref _toastNotifications, value); }
		}
		private bool _toastNotifications;

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
		/// Gets or sets whether instant notifications should be enabled.
		/// </summary>
		public bool InstantNotifications
		{
			get { return _instantNotifications; }
			set { TryChangeValue(ref _instantNotifications, value); }
		}
		private bool _instantNotifications;

		/// <summary>
		/// Gets or sets whether snaps are visible as long a finger is placed on the screen.
		/// </summary>
		public bool HoldToViewSnap
		{
			get { return _holdToViewSnap; }
			set { TryChangeValue(ref _holdToViewSnap, value); }
		}
		private bool _holdToViewSnap;

		/// <summary>
		/// Gets or sets whether the front camera should be flipped horizontally.
		/// </summary>
		public bool FrontCameraMirrorEffect
		{
			get { return _frontCameraMirrorEffect; }
			set { TryChangeValue(ref _frontCameraMirrorEffect, value); }
		}
		private bool _frontCameraMirrorEffect;

		public AutomaticallyDownloadSnapsMode DownloadSnapsMode
		{
			get { return _downloadSnapsMode; }
			set { TryChangeValue(ref _downloadSnapsMode, value); }
		}
		private AutomaticallyDownloadSnapsMode _downloadSnapsMode;

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