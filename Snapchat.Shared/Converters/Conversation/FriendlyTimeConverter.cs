using System;
using Windows.UI.Xaml.Data;
using Snapchat.Extentions;

namespace Snapchat.Converters.Conversation
{
	public class FriendlyTimeConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			var lastInteractionTime = (DateTime)value;
			var now = DateTime.Now;

			// Check if today
			if (lastInteractionTime.Year == now.Year &&
				lastInteractionTime.DayOfYear == now.DayOfYear)
				return String.Format("{0}", App.Strings.GetString("TimeToday"));

			// Check if yesterday
			if (lastInteractionTime.Year == now.Year &&
				lastInteractionTime.DayOfYear == now.DayOfYear - 1)
				return String.Format("{0}", App.Strings.GetString("TimeYesterday"));

			// Check if in last week
			var difference = now - lastInteractionTime;
			return difference.TotalDays <= 7
				? String.Format("{0:dddd}", lastInteractionTime)
				: String.Format("{0:MMMM d}{1}", lastInteractionTime, lastInteractionTime.Day.GetOrdinal());
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotImplementedException();
		}
	}
}
