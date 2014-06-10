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

		protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
		{
			var messageContainer = (MessageContainer) item;

			var selectedDataTemplate = (messageContainer.ChatMessage == null)
				? MessageSnapDataTemplate
				: messageContainer.ChatMessage.Body.Type == MessageBodyType.Screenshot
					? MessageChatScreenshotDataTemplate
					: MessageChatDataTemplate;

			return selectedDataTemplate;
		}
	}
}
