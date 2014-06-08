using System;
using Windows.UI.Xaml.Data;
using Snapchat.Extentions;
using SnapDotNet.Core.Snapchat.Models.New;

namespace Snapchat.Converters.Conversations
{
	public class FriendlySnapTimeConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			var conversation = (ConversationResponse)value;
			if (conversation == null)
				return null;

			var lastInteractionTime = conversation.LastInteraction;
			var now = DateTime.Now;

			if (lastInteractionTime.Year != now.Year || lastInteractionTime.Month != now.Month)
				return String.Format("{0:MMMM d}{1}", lastInteractionTime, lastInteractionTime.Day.ToOrdinal());
			if (lastInteractionTime.Day == now.Day)
				return String.Format("{0} {1:HH:mm tt}", "Today", lastInteractionTime);
			return lastInteractionTime.Day == now.Day - 1
				? String.Format("{0} {1:HH:mm tt}", "Yesterday", lastInteractionTime)
				: String.Format("{0:MMMM d}{1}", lastInteractionTime, lastInteractionTime.Day.ToOrdinal());
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotImplementedException();
		}
	}
}
