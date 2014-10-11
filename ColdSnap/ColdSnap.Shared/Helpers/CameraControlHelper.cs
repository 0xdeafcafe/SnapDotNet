using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Media.Capture;
using Windows.UI.Xaml.Controls;

namespace ColdSnap.Helpers
{
	public class CameraControlHelper
	{
		#region public

		private MediaCapture _mcap;
		private CaptureElement _cape;
		public MediaCapture MediaCapture
		{
			get { return _mcap; }
		}

		public CaptureElement CaptureElement
		{
			get { return _cape; }
		}

		#endregion

		private CameraStatusManager _s; //break and check this to debug
		private DeviceInformationCollection _camInfoCollection;
		private DeviceInformationCollection _micInfoCollection;
		private int _currentCamDeviceIndex = 0;
		private int _currentMicDeviceIndex = 0;

		public CameraControlHelper()
		{
			_s = new CameraStatusManager();
			_mcap = new MediaCapture();
			_cape = new CaptureElement();


		}

		public async Task InitializeCameraAsync()
		{
			if (!_s.areDevicesDiscovered)
			{
				await DiscoverDevicesAsync();
			}
			if (_s.isInitialized)
			{
				_mcap = new MediaCapture();
			}
			var settings = new MediaCaptureInitializationSettings
			{
				AudioDeviceId = _micInfoCollection[_currentMicDeviceIndex].Id,
				VideoDeviceId = _camInfoCollection[_currentCamDeviceIndex].Id,
				StreamingCaptureMode = StreamingCaptureMode.Video,
				PhotoCaptureSource = PhotoCaptureSource.Photo
			};
			
			await _mcap.InitializeAsync(settings);

			_s.isInitialized = true;
		}

		public async Task StartPreviewAsync()
		{
			if (_cape != null && !_s.isPreviewing)
			{
				_cape.Source = _mcap;
				await _mcap.StartPreviewAsync();
				_s.isPreviewing = true;
			}
		}
		public async Task DiscoverDevicesAsync() //may as well start running as soon as app load
		{
			if (_s.areDevicesDiscovered)
			{
				Debug.WriteLine("Devices already discovered");
				return;
			}

			var findCamTask = Task.Run(async () =>
			{
				_camInfoCollection = await DeviceInformation.FindAllAsync(DeviceClass.VideoCapture);
			});

			var findMicTask = Task.Run(async () =>
			{
				_micInfoCollection = await DeviceInformation.FindAllAsync(DeviceClass.AudioCapture);
			});

			Task.WaitAll(findCamTask, findMicTask);
		}
		private class CameraStatusManager
		{
			public bool isInitialized = false;
			public bool isPreviewing = false;
			public bool areDevicesDiscovered = false;
		}
	}
}
