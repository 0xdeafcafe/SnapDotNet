using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Graphics.Display;
using Windows.Media.Capture;
using Windows.Media.MediaProperties;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Panel = Windows.Devices.Enumeration.Panel;

namespace Snapchat.Helpers
{
	public static class MediaCaptureManager
	{
		private static MediaCapture MediaCapture { get; set; }
		public static CaptureElement PreviewElement { get; set; }

		public static bool IsRecording { get; set; }
		public static bool IsPreviewing { get; set; }
		public static bool IsMirrored
		{
			get
			{
				return AppSettings.Get<bool>("FrontCameraMirrorEffect") && IsUsingFrontCamera;
			}
		}

		public static bool IsUsingFrontCamera
		{
			get { return _cameraInfoCollection[_currentVideoDevice].EnclosureLocation.Panel == Panel.Front; }
		}

		/// <summary>
		/// Gets or sets whether flash is currently enabled.
		/// </summary>
		public static bool IsFlashEnabled
		{
			get { return MediaCapture.VideoDeviceController.FlashControl.Enabled; }
			set { MediaCapture.VideoDeviceController.FlashControl.Enabled = value; }
		}

		/// <summary>
		/// Gets a boolean value indicating whether a front camera is available.
		/// </summary>
		public static bool HasFrontCamera { get; private set; }

		/// <summary>
		/// Gets a boolean value indicating whether flash is supported.
		/// </summary>
		public static bool IsFlashSupported
		{
			get
			{
				return IsInitialized && MediaCapture.VideoDeviceController.FlashControl.Supported;
			}
		}

		public static bool IsPrepared { get; private set; }
		public static bool IsInitialized { get; private set; }

		private static DeviceInformationCollection _cameraInfoCollection, _microphoneInfoCollection;
		private static int _currentVideoDevice, _currentAudioDevice = 0;

		#region Capture Methods

		public static async Task<WriteableBitmap> CapturePhotoAsync()
		{
			var imageEncodingProperties = ImageEncodingProperties.CreateJpeg();
			imageEncodingProperties.Width = 720;
			imageEncodingProperties.Height = 1280;

			var bitmapImage = new BitmapImage();
			WriteableBitmap writableBitmap;
			using (var photoStream = new InMemoryRandomAccessStream())
			{
				await MediaCapture.CapturePhotoToStreamAsync(imageEncodingProperties, photoStream);
				await photoStream.FlushAsync();
				photoStream.Seek(0);
				bitmapImage.SetSource(photoStream);

				writableBitmap = new WriteableBitmap(bitmapImage.PixelWidth, bitmapImage.PixelHeight);
				photoStream.Seek(0);
				await writableBitmap.SetSourceAsync(photoStream);

				// Fix image rotation
				writableBitmap = writableBitmap.Rotate(RotationValueFromVideoRotation(VideoPreviewRotationLookup()));

				// Fix image mirroring (as we only actualy mirror the preview, not the capture element)
				if (IsMirrored) writableBitmap = writableBitmap.Flip(WriteableBitmapExtensions.FlipMode.Horizontal);
			}
			return writableBitmap;
		}

		#endregion

		#region Prep and Disposal Methods

		/// <summary>
		/// Prepares the media capture manager by obtaining device information.
		/// </summary>
		public static async Task PrepareAsync()
		{
			if (IsPrepared)
			{
				Debug.WriteLine("Already prepared media capture manager, skipping unnecessary re-preparation");
				return;
			}

			Debug.WriteLine("Preparing media capture manager");
			_cameraInfoCollection = await DeviceInformation.FindAllAsync(DeviceClass.VideoCapture);
			_microphoneInfoCollection = await DeviceInformation.FindAllAsync(DeviceClass.AudioCapture);

			// Enable front camera if there's more than one camera.
			HasFrontCamera = _cameraInfoCollection.Any(c => c.EnclosureLocation.Panel == Panel.Front);

			IsPrepared = true;
		}

		public static async Task ToggleCameraAsync()
		{
			_currentVideoDevice = _currentVideoDevice == 0 ? 1 : 0;

			await InitializeCameraAsync();
			if (!IsPreviewing && PreviewElement != null)
			{
				await StartPreviewAsync(PreviewElement);
			}
		}

		public static async Task InitializeCameraAsync()
		{
			if (!IsPrepared)
				await PrepareAsync();

			if (IsInitialized)
				await CleanupCaptureResourcesAsync();

			Debug.WriteLine("Initializing camera");

			// Initialize media capture
			MediaCapture = new MediaCapture();
			await MediaCapture.InitializeAsync(new MediaCaptureInitializationSettings
			{
				VideoDeviceId =  _cameraInfoCollection[_currentVideoDevice].Id,
				PhotoCaptureSource = PhotoCaptureSource.VideoPreview,
				//AudioDeviceId = _cameraInfoCollection[_currentAudioDevice].Id,
				StreamingCaptureMode = StreamingCaptureMode.Video
			});
			IsInitialized = true;

			// Correct camera rotation
			MediaCapture.SetPreviewRotation(VideoPreviewRotationLookup());

			Debug.WriteLine("Initialized camera!");
		}

		public static async Task CleanupCaptureResourcesAsync()
		{
			Debug.WriteLine("Cleaning up camera resources");

			if (IsRecording && MediaCapture != null)
				await MediaCapture.StopRecordAsync();
			IsRecording = false;

			if (IsPreviewing && MediaCapture != null)
			{
				await StopPreviewAsync();
			}

			if (MediaCapture != null)
			{
				if (PreviewElement != null)
					PreviewElement.Source = null;
				
				MediaCapture.Dispose();
				MediaCapture = null;
			}

			IsInitialized = false;
		}

		public static async Task StartPreviewAsync(CaptureElement element)
		{
			if (!IsInitialized)
			{
				Debug.WriteLine("Camera not initialized, now initializing camera...");
				await InitializeCameraAsync();
			}

			if (PreviewElement != null)
			{
				Debug.WriteLine("Unbinding media capture from existing UI element");
				PreviewElement.Source = null;
			}

			PreviewElement = element;
			PreviewElement.Source = MediaCapture;

			DisplayInformation.AutoRotationPreferences = DisplayOrientations.Portrait;
			await MediaCapture.StartPreviewAsync();
			IsPreviewing = true;

			Debug.WriteLine("Started camera preview");
		}

		public static async Task StopPreviewAsync()
		{
			if (PreviewElement != null)
				PreviewElement.Source = null;

			try
			{
				await MediaCapture.StopPreviewAsync();
				Debug.WriteLine("Stopped camera preview");
			}
			catch (Exception e) { }
			IsPreviewing = false;
		}

		#endregion

		private static VideoRotation VideoPreviewRotationLookup()
		{
			var previewMirroring = MediaCapture.GetPreviewMirroring() && IsUsingFrontCamera;
			var counterclockwiseRotation = (previewMirroring && !IsUsingFrontCamera) ||
				(!previewMirroring && IsUsingFrontCamera);

			switch (DisplayInformation.GetForCurrentView().CurrentOrientation)
			{
				case DisplayOrientations.Portrait:
					return counterclockwiseRotation ? VideoRotation.Clockwise270Degrees : VideoRotation.Clockwise90Degrees;

				case DisplayOrientations.LandscapeFlipped:
					return VideoRotation.Clockwise180Degrees;

				case DisplayOrientations.PortraitFlipped:
					return counterclockwiseRotation ? VideoRotation.Clockwise90Degrees : VideoRotation.Clockwise270Degrees;

				case DisplayOrientations.Landscape:
				default:
					return VideoRotation.None;
			}
		}

		private static int RotationValueFromVideoRotation(VideoRotation rotation)
		{
			switch (rotation)
			{
				case VideoRotation.Clockwise180Degrees:
					return 180;
				case VideoRotation.Clockwise270Degrees:
					return 270;
				case VideoRotation.Clockwise90Degrees:
					return 90;

				case VideoRotation.None:
				default:
					return 0;
			}
		}
	}
}
