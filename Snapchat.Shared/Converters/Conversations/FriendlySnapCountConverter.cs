using System;
using Windows.UI.Xaml.Data;

namespace Snapchat.Converters.Conversations
{
	public class FriendlySnapCountConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			var snapCount = System.Convert.ToInt32(value);

			if (snapCount <= 0) return "";
			return snapCount > 99 ? "99+" : value;
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotImplementedException();
		}
	}
}
