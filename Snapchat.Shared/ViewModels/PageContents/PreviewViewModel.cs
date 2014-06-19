using Windows.UI.Xaml.Media.Imaging;

namespace Snapchat.ViewModels.PageContents
{
	public class PreviewViewModel
		: BaseViewModel
	{
		public PreviewViewModel(BitmapImage bitmapImage)
		{
			BitmapImage = bitmapImage;
		}

		public BitmapImage BitmapImage
		{
			get { return _bitmapImage; }
			set { TryChangeValue(ref _bitmapImage, value); }
		}
		private BitmapImage _bitmapImage;
	}
}
