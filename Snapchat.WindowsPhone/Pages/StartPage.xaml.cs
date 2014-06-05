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
			var initializeCameraTask = MediaCaptureManager.InitializeCameraAsync();

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
			// Exit the app if it's at the start screen.
			if (VisualStateManager.GetVisualStateGroups(PageContainer).First().CurrentState.Name == "Start")
				return;

			// Otherwise, go back to the start screen.
			VisualStateManager.GoToState(VisualStateUtilities.FindNearestStatefulControl(PageContainer), "Start", false);
			e.Handled = true;
			
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
						StartPageGridPortrait.Visibility = Visibility.Visible;
						StartPageGridLandscape.Visibility = Visibility.Collapsed;
					}
					break;

				case DisplayOrientations.Landscape:
				case DisplayOrientations.LandscapeFlipped:
				default:
					if (StartPageGridPortrait.Visibility == Visibility.Visible)
					{
						StartPageGridLandscape.Visibility = Visibility.Visible;
						StartPageGridPortrait.Visibility = Visibility.Collapsed;
					}
					break;
			}
		}

		private void LogInPageEmailUsernameTextBox_KeyDown(object sender, KeyRoutedEventArgs e)
	    {
			if (e.Key == Windows.System.VirtualKey.Enter)
				LogInPagePasswordBox.Focus(FocusState.Keyboard);
	    }

	    private void LogInPagePasswordBox_KeyDown(object sender, KeyRoutedEventArgs e)
	    {
		    if (e.Key == Windows.System.VirtualKey.Enter)
		    {
				LogInPageLoginButton.Focus(FocusState.Programmatic);
				LogInPageLoginButton.Command.Execute(null);
		    }
	    }

		private void SignUpPageEmailTextBox_KeyDown(object sender, KeyRoutedEventArgs e)
		{
			if (e.Key == Windows.System.VirtualKey.Enter)
				SignUpPagePasswordBox.Focus(FocusState.Keyboard);
		}

		private void SignUpPagePasswordBox_KeyDown(object sender, KeyRoutedEventArgs e)
		{
			if (e.Key == Windows.System.VirtualKey.Enter)
				SignUpPageBirthdayDatePicker.Focus(FocusState.Programmatic);
		}
    }
}
