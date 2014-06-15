using System;
using Windows.UI;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using SnapDotNet.Core.Snapchat.Models.New;

namespace Snapchat.Converters.Conversation
{
	public class UserHeaderChildIsSavedConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			var firstChatMessage = value as ChatMessage;
			if (firstChatMessage == null) return null;

			var isSaved = firstChatMessage.SavedState.ContainsKey(App.SnapchatManager.Username) &&
						  firstChatMessage.SavedState[App.SnapchatManager.Username].Saved;

			return new SolidColorBrush(
				isSaved ? new Color { A = 0xFF, R = 0xF3, G = 0xF3, B = 0xF3 } : Colors.White);
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotImplementedException();
		}
	}
}
