using System;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media.Imaging;

namespace ColdSnap.Converters
{
	public class BytesToBitmapImageConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			var data = value as byte[];
			if (data != null)
			{
				using (var stream = new InMemoryRandomAccessStream())
				using (var writer = new DataWriter(stream.GetOutputStreamAt(0)))
				{
					writer.WriteBytes(data);
					writer.StoreAsync().GetResults();

					var image = new BitmapImage();
					image.SetSource(stream);
					return image;
				}
			}
			return null;
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotImplementedException();
		}
	}
}