using System;
using Windows.UI.Xaml.Data;

namespace Snapchat.Converters
{
	public sealed class IntegerConverter
		: IValueConverter
	{
		#region IValueConverter Members

		public object Convert(object value, Type targetType, object parameter, string language)
		{
			return (int) value;
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			return System.Convert.ChangeType(value, targetType);
		}

		#endregion
	}
}