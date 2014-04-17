using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;

namespace SnapDotNet.Core.Miscellaneous.Extensions
{
	public static class ByteArrayExtensions
	{
		public static BitmapImage ToBitmapImage(this byte[] byteArray)
		{
			if (byteArray == null) return null;

			using (var stream = new InMemoryRandomAccessStream())
			{
				// Writes the image byte array in an InMemoryRandomAccessStream
				// that is needed to set the source of BitmapImage.
				using (var writer = new DataWriter(stream.GetOutputStreamAt(0)))
				{
					writer.WriteBytes(byteArray);
					writer.StoreAsync().GetResults();
				}

				var image = new BitmapImage();
				image.SetSource(stream);
				return image;
			}
		}
	}
}
