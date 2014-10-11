
using Windows.Media.Capture;
using Windows.UI.Xaml.Controls;
using ColdSnap.Helpers;
using SnapDotNet.Data;

namespace ColdSnap.ViewModels
{
	public sealed class CameraPageViewModel
		: BaseViewModel
	{
		private MediaCapture _mcapSource;
		public MediaCapture MediaCaptureSource //databind viewfinder to this property
		{
			get { return _mcapSource; }
			set { _mcapSource = value; }
		}
		public CameraPageViewModel()
		{
			
		}
		//public CameraPageViewModel(MediaCapture mediaCaptureToBind)
		//{
		//	_mcapSource = mediaCaptureToBind;
		//}
	}
}
