using System;
using System.Linq;
using Windows.UI.Xaml.Data;

namespace Snapchat.Converters.Conversations
{
	public class FriendlyFriendNameConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			var friendName = (String) value;
			if (friendName == null)
				return "..unknown";

			// get the snap object
			var friend = App.SnapchatManager.SnapchatData.UserAccount.Friends.FirstOrDefault(f => f.Name == friendName);

			return friend == null ? friendName : friend.FriendlyName;
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotImplementedException();
		}
	}
}
