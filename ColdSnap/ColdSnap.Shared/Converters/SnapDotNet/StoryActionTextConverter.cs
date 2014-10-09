using System;
using System.Linq;
using Windows.UI.Xaml.Data;
using SnapDotNet.Data;

namespace ColdSnap.Converters.SnapDotNet
{
    public sealed class StoryActionTextConverter
		: IValueConverter
    {
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			var friend = value as Friend;
			if (friend == null) return null;

			foreach (var story in friend.Stories)
			{
				if (!story.IsCached && !story.IsDownloading)
					return App.Strings.GetString("DownloadStoryActionText");

				if (story.IsDownloading)
					return App.Strings.GetString("DownloadingStoryActionText");
			}

			if (friend.Stories.Any())
				return App.Strings.GetString("HoldToViewStoryActionText");

			//return friend.HasDisplayName ? friend.Username : null;
			return friend.Username;
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotImplementedException();
		}
	}
}
