using SnapDotNet.Core.Snapchat.Models.New;
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
			var conversation = item as ConversationResponse;
			if (conversation == null) return null;
			return conversation.HasPendingSnaps
				? PendingSnapDataTemplate
				: HistoricSnapDataTemplate;
		}
	}
}
