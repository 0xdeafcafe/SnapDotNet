using System;
using Windows.UI.Xaml.Data;
using SnapDotNet.Core.Snapchat.Models.AppSpecific;

namespace Snapchat.Converters.OutboundSelectFriends
{
	public sealed class SelectedItemToFriendlyTextConverter
		: IValueConverter
	{
		#region IValueConverter Members

		public object Convert(object value, Type targetType, object parameter, string language)
		{
			if (value is SelectedFriend)
				return (value as SelectedFriend).Friend.FriendlyName;

			if (value is SelectedRecent)
				return (value as SelectedRecent).RecentName;

			if (value is SelectedStory)
				return (value as SelectedStory).StoryName;

			return null;
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}