using System;
using System.Linq;
using Windows.UI.Xaml.Data;

namespace Snapchat.Converters.Conversation
{
	public class FriendlyFriendNameConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			var name = ((String) value).ToLowerInvariant();
			if (name == App.SnapchatManager.SnapchatData.UserAccount.Username.ToLowerInvariant())
				return OutputLowercase ? "me" : "ME";

			var friend = App.SnapchatManager.SnapchatData.UserAccount.Friends.FirstOrDefault(f => f.Name.ToLowerInvariant() == name);

			return friend == null
				? OutputLowercase
					? name
					: name.ToUpperInvariant()
				: OutputLowercase
					? friend.FriendlyName.Split(' ')[0].ToLowerInvariant()
					: friend.FriendlyName.Split(' ')[0].ToUpperInvariant();
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotImplementedException();
		}

		public Boolean OutputLowercase { get; set; }
	}
}
