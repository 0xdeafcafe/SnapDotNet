using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace SnapDotNet.Apps.Converters
{
	public sealed class GreaterThanZeroToVisibilityConverter
		 : IValueConverter
	{
		public bool IsInverse { get; set; }

		public object Convert(object value, Type targetType, object parameter, string language)
		{
			if (IsInverse)
				return (decimal) value > 0 ? Visibility.Collapsed : Visibility.Visible;
			else
				return (decimal) value > 0 ? Visibility.Visible : Visibility.Collapsed;
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotImplementedException();
		}
	}
}
