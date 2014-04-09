using System;
using System.Diagnostics;
using System.Threading.Tasks;
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
		private MediaCaptureInitializationSettings _mediaCaptureSettings;
		private DeviceInformationCollection _cameraInfoCollection;
		private DeviceInformationCollection _microphoneInfoCollection;

		public MainPage()
		{
			InitializeComponent();

			DataContext = ViewModel = new MainViewModel();
		}
		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			CameraInitialStartupSequencer();

			StatusBar.GetForCurrentView().BackgroundColor = new Color { A = 0x00, R = 0x00, G = 0x00, B = 0x00, };
			ApplicationView.GetForCurrentView().SetDesiredBoundsMode(ApplicationViewBoundsMode.UseCoreWindow);
		}

		protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
		{
			//_mediaCapture.StopPreviewAsync();
		}

		private async void CameraInitialStartupSequencer()
		{
			await GetCameraDevices();

			_mediaCaptureSettings = new MediaCaptureInitializationSettings();

			//generate default settings
			var cameraInfo = _cameraInfoCollection[0]; //default to first device
			var microphoneInfo = _microphoneInfoCollection[0]; //default to first device
			_mediaCaptureSettings.VideoDeviceId = cameraInfo.Id;
			_mediaCaptureSettings.AudioDeviceId = microphoneInfo.Id;
			_mediaCaptureSettings.PhotoCaptureSource = PhotoCaptureSource.VideoPreview;
			_mediaCaptureSettings.StreamingCaptureMode = StreamingCaptureMode.Video;

			await SetUICameraXAMLElements();
			await InitialiseCameraDevice();
			ButtonCamera.IsEnabled = true;
			//ButtonRecord.IsEnabled = true; //not implemented
		}

		private async Task GetCameraDevices()
		{
			_cameraInfoCollection = await DeviceInformation.FindAllAsync(DeviceClass.VideoCapture);
			_microphoneInfoCollection = await DeviceInformation.FindAllAsync(DeviceClass.AudioCapture);
		}
		private async Task InitialiseCameraDevice()
		{
			_mediaCapture = new MediaCapture();

			Debug.WriteLine("Initialising Camera");
			await _mediaCapture.InitializeAsync(_mediaCaptureSettings);
			Debug.WriteLine("Initialising Camera: OK");

			Debug.WriteLine("Starting Camera Preview");
			capturePreview.Source = _mediaCapture;
			await _mediaCapture.StartPreviewAsync();
			Debug.WriteLine("Starting Camera Preview: OK");
		}
		private async Task SetUICameraXAMLElements()
		{
			if (_cameraInfoCollection.Count < 2)
			{
				ButtonFrontFacing.IsEnabled = false;
			}
		}

		private async void CapturePhoto() //Also trigger off shutter key? IDK how in 8.1, no more CameraButtons.ShutterKeyPressed etc.
		{
			var stream = new InMemoryRandomAccessStream();
			var imageProperties = ImageEncodingProperties.CreateJpeg();

			Debug.WriteLine("Capping Photo");
			await _mediaCapture.CapturePhotoToStreamAsync(imageProperties, stream);
			if (stream.Size > 0) { Debug.WriteLine("Capping Photo: OK, stream size " + stream.Size); }
		}

		private void ButtonPhoto_OnClick(object sender, RoutedEventArgs e)
		{
			CapturePhoto();
		}

		private void ButtonRecord_OnClick(object sender, RoutedEventArgs e)
		{
			throw new NotImplementedException();
		}

		private void ButtonFrontFacing_onClick(object sender, RoutedEventArgs e)
		{

			throw new NotImplementedException();
		}

		private void ButtonLayers_onClick(object sender, RoutedEventArgs e)
		{
			throw new NotImplementedException();
		}

		private void ButtonFlash_onClick(object sender, RoutedEventArgs e)
		{
			throw new NotImplementedException();
		}

		private void Button4_onClick(object sender, RoutedEventArgs e)
		{
			throw new NotImplementedException();
			//todo IDK what this is for? Alex?
		}
	}
}
