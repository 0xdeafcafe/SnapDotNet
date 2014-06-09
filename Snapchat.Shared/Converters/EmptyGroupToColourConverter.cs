using System;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace Snapchat.Converters
{
	public sealed class EmptyGroupToColourConverter
		: IValueConverter
	{
		#region IValueConverter Members

		public object Convert(object value, Type targetType, object parameter, string language)
		{
			return ((int)value > 0) ? TrueBrush : FalseBrush;
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotImplementedException();
		}

		public SolidColorBrush TrueBrush { get; set; }

		public SolidColorBrush FalseBrush { get; set; }

		#endregion
	}
}