using System;
using Windows.UI.Xaml.Data;
using Snapchat.Extentions;

namespace Snapchat.Converters
{
	public class FriendlyTimeConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			var lastInteractionTime = (DateTime)value;
			var now = DateTime.Now;

			if (lastInteractionTime.Year != now.Year || lastInteractionTime.Month != now.Month)
				return String.Format("{0:MMMM d}{1}", lastInteractionTime, lastInteractionTime.Day.GetOrdinal());
			if (lastInteractionTime.Day == now.Day)
				return String.Format("{0} {1:hh:mm tt}", App.Strings.GetString("TimeToday"), lastInteractionTime);
			return lastInteractionTime.Day == now.Day - 1
				? String.Format("{0} {1:HH:mm tt}", App.Strings.GetString("TimeYesterday"), lastInteractionTime)
				: String.Format("{0:MMMM d}{1}", lastInteractionTime, lastInteractionTime.Day.GetOrdinal());
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotImplementedException();
		}
	}
}
