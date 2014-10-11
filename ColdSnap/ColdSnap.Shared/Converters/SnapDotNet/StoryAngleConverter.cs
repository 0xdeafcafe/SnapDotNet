using System;
using Windows.UI.Xaml.Data;
using SnapDotNet.Data;

namespace ColdSnap.Converters.SnapDotNet
{
	public class StoryAngleConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			var story = value as FriendStory;

			if (story == null || story.Owner.DontDecayThumbnail)
				return 360;

			var timeLeft = story.ExpiresAt - DateTime.UtcNow;
			return 360 - (360 / TimeSpan.FromDays(1).TotalMilliseconds) * timeLeft.TotalMilliseconds;
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotImplementedException();
		}
	}
}