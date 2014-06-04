using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Graphics.Display;
using Windows.Media.Capture;
using Windows.UI.Xaml.Controls;

namespace Snapchat.Helpers
{
	public static class MediaCaptureManager
	{
		private static MediaCapture MediaCapture { get; set; }
		public static CaptureElement PreviewElement { get; set; }

		public static bool IsRecording { get; set; }
		public static bool IsPreviewing { get; set; }
		public static bool IsMirrored { get; set; }

		public static bool IsUsingFrontCamera
		{
			get { return _currentVideoDevice == 0; }
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
				if (!IsInitialized)
					return false;

				return MediaCapture.VideoDeviceController.FlashControl.Supported;
			}
		}

		public static bool IsPrepared { get { return _isPrepared; } }
		public static bool IsInitialized { get { return _isInitialized; } }

		private static DeviceInformationCollection _cameraInfoCollection, _microphoneInfoCollection;
		private static int _currentAudioDevice = 0, _currentVideoDevice;
		private static bool _isPrepared, _isInitialized;

		/// <summary>
		/// Prepares the media capture manager by obtaining device information.
		/// </summary>
		public static async Task PrepareAsync()
		{
			if (_isPrepared)
			{
				Debug.WriteLine("Already prepared media capture manager, skipping unnecessary re-preparation");
				return;
			}

			Debug.WriteLine("Preparing media capture manager");
			_cameraInfoCollection = await DeviceInformation.FindAllAsync(DeviceClass.VideoCapture);
			_microphoneInfoCollection = await DeviceInformation.FindAllAsync(DeviceClass.AudioCapture);

			// Enable front camera if there's more than one camera.
			HasFrontCamera = _cameraInfoCollection.Count >= 2;

			_isPrepared = true;
		}

		public static async Task ToggleCameraAsync()
		{
			_currentVideoDevice = _currentVideoDevice == 0 ? 1 : 0;

			await InitializeCameraAsync();
			if (IsPreviewing && PreviewElement != null)
				await StartPreviewAsync(PreviewElement);
		}

		public static async Task InitializeCameraAsync()
		{
			if (!_isPrepared)
				await PrepareAsync();

			if (_isInitialized)
				await CleanupCaptureResourcesAsync();

			Debug.WriteLine("Initializing camera");

			// Initialize media capture
			MediaCapture = new MediaCapture();
			await MediaCapture.InitializeAsync(new MediaCaptureInitializationSettings
			{
				VideoDeviceId =  _cameraInfoCollection[_currentVideoDevice].Id,
				PhotoCaptureSource = PhotoCaptureSource.VideoPreview,
				StreamingCaptureMode = StreamingCaptureMode.Video
			});
			_isInitialized = true;

			// Correct camera rotation
			MediaCapture.SetPreviewRotation(IsUsingFrontCamera
				? VideoRotation.Clockwise270Degrees
				: VideoRotation.Clockwise90Degrees);

			if (IsMirrored && IsUsingFrontCamera)
				MediaCapture.SetPreviewMirroring(true);

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

			_isInitialized = false;
		}

		public static async Task StartPreviewAsync(CaptureElement element)
		{
			if (!_isInitialized)
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
	}
}
