using System;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using Snapchat.SnapLogic.Models.New;

namespace Snapchat.Converters.Conversation
{
	public class MessageStateConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			var chatMessage = value as ChatMessage;
			if (chatMessage == null) return null;

			var isSaved = chatMessage.SavedStates.ContainsKey(App.SnapchatManager.Username) && 
						  chatMessage.SavedStates[App.SnapchatManager.Username].Saved;

			if (IsForBorderThickness)
				return new Thickness(isSaved ? 2 : 1, 0, 0, 0);

			return new SolidColorBrush(
				isSaved ? new Color { A = 0xFF, R = 0xF3, G = 0xF3, B = 0xF3 } : Colors.White);
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotImplementedException();
		}

		public bool IsForBorderThickness { get; set; }
	}
}
