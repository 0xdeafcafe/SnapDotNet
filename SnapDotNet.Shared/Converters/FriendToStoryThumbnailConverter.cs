using System;
using System.Linq;
using Windows.UI.Xaml.Data;
using SnapDotNet.Core.Miscellaneous.Extensions;
using SnapDotNet.Core.Snapchat.Models;

namespace SnapDotNet.Apps.Converters
{
	public class FriendToStoryThumbnailConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			var friend = value as Friend;
			if (friend == null) return null;

			var story = App.SnapChatManager.Stories.FriendStories.FirstOrDefault(f => f.Username == friend.Name);
			if (story == null) return null;

			var mostRecentStory = story.Stories.FirstOrDefault();
			if (mostRecentStory == null) return null;

			// download thumbnail blob
			var blob = App.SnapChatManager.Endpoints.GetStoryThumbnailBlob(mostRecentStory.Story);

			return (blob == null) ? null : blob.ToBitmapImageAsync();
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotImplementedException();
		}
	}
}
