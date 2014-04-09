using System;
using System.Diagnostics;
using Windows.Devices.Enumeration;
using Windows.Media.MediaProperties;
using Windows.Storage.Streams;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;
using Windows.Media.Capture;
using SnapDotNet.Apps.Attributes;
using SnapDotNet.Apps.ViewModels;

namespace SnapDotNet.Apps.Pages.SignedIn
{
	[RequiresAuthentication]
	public sealed partial class MainPage
	{
		public MainViewModel ViewModel { get; private set; }

		private MediaCapture _mediaCapture;
		private DeviceInformationCollection _cameraInfoCollection;
		private DeviceInformationCollection _microphoneInfoCollection;

		public MainPage()
		{
			InitializeComponent();

			DataContext = ViewModel = new MainViewModel();
		}
		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			StartCameraDevice();

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
			settings.PhotoCaptureSource = PhotoCaptureSource.VideoPreview;
			settings.StreamingCaptureMode = StreamingCaptureMode.Video;

			Debug.WriteLine("Initialising Camera");
			await _mediaCapture.InitializeAsync(settings);
			Debug.WriteLine("Initialising Camera: OK");

			Debug.WriteLine("Starting Camera Preview");
			capturePreview.Source = _mediaCapture;
			await _mediaCapture.StartPreviewAsync();
			Debug.WriteLine("Starting Camera Preview: OK");
		}

		private void ButtonPhoto_OnClick(object sender, RoutedEventArgs e)
		{
			CapturePhoto();
		}
		private async void CapturePhoto() //Also trigger off shutter key? IDK how in 8.1, no more CameraButtons.ShutterKeyPressed etc.
		{
			var stream = new InMemoryRandomAccessStream();
			var imageProperties = ImageEncodingProperties.CreateJpeg();

			Debug.WriteLine("Capping Photo");
			await _mediaCapture.CapturePhotoToStreamAsync(imageProperties, stream);
			if (stream.Size > 0) { Debug.WriteLine("Capping Photo: OK, stream size " + stream.Size); }
		}
	}
}
