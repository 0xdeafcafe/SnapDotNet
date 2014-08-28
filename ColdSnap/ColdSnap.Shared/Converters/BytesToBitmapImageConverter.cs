using System;
using Windows.UI.Xaml.Data;
using SnapDotNet.Extentions;

namespace ColdSnap.Converters
{
	public class BytesToBitmapImageConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			var data = value as byte[];
			return data == null ? null : data.ToBitmapImage();
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotImplementedException();
		}
	}
}
