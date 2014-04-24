using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml.Data;

namespace SnapDotNet.Apps.Converters
{
    public sealed class InverseBooleanConverter
		: IValueConverter
    {
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			return !(bool) value;
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotImplementedException();
		}
	}
}
