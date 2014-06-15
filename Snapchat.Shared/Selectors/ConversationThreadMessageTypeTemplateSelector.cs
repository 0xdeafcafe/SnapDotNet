using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using SnapDotNet.Core.Snapchat.Models.New;

namespace Snapchat.Selectors
{
	public class ConversationThreadMessageTypeTemplateSelector : DataTemplateSelector
	{
		public DataTemplate MessageSnapDataTemplate { get; set; }
		public DataTemplate MessageChatDataTemplate { get; set; }
		public DataTemplate MessageChatScreenshotDataTemplate { get; set; }
		public DataTemplate MessageChatMediaDataTemplate { get; set; }

		protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
		{
			var messageContainer = (MessageContainer) item;

			if (messageContainer.Snap != null)
				return MessageSnapDataTemplate;

			if (messageContainer.ChatMessage != null)
			{
				switch (messageContainer.ChatMessage.Body.Type)
				{
					case MessageBodyType.Text:
						return MessageChatDataTemplate;

					case MessageBodyType.Screenshot:
						return MessageChatScreenshotDataTemplate;

					case MessageBodyType.Media:
						return MessageChatMediaDataTemplate;
				}
			}

			throw new ArgumentOutOfRangeException("item");
		}
	}
}
