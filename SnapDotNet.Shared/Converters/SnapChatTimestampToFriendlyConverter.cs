using System;
using Windows.UI.Xaml.Data;
using SnapDotNet.Core.Miscellaneous.Helpers;

namespace SnapDotNet.Apps.Converters
{
	public class SnapChatTimestampToFriendlyConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			var dateTime = DateTime.Parse(value.ToString());
			return Time.GetReleativeDate(dateTime) ?? dateTime.ToString("dd/M/yy - HH:mm");
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			return value;
		}
	}
}