using ColdSnap.Common;
using ColdSnap.Pages;
using SnapDotNet;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace ColdSnap.ViewModels
{
	public sealed class SettingsPageViewModel
		: BaseViewModel
	{
		public SettingsPageViewModel()
		{
			UpgradeProCommand = new RelayCommand(UpgradeToPro);
			LogoutCommand = new RelayCommand(Logout);
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
				OnPropertyChanged("TileTransparencyUiExampleBrush");
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
			get { return (DateTime.Now - Account.LastReplayed).ToString(); }
		}

		private void UpgradeToPro()
		{

		}

		private void Logout()
		{
			Window.Current.Navigate(typeof(StartPage), Account);
		}
    }
}
