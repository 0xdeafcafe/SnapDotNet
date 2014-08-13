using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Graphics.Display;
using Windows.Media.Capture;
using Windows.Media.MediaProperties;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Panel = Windows.Devices.Enumeration.Panel;

namespace Snapchat.Helpers
{
	public sealed class CameraManager
		: IDisposable
	{
		private DeviceInformationCollection _cameraInfoCollection, _microphoneInfoCollection;
		private bool _isInitialized, _isPrepped, _isPreviewing, _isRecording;
		private MediaCapture _deviceCapture, _screenCapture;
		private CaptureElement _captureElement;
		private int _currentVideoDevice, _currentAudioDevice;

		private LowLagPhotoCapture _lowLagPhotoCapture;
		private bool _isDisposed;

		~CameraManager()
		{
			Dispose();
		}

		/// <summary>
		/// Set true to NOT capture via UI screenshot. Fallback alternative is lowlagcapture
		/// </summary>
		/// TODO: make less mess
		public bool IsNotUsingInstantCapture { get; set; }
		
		/// <summary>
		/// Gets a boolean value indicating whether a front camera is available.
		/// </summary>
		public bool HasFrontCamera { get; private set; }

		/// <summary>
		/// Gets a boolean value indicating whether the front camera is currently active.
		/// </summary>
		public bool IsUsingFrontCamera { get { return _cameraInfoCollection[_currentVideoDevice].EnclosureLocation.Panel == Panel.Front; } }

		/// <summary>
		/// Gets the height in pixels of photos captured via this instance.
		/// </summary>
		public uint PhotoCaptureHeight { get; private set; }

		/// <summary>
		/// Gets the width in pixels of photos captured via this instance.
		/// </summary>
		public uint PhotoCaptureWidth { get; private set; }

		/// <summary>
		/// Gets or sets whether flash is currently enabled.
		/// </summary>
		public bool IsFlashEnabled
		{
			get { return _deviceCapture.VideoDeviceController.FlashControl.Enabled; }
			set { _deviceCapture.VideoDeviceController.FlashControl.Enabled = value; }
		}

		/// <summary>
		/// Gets a boolean value indicating whether flash is supported for the current camera device.
		/// </summary>
		public bool IsFlashSupported
		{
			get { return _isInitialized && _deviceCapture.VideoDeviceController.FlashControl.Supported; }
		}

		/// <summary>
		/// Gets a boolean value indicating whether the front camera should be mirrored.
		/// </summary>
		public bool IsFrontCameraMirrored
		{
			get { return AppSettings.Get<bool>("FrontCameraMirrorEffect") && IsUsingFrontCamera; }
		}

		/// <summary>
		/// Preloads the camera system ahead of time to allow for faster capture element loading.
		/// </summary>
		/// <returns></returns>
		public async Task PreloadAsync()
		{
			Debug.WriteLine("Preloading camera manager");
			if (!_isPrepped) await DiscoverDevicesAsync();
			if (!_isInitialized) await InitializeCameraAsync();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="captureElement"></param>
		public async Task SetPreviewSourceAsync(CaptureElement captureElement)
		{
			if (!_isPrepped) await DiscoverDevicesAsync();
			if (!_isInitialized) await InitializeCameraAsync();

			if (captureElement.Source != null)
			{
				Debug.WriteLine("Unbinding media capture source from capture element");
				captureElement.Source = null;
			}
			captureElement.Source = _deviceCapture;
			_captureElement = captureElement;
			Debug.WriteLine("Set capture element source to device media capture");

			await StartPreviewAsync();
		}

		public async Task FlipCameraAsync()
		{
			var captureElement = _captureElement;

			await StopPreviewAsync();
			Dispose();
			_isDisposed = false;

			_currentVideoDevice = _currentVideoDevice == 0 ? 1 : 0;
			await SetPreviewSourceAsync(captureElement);
		}

		/// <summary>
		/// Starts the camera preview.
		/// </summary>
		public async Task StartPreviewAsync()
		{
			if (!_isPreviewing && _deviceCapture != null && _captureElement != null)
			{
				_captureElement.Source = _deviceCapture;
				await _deviceCapture.StartPreviewAsync();
				_isPreviewing = true;
				Debug.WriteLine("Started camera preview");
			}
		}

		/// <summary>
		/// Stops the camera preview.
		/// </summary>
		public async Task StopPreviewAsync()
		{
			if (_isPreviewing)
			{
				_captureElement.Source = null;
				await _deviceCapture.StopPreviewAsync();
				_isPreviewing = false;
				Debug.WriteLine("Stopped camera preview");
			}
		}

		public async Task<WriteableBitmap> CapturePhotoAsync()
		{
			Debug.WriteLine("Attempting Photo Capture. Using Instant: " + !IsNotUsingInstantCapture);
			var bitmap = new WriteableBitmap((int) PhotoCaptureWidth, (int) PhotoCaptureHeight);

			try
			{
				if (!IsNotUsingInstantCapture)
				{
					// TODO: Hide all FrameworkElements that isn't a parent in the hierarchy of the capture element's visual tree.

					var imageEncoding = ImageEncodingProperties.CreateJpeg();
					imageEncoding.Height = PhotoCaptureHeight;
					imageEncoding.Width = PhotoCaptureWidth;

					var stream = new InMemoryRandomAccessStream();
					await _screenCapture.CapturePhotoToStreamAsync(imageEncoding, stream);
					await bitmap.SetSourceAsync(stream);
				}
				else
				{
					CapturedPhoto capturedPhoto;
					capturedPhoto = await _lowLagPhotoCapture.CaptureAsync();
					Debug.WriteLine("Successfully captured via lowlag");
					await bitmap.SetSourceAsync(capturedPhoto.Frame);

					// Fix image mirroring (as we only actualy mirror the preview, not the capture element)
					if (!IsFrontCameraMirrored)
						bitmap = bitmap.Flip(WriteableBitmapExtensions.FlipMode.Horizontal);

					// Fix rotation
					bitmap = bitmap.RotateFree(GetRotationDegreeFromVideoRotation(GetVideoPreviewRotation()), false);
				}
			}
			catch (Exception e)
			{

			}
			return bitmap;
		}

		/// <summary>
		/// Frees up unmanaged resources used by the video and audio capture device.
		/// </summary>
		public async void Dispose()
		{
			if (_isDisposed)
				return;

			Debug.WriteLine("Cleaning up camera resources");

			if (_captureElement != null)
			{
				try { _captureElement.Source = null; }
				catch { }
				finally { _captureElement = null; }
			}

			if (_deviceCapture != null)
			{
				if (_isRecording)
				{
					await _deviceCapture.StopRecordAsync();
					_isRecording = false;
				}

				if (_isPreviewing)
				{
					await _deviceCapture.StopPreviewAsync();
					_isPreviewing = false;
				}

				try
				{
					_deviceCapture.Dispose();
				}
				catch { }
				_deviceCapture = null;
			}

			if (_lowLagPhotoCapture != null)
			{
				try { await _lowLagPhotoCapture.FinishAsync(); }
				catch { }
				finally { _lowLagPhotoCapture = null; }
			}

			if (_screenCapture != null)
			{
				try { _screenCapture.Dispose(); }
				catch { }
				_screenCapture = null;
			}

			_isInitialized = false;
			_isDisposed = true;
		}

		private static int GetRotationDegreeFromVideoRotation(VideoRotation rotation)
		{
			var degree = 0;
			switch (rotation)
			{
				case VideoRotation.Clockwise180Degrees:
					degree = 180;
					break;

				case VideoRotation.Clockwise270Degrees:
					degree = 270;
					break;

				case VideoRotation.Clockwise90Degrees:
					degree = 90;
					break;
			}

			if (degree == 0)
				return 0;
			return 360 - degree;
		}

		/// <summary>
		/// Obtains all video and audio capture devices on this device.
		/// </summary>
		private async Task DiscoverDevicesAsync()
		{
			await Task.Run(() =>
			{
				if (_isPrepped)
				{
					Debug.WriteLine("Skipping unnecessary device discovery");
					return;
				}

				Debug.WriteLine("Discovering all video and audio capture devices...");

				var findAllCameraTask = Task.Run(async () =>
				{
					_cameraInfoCollection = await DeviceInformation.FindAllAsync(DeviceClass.VideoCapture);
					Debug.WriteLine("Found all camera devices ({0} total)", _cameraInfoCollection.Count);
				});

				var findAllMicrophoneTask = Task.Run(async () =>
				{
					_microphoneInfoCollection = await DeviceInformation.FindAllAsync(DeviceClass.AudioCapture);
					Debug.WriteLine("Found all audio capture devices ({0} total)", _microphoneInfoCollection.Count);
				});

				Task.WaitAll(findAllCameraTask, findAllMicrophoneTask);
				_isPrepped = true;
			});
		}

		/// <summary>
		/// Initializes the currently selected camera.
		/// </summary>
		private async Task InitializeCameraAsync()
		{
			if (_isInitialized)
			{
				Debug.WriteLine("Skipping unnecessary initialization");
				return;
			}

			Debug.WriteLine("Initializing camera media capture...");
			_deviceCapture = new MediaCapture();
			await _deviceCapture.InitializeAsync(new MediaCaptureInitializationSettings
			{
				VideoDeviceId = _cameraInfoCollection[_currentVideoDevice].Id,
				PhotoCaptureSource = PhotoCaptureSource.VideoPreview,
				//AudioDeviceId = _microphoneInfoCollection[_currentAudioDevice].Id
				StreamingCaptureMode = StreamingCaptureMode.Video
			});
			Debug.WriteLine("Initialized camera media capture!");

			/*Debug.WriteLine("Initializing screen media capture...");
			_screenCapture = new MediaCapture();
			try
			{
				await _screenCapture.InitializeAsync(new MediaCaptureInitializationSettings
				{
					VideoSource = ScreenCapture.GetForCurrentView().VideoSource
				});
				!_isNotUsingInstantCapture = true;
				Debug.WriteLine("Initialized screen media capture!");
				Debug.WriteLine("Instant screen capture is enabled");
			}
			catch
			{
				Debug.WriteLine("Failed to initialize screen capture, falling back to slow camera capture instead.");
			}*/

			// Set up resolutions.
			//Task.WaitAll(
			//	Task.Factory.StartNew(() => SetResolutionAsync(MediaStreamType.Photo)),
			//	Task.Factory.StartNew(() => SetResolutionAsync(MediaStreamType.VideoPreview)),
			//	Task.Factory.StartNew(() => SetResolutionAsync(MediaStreamType.VideoRecord))
			//);

			// Correct camera rotation.
			var rotation = GetVideoPreviewRotation();
			_deviceCapture.SetPreviewRotation(rotation);
			_deviceCapture.SetRecordRotation(rotation);

			// TODO: Remove this when SetResolutionAsync works properly
			PhotoCaptureWidth = Convert.ToUInt32(Window.Current.Bounds.Width);
			PhotoCaptureHeight = Convert.ToUInt32(Window.Current.Bounds.Height);

			// TODO: Fix Auto Focus
			//_deviceCapture.VideoDeviceController.FocusControl.Configure(new FocusSettings { AutoFocusRange = AutoFocusRange.Normal, Mode = FocusMode.Continuous });

			// Set up low-lag photo capture
			if (IsNotUsingInstantCapture)
			{
				Debug.WriteLine("Preparing low-lag photo capture");
				var imageEncoding = ImageEncodingProperties.CreateJpeg();
				imageEncoding.Width = PhotoCaptureWidth;
				imageEncoding.Height = PhotoCaptureHeight;
				_lowLagPhotoCapture = await _deviceCapture.PrepareLowLagPhotoCaptureAsync(imageEncoding);
			}

			_isInitialized = true;
			Debug.WriteLine("Initialized camera!");
		}

		/// <summary>
		/// Determines the correct camera preview rotation based on the device's orientation.
		/// </summary>
		/// <returns>The rotation that has been determined to be correct.</returns>
		private VideoRotation GetVideoPreviewRotation()
		{
			var previewMirroring = _deviceCapture.GetPreviewMirroring() && IsUsingFrontCamera;
			var counterClockwiseRotation = (previewMirroring && !IsUsingFrontCamera) || (!_isPreviewing && IsUsingFrontCamera);

			switch (DisplayInformation.GetForCurrentView().CurrentOrientation)
			{
				case DisplayOrientations.Portrait:
					return counterClockwiseRotation ? VideoRotation.Clockwise270Degrees : VideoRotation.Clockwise90Degrees;

				case DisplayOrientations.LandscapeFlipped:
					return VideoRotation.Clockwise180Degrees;

				case DisplayOrientations.PortraitFlipped:
					return counterClockwiseRotation ? VideoRotation.Clockwise90Degrees : VideoRotation.Clockwise270Degrees;

				default:
					return VideoRotation.None;
			}
		}

		/// <summary>
		/// Associates the best possible resolution with the particular media stream type.
		/// </summary>
		/// <param name="mediaStreamType">
		/// The media stream type to associate the best possible resolution with.
		/// </param>
		private async Task SetResolutionAsync(MediaStreamType mediaStreamType)
		{
			// TODO: ::NOTE::
			// I believe the best SubType GUID's are YUY2, UYVY and NV12 - they have the 
			// best sampleing and bits per channel. I also believe they are supported 
			// by every device. But that DOES need claraification from !testing! (or we fucked yo)
			// TODO: ::NOTE::

			var encodings = _deviceCapture.VideoDeviceController.GetAvailableMediaStreamProperties(mediaStreamType);
			if (encodings.Count <= 0) return;

			var resolutions = new List<Tuple<uint, uint, int>>();
			var perfectResolution = new Tuple<uint, uint, int>(0, 0, 0);

			for (int i = 0; i < encodings.Count; i++)
			{
				var properties = encodings[i] as VideoEncodingProperties;
				if (properties == null) continue;
				double frameRate = properties.FrameRate.Numerator / properties.FrameRate.Denominator;

				if (properties.Subtype.Equals("YUY2") || properties.Subtype.Equals("NV12") || properties.Subtype.Equals("UYVY"))
				{
					resolutions.Add(new Tuple<uint, uint, int>(properties.Width, properties.Height, i));

					if (properties.Width > perfectResolution.Item1)
						perfectResolution = new Tuple<uint, uint, int>(properties.Width, properties.Height, i);
				}
			}

			var distanceInfo = new Tuple<int, int>(-1, -1);
			if (perfectResolution.Item1 > 640)
			{
				// Get resolution closest to 480p (640x480)
				int index = 0;
				foreach (var resolution in resolutions)
				{
					var distance = Math.Abs(640 - (int) resolution.Item1);
					if (distanceInfo.Item1 == -1 || distance < distanceInfo.Item1)
					{
						distanceInfo = new Tuple<int, int>(distance, index++);
					}
				}

				var perfectIndex = distanceInfo.Item2;
				perfectResolution = new Tuple<uint, uint, int>(
					resolutions[perfectIndex].Item1,
					resolutions[perfectIndex].Item2,
					resolutions[perfectIndex].Item3
				);
			}

			PhotoCaptureWidth = perfectResolution.Item1;
			PhotoCaptureHeight = perfectResolution.Item2;

			await _deviceCapture.VideoDeviceController.SetMediaStreamPropertiesAsync(mediaStreamType, encodings[perfectResolution.Item3]);
			Debug.WriteLine("Set {0} resolution to {1}x{2}", mediaStreamType, perfectResolution.Item1, perfectResolution.Item2);
		}
	}
}