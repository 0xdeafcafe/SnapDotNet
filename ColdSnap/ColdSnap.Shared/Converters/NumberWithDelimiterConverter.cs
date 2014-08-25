using System;
using Windows.UI.Xaml.Data;

namespace ColdSnap.Converters
{
	public class NumberWithDelimiterConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			var number = int.Parse(value.ToString());
			return number >= 1000 ? number.ToString("n0") : number.ToString("d");
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotImplementedException();
		}
	}
}
