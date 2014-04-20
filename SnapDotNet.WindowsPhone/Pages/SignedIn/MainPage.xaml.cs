using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Media.MediaProperties;
using Windows.Phone.UI.Input;
using Windows.Storage.Streams;
using Windows.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Windows.Media.Capture;
using SnapDotNet.Apps.Attributes;
using SnapDotNet.Apps.ViewModels.SignedIn;
using SnapDotNet.Core.Miscellaneous.Helpers;

namespace SnapDotNet.Apps.Pages.SignedIn
{
	[RequiresAuthentication]
	public sealed partial class MainPage
	{
		public MainViewModel ViewModel { get; private set; }

		private MediaCapture _mediaCapture;
		private MediaCaptureInitializationSettings _mediaCaptureSettings;
		private DeviceInformationCollection _cameraInfoCollection;
		private int _currentSelectedCameraDevice;
		private DeviceInformationCollection _microphoneInfoCollection;
		private int _currentSelectedAudioDevice;

		private bool _areWeInitialising;
		private bool _areWePreppingCamera;
		private bool _isCameraPrepped;
		private bool _isCameraInitialised;

		private readonly DispatcherTimer _videoRecordTimer = new DispatcherTimer();
		private readonly Stopwatch _videoRecordStopwatch = new Stopwatch();

		public MainPage()
		{
			Debug.WriteLine("**MainPage Constructor Triggered**");
			InitializeComponent();

			HardwareButtons.CameraPressed += HardwareButtons_CameraPressed;
			DataContext = ViewModel = new MainViewModel();
#if !DEBUG
			Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().IsScreenCaptureEnabled = false;
#endif
		}

		protected async override void OnNavigatedTo(NavigationEventArgs e)
		{
			Debug.WriteLine("**MainPage OnNavigateTo Triggered**");
			//Check backstack
			if (e.Parameter is String && ((string)e.Parameter) == "removeBackStack")
				while (App.CurrentFrame.CanGoBack)
					App.CurrentFrame.BackStack.RemoveAt(0);

			_videoRecordTimer.Interval = new TimeSpan(0, 0, 0, 1);
			_videoRecordTimer.Tick += videoRecordTimer_Tick;

			await CameraStartupPrep();
			SetUiCameraXamlElements();
			await InitialiseCameraDevice();

			CapturePreview.Source = _mediaCapture;
			await TryStartPreviewAsync();
		}

		private async Task TryStartPreviewAsync()
		{
			Debug.WriteLine("======TryStartPreviewAsync======");

			if (_isCameraInitialised && _isCameraPrepped)
			{
				try
				{
					await _mediaCapture.StartPreviewAsync();
					Debug.WriteLine("TryStartPreviewAsyc: OK?");
				}
				catch (Exception ex)
				{
					Debug.WriteLine("TryStartPreviewAsyc: FAILED");
				}
			}
			else
			{
				Debug.WriteLine("TryStartPreviewAsync: NOT READY!");
			}
		}


		void videoRecordTimer_Tick(object sender, object e)
		{
			VideoTimerBlock.Text = String.Format("{0}s", _videoRecordStopwatch.Elapsed.Seconds);
		}

		protected override async void OnNavigatingFrom(NavigatingCancelEventArgs e)
		{
			try
			{
				if (_mediaCapture != null)
					await _mediaCapture.StopPreviewAsync();
			}
			catch (Exception exception)
			{
				SnazzyDebug.WriteLine(exception);
			}
		}

		private async Task CameraStartupPrep()
		{
			if (_isCameraPrepped == false)
			{
				if (_areWePreppingCamera != true)
				{
					_areWePreppingCamera = true;
					Debug.WriteLine("Camera Startup Prep");

					await GetCameraDevices();

					_mediaCaptureSettings = new MediaCaptureInitializationSettings();

					//generate default settings
					_currentSelectedAudioDevice = 0;
					_currentSelectedCameraDevice = 0;
					var cameraInfo = _cameraInfoCollection[_currentSelectedCameraDevice]; //default to first device
					//var microphoneInfo = _microphoneInfoCollection[_currentSelectedAudioDevice]; //default to first device

					_mediaCaptureSettings.VideoDeviceId = cameraInfo.Id;
					//_mediaCaptureSettings.AudioDeviceId = microphoneInfo.Id;
					//_mediaCaptureSettings.PhotoCaptureSource = PhotoCaptureSource.VideoPreview;
					//_mediaCaptureSettings.StreamingCaptureMode = StreamingCaptureMode.Video;

					Debug.WriteLine("Camera Startup Prep: DONE");
					_areWePreppingCamera = false;
					_isCameraPrepped = true;
				}
				else
				{
					Debug.WriteLine("Camera Startup Prep: FAIL; Already Running");
				}
			}
			else
			{
				Debug.WriteLine("Camera Startup Prep: FAIL; Already Prepped");
			}
		}

		private async Task GetCameraDevices()
		{
			_cameraInfoCollection = await DeviceInformation.FindAllAsync(DeviceClass.VideoCapture);
			_microphoneInfoCollection = await DeviceInformation.FindAllAsync(DeviceClass.AudioCapture);
		}

		private async Task InitialiseCameraDevice() //must manually.stopPreviewAsync Before re-initialising. -> now auto
		{
			if (_isCameraPrepped == true)
			{
				if (_areWeInitialising != true)
				{
					Debug.WriteLine("Initialising Camera");
					_areWeInitialising = true;

					ButtonCamera.IsEnabled = false;
					ButtonRecord.IsEnabled = false;

					_mediaCapture = new MediaCapture();

					Debug.WriteLine(">Using VDevice " + _currentSelectedCameraDevice + ", ID: " + _mediaCaptureSettings.VideoDeviceId);
					await _mediaCapture.InitializeAsync(_mediaCaptureSettings);
					ButtonCamera.IsEnabled = true;
					ButtonRecord.IsEnabled = true; //not implemented

					_isCameraInitialised = true;
					_areWeInitialising = false;
					Debug.WriteLine("Initialising Camera: OK");
				}
				else
				{
					Debug.WriteLine("Initialising Camera: FAIL; Already Initialising");
				}
			}
			else
			{
				Debug.WriteLine("Initialising Camera: FAIL; Camera not Prepped yet");
			}
		}

		private void SetUiCameraXamlElements()
		{
			if (_cameraInfoCollection.Count < 2)
			{
				ButtonFrontFacing.IsEnabled = false;
			}
		}

		private async void CapturePhoto()
		{
			var stream = new InMemoryRandomAccessStream();
			var imageProperties = ImageEncodingProperties.CreateJpeg();

			Debug.WriteLine("Capping Photo");
			await _mediaCapture.CapturePhotoToStreamAsync(imageProperties, stream);
			if (stream.Size > 0)
			{
				Debug.WriteLine("Capping Photo: OK, stream size " + stream.Size);
			}
			var v = new { Stream = stream, IsPhoto = true };
			App.CurrentFrame.Navigate(typeof(PreviewPage), v);
		}

		private void ButtonPhoto_OnClick(object sender, RoutedEventArgs e)
		{
			CapturePhoto();
		}
		void HardwareButtons_CameraPressed(object sender, CameraEventArgs e)
		{
			CapturePhoto();
		}
		private async void ButtonRecord_OnHolding(object sender, Windows.UI.Xaml.Input.HoldingRoutedEventArgs e) // TODO: broken, final video stream is of size 0....
		{
			var stream = new InMemoryRandomAccessStream();
			if (e.HoldingState == HoldingState.Started)
			{
				ButtonCamera.IsEnabled = false;
				_videoRecordStopwatch.Reset();
				_videoRecordStopwatch.Start();
				_videoRecordTimer.Start();

				var videoProperties = MediaEncodingProfile.CreateMp4(VideoEncodingQuality.Auto); // TODO: do proper settings, mp4 will do for now

				Debug.WriteLine("Capping Video");
				await _mediaCapture.StartRecordToStreamAsync(videoProperties, stream);
			}
			else
			{
				_videoRecordTimer.Stop();
				_videoRecordStopwatch.Stop();
				Debug.WriteLine("Stopping Video");
				await _mediaCapture.StopRecordAsync();
				Debug.WriteLine("Stopping Video: OK, stream size " + stream.Size);
				ButtonCamera.IsEnabled = true;

				var v = new { Stream = stream, IsPhoto = false };
				App.CurrentFrame.Navigate(typeof(PreviewPage), v);
			}
		}

		private void ButtonFriends_OnClick(object sender, RoutedEventArgs e)
		{
			App.CurrentFrame.Navigate(typeof(FriendsPage));
		}

		private async void ButtonFrontFacing_CheckChanged(object sender, RoutedEventArgs e)
		{
			ButtonFrontFacing.IsEnabled = false;
			//_currentSelectedCameraDevice = _currentSelectedCameraDevice == 0 ? 1 : 0;
			//_mediaCaptureSettings.VideoDeviceId = _cameraInfoCollection[_currentSelectedCameraDevice].Id;

			//await InitialiseCameraDevice();

			//var toggleButton = sender as ToggleButton;
			//if (toggleButton == null) return;
			//if (toggleButton.IsChecked == null) return;

			//var imagePath = (bool)toggleButton.IsChecked
			//	? new Uri("ms-appx:///Assets/Icons/appbar.camera.flip.off.png")
			//	: new Uri("ms-appx:///Assets/Icons/appbar.camera.flip.png");
			//FrontFacingImage.Source = new BitmapImage(imagePath);
			//await TryStartPreviewAsync();
			//ButtonFrontFacing.IsEnabled = true;
		}

		private void FlashButton_CheckChanged(object sender, RoutedEventArgs e)
		{
			FlashButton.IsEnabled = false;
			var toggleButton = sender as ToggleButton;
			if (toggleButton == null) return;
			if (toggleButton.IsChecked == null) return;

			_mediaCapture.VideoDeviceController.FlashControl.Enabled = (bool)!toggleButton.IsChecked;

			var imagePath = (bool)toggleButton.IsChecked
				? new Uri("ms-appx:///Assets/Icons/appbar.camera.flash.off.png")
				: new Uri("ms-appx:///Assets/Icons/appbar.camera.flash.png");
			FlashImage.Source = new BitmapImage(imagePath);

			Debug.WriteLine("FlashControl.Enabled set to: " + _mediaCapture.VideoDeviceController.FlashControl.Enabled);
			FlashButton.IsEnabled = true;
		}
	}
}
