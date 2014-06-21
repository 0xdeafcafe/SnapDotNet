using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.Media.Capture;
using Windows.Media.MediaProperties;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Panel = Windows.Devices.Enumeration.Panel;

namespace Snapchat.Helpers
{
	public static class MediaCaptureManager
	{
		private static MediaCapture MediaCapture { get; set; }
		public static CaptureElement PreviewElement { get; set; }

		public static LowLagPhotoCapture LowLagPhotoCapture { get; set; }

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

		public static uint PhotoCaptureHeight { get; private set; }
		public static uint PhotoCaptureWidth { get; private set; }

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
			var capture = await LowLagPhotoCapture.CaptureAsync();
			var writableBitmap = new WriteableBitmap((int) PhotoCaptureWidth, (int) PhotoCaptureHeight);
			await writableBitmap.SetSourceAsync(capture.Frame);

			// Fix image mirroring (as we only actualy mirror the preview, not the capture element)
			if (IsMirrored) writableBitmap = writableBitmap.Flip(WriteableBitmapExtensions.FlipMode.Horizontal);

			// Fix rotation
			writableBitmap = writableBitmap.RotateFree(RotationValueFromVideoRotation(VideoPreviewRotationLookup()), false);
			
			// Get that sexy image back to the user!
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

			// Set Resolutions
			await SetResolution(MediaStreamType.Photo);
			await SetResolution(MediaStreamType.VideoPreview);
			await SetResolution(MediaStreamType.VideoRecord);

			// Correct camera rotation
			MediaCapture.SetPreviewRotation(VideoPreviewRotationLookup());
			MediaCapture.SetRecordRotation(VideoPreviewRotationLookup());

			//// TODO: Fix Auto Focus
			//MediaCapture.VideoDeviceController.FocusControl.Configure(new FocusSettings { AutoFocusRange = AutoFocusRange.Normal, Mode = FocusMode.Continuous });

			// Setup low-lag Photo Capture
			MediaCapture.VideoDeviceController.LowLagPhoto.ThumbnailEnabled = false;
			var imageEncoding = ImageEncodingProperties.CreateJpeg();
			imageEncoding.Width = PhotoCaptureWidth;
			imageEncoding.Height = PhotoCaptureHeight;
			LowLagPhotoCapture = await MediaCapture.PrepareLowLagPhotoCaptureAsync(imageEncoding);

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

				if (LowLagPhotoCapture != null)
					await LowLagPhotoCapture.FinishAsync();

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

		#region Helpers

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
		
		#endregion

		private static async Task SetResolution(MediaStreamType mediaStreamType)
		{
			// TODO: ::NOTE::
			// I believe the best SubType GUID's are YUY2, UYVY and NV12 - they have the 
			// best sampleing and bits per channel. I also believe they are supported 
			// by every device. But that DOES need claraification from !testing! (or we fucked yo)
			// TODO: ::NOTE::

			var res = MediaCapture.VideoDeviceController.GetAvailableMediaStreamProperties(mediaStreamType);
			var resolutions = new List<Tuple<uint, uint, int>>();
			var perfectResolution = new Tuple<uint, uint, int>(0, 0, 0);

			if (res.Count <= 0) return;

			for (var i = 0; i < res.Count; i++)
			{
				var vp = (VideoEncodingProperties)res[i];
				var frameRate = (vp.FrameRate.Numerator / vp.FrameRate.Denominator);

				if (vp.Subtype.Equals("YUY2") || vp.Subtype.Equals("NV12") || vp.Subtype.Equals("UYVY"))
					resolutions.Add(new Tuple<uint, uint, int>(vp.Width, vp.Height, i));

				Debug.WriteLine("{0}) {1}, {2}x{3}, Frame/s: {4}", i, vp.Subtype, vp.Width, vp.Height, frameRate);

				if (vp.Width > perfectResolution.Item1 && (vp.Subtype.Equals("YUY2") || vp.Subtype.Equals("NV12") || vp.Subtype.Equals("UYVY")))
					perfectResolution = new Tuple<uint, uint, int>(vp.Width, vp.Height, i);
			}

			var distanceInfo = new Tuple<int, int>(-1, -1);
			if (perfectResolution.Item1 > 640)
			{
				// Get res closest to 480p (640x480)
				var index = 0;
				foreach (var resolution in resolutions)
				{
					var distance = 640 - (int)resolution.Item1;
					if (distance < 0) distance = distance * (-1);

					if (distanceInfo.Item1 == -1 ||
					    distance < distanceInfo.Item1)
					{
						distanceInfo = new Tuple<int, int>(distance, index);
					}

					index++;
				}
				var pefectIndex = distanceInfo.Item2;
				perfectResolution = new Tuple<uint, uint, int>(
					resolutions[pefectIndex].Item1, 
					resolutions[pefectIndex].Item2,
					resolutions[pefectIndex].Item3);
			}

			switch (mediaStreamType)
			{
				case MediaStreamType.Photo:
					PhotoCaptureWidth = perfectResolution.Item1;
					PhotoCaptureHeight = perfectResolution.Item2;
					break;
			}

			Debug.WriteLine("Set {0} Resolution - {1}x{2}", mediaStreamType, perfectResolution.Item1, perfectResolution.Item2);

			// setting resolution
			await MediaCapture.VideoDeviceController.SetMediaStreamPropertiesAsync(mediaStreamType, res[perfectResolution.Item3]);
		}
	}
}
