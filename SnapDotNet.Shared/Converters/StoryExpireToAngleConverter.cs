using System;
using System.Linq;
using Windows.UI.Xaml.Data;
using SnapDotNet.Core.Snapchat.Models;

namespace SnapDotNet.Apps.Converters
{
	public class StoryExpireToAngleConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			var story = value as Story;
			if (story == null) return 0;

			return (360 / TimeSpan.FromDays(1).TotalMilliseconds) * story.TimeLeft.TotalMilliseconds;
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotImplementedException();
		}
	}
}