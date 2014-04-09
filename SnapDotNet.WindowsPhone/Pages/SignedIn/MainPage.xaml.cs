using System;
using System.Diagnostics;
using Windows.Devices.Enumeration;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml.Navigation;
using Windows.Media.Capture;

namespace SnapDotNet.Apps.Pages.SignedIn
{
	public sealed partial class MainPage
	{
		private MediaCapture _mediaCapture;
		private DeviceInformationCollection _cameraInfoCollection;
		private DeviceInformationCollection _microphoneInfoCollection;
		public MainPage()
		{
			InitializeComponent();
		}
		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			//StartCameraDevice();

			StatusBar.GetForCurrentView().BackgroundColor = new Color { A = 0x00, R = 0x00, G = 0x00, B = 0x00, };
			ApplicationView.GetForCurrentView().SetDesiredBoundsMode(ApplicationViewBoundsMode.UseCoreWindow);
		}

		protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
		{
			//_mediaCapture.StopPreviewAsync();
		}
		private async void StartCameraDevice()
		{
			_mediaCapture = new MediaCapture();
			_cameraInfoCollection = await DeviceInformation.FindAllAsync(DeviceClass.VideoCapture);
			_microphoneInfoCollection = await DeviceInformation.FindAllAsync(DeviceClass.AudioCapture);

			var settings = new MediaCaptureInitializationSettings();
			var cameraInfo = _cameraInfoCollection[0];
			var microphoneInfo = _microphoneInfoCollection[0];

			settings.VideoDeviceId = cameraInfo.Id;
			settings.AudioDeviceId = microphoneInfo.Id;

			Debug.WriteLine("Initialising Camera");
			await _mediaCapture.InitializeAsync(settings);
			Debug.WriteLine("Initialising Camera: OK");

			Debug.WriteLine("Starting Camera Preview");
			//CameraPreview.Source = _mediaCapture;
			await _mediaCapture.StartPreviewAsync();
			Debug.WriteLine("Starting Camera Preview: OK");
		}
	}
}
