using System;
using System.Linq;
using Windows.UI;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using SnapDotNet.Core.Snapchat.Models.New;

namespace Snapchat.Converters.Conversations
{
	// ReSharper disable UnusedVariable
	public class PendingChatsConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			var conversation = (ConversationResponse)value;
			if (conversation == null)
				return null;

			// get the snap object
			if (conversation.PendingChatsFor == null)
				return new SolidColorBrush(Colors.Transparent);

			var hasPendingChats = false;
			foreach (var pendingChat in conversation.PendingChatsFor.Where(pendingChat => pendingChat == App.SnapchatManager.AllUpdates.UpdatesResponse.Username))
				hasPendingChats = true;

			return hasPendingChats
				? new SolidColorBrush(Colors.White)
				: new SolidColorBrush(Colors.Transparent);
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotImplementedException();
		}
	}
}
