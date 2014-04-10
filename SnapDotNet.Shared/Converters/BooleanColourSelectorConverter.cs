using System;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace SnapDotNet.Apps.Converters
{
	public class BooleanColourSelectorConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			if ((bool) value)
				return TrueBrush;

			return FalseBrush;
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotImplementedException();
		}

		public SolidColorBrush TrueBrush { get; set; }

		public SolidColorBrush FalseBrush { get; set; }
	}
}
