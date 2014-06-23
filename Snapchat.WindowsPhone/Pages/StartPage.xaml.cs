using Microsoft.Xaml.Interactivity;
using System;
using System.Linq;
using Windows.Phone.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
using Snapchat.Helpers;
using Windows.Graphics.Display;
using Windows.Devices.Sensors;
using Windows.UI.Core;

namespace Snapchat.Pages
{
    public sealed partial class StartPage : Page
	{
		private Accelerometer _accelerometer;

		public StartPage()
		{
			NavigationCacheMode = NavigationCacheMode.Required;

            InitializeComponent();

			// Detect orientation changes
			_accelerometer = Accelerometer.GetDefault();
        }

	    protected override async void OnNavigatedTo(NavigationEventArgs e)
	    {
			HardwareButtons.BackPressed += HardwareButtons_BackPressed;

		    var logoutTask = App.SnapchatManager.Logout();
			var preloadCameraTask = App.Camera.PreloadAsync();

			// Detect orientation changed
			if (_accelerometer != null)
				_accelerometer.ReadingChanged += CalculateDeviceRotation;
	    }

	    protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
	    {
		    HardwareButtons.BackPressed -= HardwareButtons_BackPressed;
	    }

	    private void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
		{
			switch (DisplayInformation.GetForCurrentView().CurrentOrientation)
			{
				case DisplayOrientations.Portrait:
				case DisplayOrientations.PortraitFlipped:
					// Exit the app if it's at the start screen.
					if (VisualStateManager.GetVisualStateGroups(PageContainer).First().CurrentState.Name == "StartPortrait")
						return;

					// Otherwise, go back to the start screen.
					VisualStateManager.GoToState(VisualStateUtilities.FindNearestStatefulControl(PageContainer), "StartPortrait", false);
					e.Handled = true;
					break;

				case DisplayOrientations.Landscape:
				case DisplayOrientations.LandscapeFlipped:
					// Exit the app if it's at the start screen.
					if (VisualStateManager.GetVisualStateGroups(PageContainer).First().CurrentState.Name == "StartLandscape")
						return;

					// Otherwise, go back to the start screen.
					VisualStateManager.GoToState(VisualStateUtilities.FindNearestStatefulControl(PageContainer), "StartLandscape", false);
					e.Handled = true;
					break;
			}

			if (_accelerometer != null)
				_accelerometer.ReadingChanged -= CalculateDeviceRotation;
		}

		private async void CalculateDeviceRotation(Accelerometer sender, AccelerometerReadingChangedEventArgs args)
		{
			await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => { UpdateOrientation(); });
		}

		private void UpdateOrientation()
		{
			switch (DisplayInformation.GetForCurrentView().CurrentOrientation)
			{
				case DisplayOrientations.Portrait:
				case DisplayOrientations.PortraitFlipped:
					if (StartPageGridLandscape.Visibility == Visibility.Visible)
					{
						StartPagePortraitGrid.Visibility = Visibility.Visible;
						StartPageGridLandscape.Visibility = Visibility.Collapsed;
					}
					if (LogInPageLandscapeGrid.Visibility == Visibility.Visible)
					{
						LogInPagePortraitGrid.Visibility = Visibility.Visible;
						LogInPageLandscapeGrid.Visibility = Visibility.Collapsed;
					}
					if (SignUpPageLandscapeGrid.Visibility == Visibility.Visible)
					{
						SignUpPagePortraitGrid.Visibility = Visibility.Visible;
						SignUpPageLandscapeGrid.Visibility = Visibility.Collapsed;
					}
					break;

				case DisplayOrientations.Landscape:
				case DisplayOrientations.LandscapeFlipped:
				default:
					if (StartPagePortraitGrid.Visibility == Visibility.Visible)
					{
						StartPageGridLandscape.Visibility = Visibility.Visible;
						StartPagePortraitGrid.Visibility = Visibility.Collapsed;
					}
					if (LogInPagePortraitGrid.Visibility == Visibility.Visible)
					{
						LogInPageLandscapeGrid.Visibility = Visibility.Visible;
						LogInPagePortraitGrid.Visibility = Visibility.Collapsed;
					}
					if (SignUpPagePortraitGrid.Visibility == Visibility.Visible)
					{
						SignUpPageLandscapeGrid.Visibility = Visibility.Visible;
						SignUpPagePortraitGrid.Visibility = Visibility.Collapsed;
					}
					break;
			}
		}

		private void LogInPageEmailUsernamePortraitTextBox_KeyDown(object sender, KeyRoutedEventArgs e)
	    {
			if (e.Key == Windows.System.VirtualKey.Enter)
				LogInPagePortraitPasswordBox.Focus(FocusState.Keyboard);
	    }

		private void LogInPageEmailUsernameLandscapeTextBox_KeyDown(object sender, KeyRoutedEventArgs e)
		{
			if (e.Key == Windows.System.VirtualKey.Enter)
				LogInPageLandscapePasswordBox.Focus(FocusState.Keyboard);
		}

		private void LogInPagePortraitPasswordBox_KeyDown(object sender, KeyRoutedEventArgs e)
		{
			if (e.Key == Windows.System.VirtualKey.Enter)
			{
				LogInPageLoginPortraitButton.Focus(FocusState.Programmatic);
				LogInPageLoginPortraitButton.Command.Execute(null);
			}
		}

		private void LogInPageLandscapePasswordBox_KeyDown(object sender, KeyRoutedEventArgs e)
		{
			if (e.Key == Windows.System.VirtualKey.Enter)
			{
				LogInPageLoginLandscapeButton.Focus(FocusState.Programmatic);
				LogInPageLoginLandscapeButton.Command.Execute(null);
			}
		}

		private void SignUpPageEmailPortraitTextBox_KeyDown(object sender, KeyRoutedEventArgs e)
		{
			if (e.Key == Windows.System.VirtualKey.Enter)
				SignUpPagePortraitPasswordBox.Focus(FocusState.Keyboard);
		}

		private void SignUpPageEmailLandscapeTextBox_KeyDown(object sender, KeyRoutedEventArgs e)
		{
			if (e.Key == Windows.System.VirtualKey.Enter)
				SignUpPageLandscapePasswordBox.Focus(FocusState.Keyboard);
		}

		private void SignUpPagePortraitPasswordBox_KeyDown(object sender, KeyRoutedEventArgs e)
		{
			if (e.Key == Windows.System.VirtualKey.Enter)
				SignUpPageBirthdayPortraitDatePicker.Focus(FocusState.Programmatic);
		}

		private void SignUpPageLandscapePasswordBox_KeyDown(object sender, KeyRoutedEventArgs e)
		{
			if (e.Key == Windows.System.VirtualKey.Enter)
				SignUpPageBirthdayLandscapeDatePicker.Focus(FocusState.Programmatic);
		}
    }
}
