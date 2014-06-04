using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Media.MediaProperties;
using Windows.Phone.UI.Input;
using Windows.Storage.Streams;
using Windows.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Windows.Media.Capture;
using SnapDotNet.Apps.Attributes;
using SnapDotNet.Apps.ViewModels.SignedIn;

namespace SnapDotNet.Apps.Pages.SignedIn
{
	[RequiresAuthentication]
	public sealed partial class MainPage
	{
		public MainViewModel ViewModel { get; private set; }

		private readonly DispatcherTimer _videoRecordTimer = new DispatcherTimer();
		private readonly Stopwatch _videoRecordStopwatch = new Stopwatch();
		private CameraWrapped _cam;

		public MainPage()
		{
			Debug.WriteLine("**MainPage Constructor Triggered**");
			InitializeComponent();

			HardwareButtons.CameraPressed += HardwareButtons_CameraPressed;
			DataContext = ViewModel = new MainViewModel();

			this.Loaded += delegate { App.UpdateSnapchatData(); };

			_cam = new CameraWrapped();
		}

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			Debug.WriteLine("**MainPage OnNavigateTo Triggered**");
			//Check backstack
			if (e.Parameter is String && ((string)e.Parameter) == "removeBackStack")
				while (App.CurrentFrame.CanGoBack)
					App.CurrentFrame.BackStack.RemoveAt(0);

			_videoRecordTimer.Interval = new TimeSpan(0, 0, 0, 1);
			_videoRecordTimer.Tick += videoRecordTimer_Tick;

			camOnNav();
		}

		private async Task camOnNav()
		{
			await _cam.Initialise();
			CapturePreview.Source = null;
			CapturePreview.Source = _cam.mediaCapture;
			await _cam.TryStartPreviewAsync();
			ButtonFrontFacing.IsEnabled = _cam.isFrontFacingSupported;
			FlashButton.IsEnabled = _cam.isFlashSupported;
			ButtonCamera.IsEnabled = true;
		}
		protected override async void OnNavigatingFrom(NavigatingCancelEventArgs e)
		{
			CapturePreview.Source = null;
			_cam.TryStopPreviewAsync();
		}
		void videoRecordTimer_Tick(object sender, object e)
		{
			VideoTimerBlock.Text = String.Format("{0}s", _videoRecordStopwatch.Elapsed.Seconds);
		}

		private void ButtonPhoto_OnClick(object sender, RoutedEventArgs e)
		{
			CapturePhoto();
		}

		void HardwareButtons_CameraPressed(object sender, CameraEventArgs e)
		{
			CapturePhoto();
		}

		private async void CapturePhoto()
		{
			var v = new { Stream = await _cam.CapturePhoto(),  IsPhoto = true };
			App.CurrentFrame.Navigate(typeof(PreviewPage), v);
		}
		private async void ButtonRecord_OnHolding(object sender, Windows.UI.Xaml.Input.HoldingRoutedEventArgs e) // TODO: broken, final video stream is of size 0....
		{
			if (e.HoldingState == HoldingState.Started)
			{
				RecordingIcon.Visibility = Visibility.Visible;
				CameraIcon.Visibility = Visibility.Collapsed;
				VideoTimerBlock.Visibility = Visibility.Visible;
				_videoRecordStopwatch.Reset();
				_videoRecordStopwatch.Start();
				_videoRecordTimer.Start();

				await _cam.BeginVideoRecord();
			}
			else
			{
				RecordingIcon.Visibility = Visibility.Collapsed;
				CameraIcon.Visibility = Visibility.Visible;
				_videoRecordTimer.Stop();
				_videoRecordStopwatch.Stop();
				Debug.WriteLine("Stopping Video");
				var _localStream = (await _cam.EndVideoRecord()).CloneStream();
				Debug.WriteLine("Stopping Video: OK, stream size " + _localStream.Size);
				VideoTimerBlock.Visibility = Visibility.Collapsed;

				var v = new { Stream = _localStream, IsPhoto = false };
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

			_cam.ChangeActiveCamera();

			var toggleButton = sender as ToggleButton;
			if (toggleButton == null) return;
			if (toggleButton.IsChecked == null) return;

			var imagePath = (bool)toggleButton.IsChecked
				? new Uri("ms-appx:///Assets/Icons/appbar.camera.flip.off.png")
				: new Uri("ms-appx:///Assets/Icons/appbar.camera.flip.png");
			FrontFacingImage.Source = new BitmapImage(imagePath);

			ButtonFrontFacing.IsEnabled = true;
		}

		private void FlashButton_CheckChanged(object sender, RoutedEventArgs e)
		{
			FlashButton.IsEnabled = false;
			var toggleButton = sender as ToggleButton;
			if (toggleButton == null) return;
			if (toggleButton.IsChecked == null) return;


			if (!(bool)toggleButton.IsChecked)
			{
				_cam.EnableFlash();
			}
			else
			{
				_cam.DisableFlash();
			}

			var imagePath = (bool)toggleButton.IsChecked
				? new Uri("ms-appx:///Assets/Icons/appbar.camera.flash.off.png")
				: new Uri("ms-appx:///Assets/Icons/appbar.camera.flash.png");
			FlashImage.Source = new BitmapImage(imagePath);

			Debug.WriteLine("Flash " + toggleButton.IsChecked);
			FlashButton.IsEnabled = true;
		}
	}

	public class CameraWrapped
	{
		private bool _areWeInitialising;

		public MediaCapture mediaCapture; //pretend this is private, only use when necessary (eg, binding for video stream)
		private MediaCaptureInitializationSettings _mediaCaptureSettings;
		private DeviceInformationCollection _cameraInfoCollection;
		private int _currentSelectedCameraDevice;
		private DeviceInformationCollection _microphoneInfoCollection;
		private int _currentSelectedAudioDevice;

		public bool isFlashSupported;
		public bool isFrontFacingSupported;

		public void EnableFlash()
		{
			if (isFlashSupported)
			{
				mediaCapture.VideoDeviceController.FlashControl.Enabled = true; //auto
			}
		}

		public void DisableFlash()
		{
			if (isFlashSupported)
			{
				mediaCapture.VideoDeviceController.FlashControl.Enabled = false;
			}
		}
		private async Task GenerateDefaults()
		{
			await GetCameraDevices();

			_mediaCaptureSettings = new MediaCaptureInitializationSettings();

			//generate default settings
			_currentSelectedAudioDevice = 0;
			_currentSelectedCameraDevice = _cameraInfoCollection.Count > 1 ? 1 : 0;
			
			var cameraInfo = _cameraInfoCollection[_currentSelectedCameraDevice]; //default to first device
			//var microphoneInfo = _microphoneInfoCollection[_currentSelectedAudioDevice]; //default to first device

			_mediaCaptureSettings.VideoDeviceId = cameraInfo.Id;
			//_mediaCaptureSettings.AudioDeviceId = microphoneInfo.Id;
			//_mediaCaptureSettings.PhotoCaptureSource = PhotoCaptureSource.VideoPreview;
			//_mediaCaptureSettings.StreamingCaptureMode = StreamingCaptureMode.Video
		}
		private async Task GetCameraDevices()
		{
			_cameraInfoCollection = await DeviceInformation.FindAllAsync(DeviceClass.VideoCapture);
			_microphoneInfoCollection = await DeviceInformation.FindAllAsync(DeviceClass.AudioCapture);

			isFrontFacingSupported = _cameraInfoCollection.Count > 1;
		}
		public async Task Initialise()
		{
			if (!_areWeInitialising)
			{
				_areWeInitialising = true;
				if (_mediaCaptureSettings == null) { await GenerateDefaults(); }
				if (_mediaCaptureSettings != null)
				{

					if (mediaCapture != null)
					{
						mediaCapture.Dispose();
					}
					mediaCapture = new MediaCapture();
					await mediaCapture.InitializeAsync(_mediaCaptureSettings);
					isFlashSupported = mediaCapture.VideoDeviceController.FlashControl.Supported;
					mediaCapture.SetPreviewRotation(VideoRotation.Clockwise90Degrees);
					EnableFlash();
				}
			_areWeInitialising = false;
			}
		}
		public async void ChangeActiveCamera()
		{
			_currentSelectedCameraDevice += 1;
			if (_currentSelectedCameraDevice > (_cameraInfoCollection.Count - 1))
			{
				_currentSelectedCameraDevice = 0;
			}
			_mediaCaptureSettings.VideoDeviceId = _cameraInfoCollection[_currentSelectedCameraDevice].Id;

			await TryStopPreviewAsync();
			await Initialise();

			if (_currentSelectedCameraDevice == 1)
			{
				mediaCapture.SetPreviewRotation(VideoRotation.Clockwise90Degrees);
			}
			else
			{
				mediaCapture.SetPreviewRotation(VideoRotation.Clockwise270Degrees);
			}
			await TryStartPreviewAsync();
		}
		public async Task<InMemoryRandomAccessStream> CapturePhoto()
		{
			var stream = new InMemoryRandomAccessStream();
			var imageProperties = ImageEncodingProperties.CreateJpeg();

			Debug.WriteLine("Capping Photo");
			await mediaCapture.CapturePhotoToStreamAsync(imageProperties, stream);
			if (stream.Size > 0)
			{
				Debug.WriteLine("Capping Photo: OK, stream size " + stream.Size);
			}
			return stream;
		}

		private InMemoryRandomAccessStream _videoTemporaryRecordingStream;
		public async Task BeginVideoRecord()
		{
			_videoTemporaryRecordingStream = new InMemoryRandomAccessStream();
			var videoProperties = MediaEncodingProfile.CreateMp4(VideoEncodingQuality.Auto); // TODO: do proper settings, mp4 will do for now
			Debug.WriteLine("Capping Video");
			await mediaCapture.StartRecordToStreamAsync(videoProperties, _videoTemporaryRecordingStream);

		}
		public async Task<InMemoryRandomAccessStream> EndVideoRecord()
		{
			await mediaCapture.StopRecordAsync();
			return _videoTemporaryRecordingStream;
		}
		public async Task TryStartPreviewAsync()
		{
			if (mediaCapture != null)
			{
				try
				{
					await mediaCapture.StartPreviewAsync();

				}
				catch (Exception ex)
				{
					Debug.WriteLine(ex.Message + ex.StackTrace);
				}
				Debug.WriteLine("Started Preview");
			}
			else
			{
				Debug.WriteLine("Camera not initialised yet");
			}
		}
		public async Task TryStopPreviewAsync()
		{
			if (mediaCapture != null)
			{
				await mediaCapture.StopPreviewAsync();
				Debug.WriteLine("Stopped Preview");
			}
		}
	}
}
