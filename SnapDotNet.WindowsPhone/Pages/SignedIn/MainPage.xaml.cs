using System;
using System.Diagnostics;
using Windows.Devices.Enumeration;
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
			CameraPreview.Source = _mediaCapture;
			StartCameraDevice();

		}

		protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
		{
			_mediaCapture.StopPreviewAsync();
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
			await _mediaCapture.StartPreviewAsync();
			Debug.WriteLine("Starting Camera Preview: OK");
		}
	}
}
