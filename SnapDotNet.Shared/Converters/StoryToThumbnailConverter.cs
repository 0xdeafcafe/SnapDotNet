using System;
using Windows.UI.Xaml.Data;
using SnapDotNet.Core.Miscellaneous.Extensions;
using SnapDotNet.Core.Snapchat.Models;

namespace SnapDotNet.Apps.Converters
{
	public class StoryToThumbnailConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			var story = value as Story;
			if (story == null) return null;

			// download thumbnail blob
			var blob = App.SnapchatManager.Endpoints.GetStoryThumbnailBlob(story);

			return (blob == null) ? null : blob.ToBitmapImage();
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotImplementedException();
		}
	}
}
