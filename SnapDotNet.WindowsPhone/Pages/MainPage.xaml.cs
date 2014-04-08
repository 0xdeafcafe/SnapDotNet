using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using System.Windows;
using Windows.UI.Xaml.Navigation;
using Windows.Media.Capture;
using Windows.Devices;

namespace SnapDotNet.Apps.Pages
{
	public sealed partial class MainPage : Page
	{
		private Windows.Media.Capture.MediaCapture _mediaCapture;
		private DeviceInformationCollection _cameraInfoCollection;
		private DeviceInformationCollection _microphoneInfoCollection;
		public MainPage()
		{
			this.InitializeComponent();
		}
		protected async override void OnNavigatedTo(NavigationEventArgs e)
		{
			CameraPreview.Source = _mediaCapture;
			StartCameraDevice();

		}

		protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
		{
			_mediaCapture.StopPreviewAsync();
		}
		private async Task StartCameraDevice()
		{
			_mediaCapture = new Windows.Media.Capture.MediaCapture();
			_cameraInfoCollection = await DeviceInformation.FindAllAsync(DeviceClass.VideoCapture);
			_microphoneInfoCollection = await DeviceInformation.FindAllAsync(DeviceClass.AudioCapture);

			var settings = new Windows.Media.Capture.MediaCaptureInitializationSettings();
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
