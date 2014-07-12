using Snapchat.Models;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Snapchat.Selectors.Conversations
{
	public class ConversationTemplateSelector : DataTemplateSelector
	{
		public DataTemplate SnapDataTemplate { get; set; }
		public DataTemplate UploadingSnapDataTemplate { get; set; }

		protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
		{
			var conversation = item as IConversation;
			if (conversation == null) return null;
			switch (conversation.ConversationType)
			{
				case ConversationType.PendingUpload:
					return UploadingSnapDataTemplate;

				case ConversationType.Person2Person:
					return SnapDataTemplate;

				default:
					return null;
			}
		}
	}
}
