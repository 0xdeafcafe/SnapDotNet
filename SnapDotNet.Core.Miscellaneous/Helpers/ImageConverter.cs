using System;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;

namespace SnapDotNet.Core.Miscellaneous.Helpers
{
	public static class ImageConverter
	{
		public static async Task<BitmapImage> ByteArrayToBitmapImageAsync(byte[] byteArray)
		{
			var bitmapImage = new BitmapImage();

			var stream = new InMemoryRandomAccessStream();
			await stream.WriteAsync(byteArray.AsBuffer());
			stream.Seek(0);

			bitmapImage.SetSource(stream);
			return bitmapImage;
		}
	}
}
