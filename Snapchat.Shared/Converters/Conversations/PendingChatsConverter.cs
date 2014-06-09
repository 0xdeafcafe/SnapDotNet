using System;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
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
				return Visibility.Collapsed;

			var hasPendingChats = false;
			foreach (var pendingChat in conversation.PendingChatsFor.Where(pendingChat => pendingChat == App.SnapchatManager.AllUpdates.UpdatesResponse.Username))
				hasPendingChats = true;

			return hasPendingChats
				? Visibility.Visible
				: Visibility.Collapsed;
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotImplementedException();
		}
	}
}
