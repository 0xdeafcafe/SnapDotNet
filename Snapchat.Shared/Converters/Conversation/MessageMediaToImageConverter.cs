using System;
using Windows.UI.Xaml.Data;
using SnapDotNet.Core.Miscellaneous.Extensions;
using SnapDotNet.Core.Miscellaneous.Helpers.Async;
using SnapDotNet.Core.Snapchat.Models.New;

namespace Snapchat.Converters.Conversation
{
	public class MessageMediaToImageConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			var chatMessage = value as ChatMessage;
			if (chatMessage == null) return null;

			var iv = chatMessage.Body.Media.Iv;
			var key = chatMessage.Body.Media.Key;
			var id = chatMessage.Body.Media.MediaId;
			var data = AsyncHelpers.RunSync(() => App.SnapchatManager.Endpoints.GetChatMediaAsync(id, iv, key));

			return data.ToBitmapImage();
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotImplementedException();
		}
	}
}
