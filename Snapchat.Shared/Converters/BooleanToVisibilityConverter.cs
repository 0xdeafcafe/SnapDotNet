using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Snapchat.Converters
{
	public sealed class BooleanToVisibilityConverter
		: IValueConverter
	{
		#region IValueConverter Members

		public object Convert(object value, Type targetType, object parameter, string language)
		{
			if (!IsInverted)
				return ((value as bool?) ?? true) ? Visibility.Visible : Visibility.Collapsed;

			return (!((value as bool?) ?? true)) ? Visibility.Visible : Visibility.Collapsed;
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotImplementedException();
		}

		public Boolean IsInverted { get; set; }

		#endregion
	}
}