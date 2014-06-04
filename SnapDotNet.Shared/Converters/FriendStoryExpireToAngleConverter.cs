using System;
using System.Linq;
using Windows.UI.Xaml.Data;
using SnapDotNet.Core.Snapchat.Models;

namespace SnapDotNet.Apps.Converters
{
    public class FriendStoryExpireToAngleConverter : IValueConverter
    {
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			var friend = value as Friend;
			if (friend == null) return 0;

			var story = App.SnapchatManager.Stories.FriendStories.FirstOrDefault(f => f.Username == friend.Name);
			if (story == null) return 0;

			var lastStory = story.Stories.FirstOrDefault();
			if (lastStory == null) return 0;

			return (360 / TimeSpan.FromDays(1).TotalMilliseconds) * lastStory.Story.TimeLeft.TotalMilliseconds;
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotImplementedException();
		}
	}
}
