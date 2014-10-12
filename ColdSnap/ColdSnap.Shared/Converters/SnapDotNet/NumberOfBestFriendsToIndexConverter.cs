using System;
using Windows.UI.Xaml.Data;

namespace ColdSnap.Converters.SnapDotNet
{
    public sealed class NumberOfBestFriendsToIndexConverter
		: IValueConverter
    {
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			return (System.Convert.ToInt32(value) - 3)/2;
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			return System.Convert.ToInt32(value)*2 + 3;
		}
	}
}
