using System.Linq;
using Snapchat.Models;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Snapchat.Selectors.Conversations
{
	public class SnapTemplateSelector : DataTemplateSelector
	{
		public DataTemplate PendingSnapDataTemplate { get; set; }
		public DataTemplate HistoricSnapDataTemplate { get; set; }

		protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
		{
			var conversation = item as Conversation;
			if (conversation == null) return null;
			return conversation.ConversationMessages.Snaps.Any(s => s.IsIncoming && s.Status == SnapStatus.Delivered)
				? PendingSnapDataTemplate
				: HistoricSnapDataTemplate;
		}
	}
}
