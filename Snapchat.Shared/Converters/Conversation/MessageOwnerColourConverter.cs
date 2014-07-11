using System;
using Windows.UI;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace Snapchat.Converters.Conversation
{
	public class MessageOwnerColourConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			return ((String) value).ToLowerInvariant() == App.SnapchatManager.SnapchatData.UserAccount.Username.ToLowerInvariant()
				? new SolidColorBrush(Color.FromArgb(0xFF, 0x3C, 0xB2, 0xE2))
				: new SolidColorBrush(Color.FromArgb(0xFF, 0xE9, 0x27, 0x54));
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotImplementedException();
		}
	}
}
