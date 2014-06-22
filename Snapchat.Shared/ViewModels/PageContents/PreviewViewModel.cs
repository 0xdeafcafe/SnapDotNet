using System;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace Snapchat.ViewModels.PageContents
{
	public class PreviewViewModel
		: BaseViewModel
	{
		public PreviewViewModel(ImageSource source)
		{
			ImageSource = source;
		}

		public ImageSource ImageSource
		{
			get { return _imageSource; }
			set { TryChangeValue(ref _imageSource, value); }
		}
		private ImageSource _imageSource;

		public string CurrentTimeFilterTime { get { return DateTime.Now.ToString("h:mm"); } }
		public string CurrentTimeFilterDesignator { get { return DateTime.Now.ToString("tt"); } }
	}
}

