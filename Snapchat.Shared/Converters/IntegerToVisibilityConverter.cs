using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Snapchat.Converters
{
	public sealed class IntegerToVisibilityConverter
		: IValueConverter
	{
		#region IValueConverter Members

		public object Convert(object value, Type targetType, object parameter, string language)
		{
			if (IsInverted)
				return (((value as int?) ?? 0) == 0) ? Visibility.Visible : Visibility.Collapsed;

			return (((value as int?) ?? 0) > 0) ? Visibility.Visible : Visibility.Collapsed;
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotImplementedException();
		}

		public Boolean IsInverted { get; set; }

		#endregion
	}
}