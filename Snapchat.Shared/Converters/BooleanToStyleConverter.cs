using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Snapchat.Converters
{
	public sealed class BooleanToStyleConverter
		: IValueConverter
	{
		#region IValueConverter Members

		public object Convert(object value, Type targetType, object parameter, string language)
		{
			return ((bool) value) ? TrueStyle : FalseStyle;
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotImplementedException() ;
		}

		public Style TrueStyle { get; set; }

		public Style FalseStyle { get; set; }

		#endregion
	}
}