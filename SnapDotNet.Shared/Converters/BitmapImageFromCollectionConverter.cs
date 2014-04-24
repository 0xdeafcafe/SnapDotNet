using System;
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media.Imaging;

namespace SnapDotNet.Apps.Converters
{
	public class BitmapImageFromCollectionConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			var array = (ObservableCollection<BitmapImage>)value;

			return array[Int32.Parse(parameter as string)];
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotImplementedException();
		}
	}
}
