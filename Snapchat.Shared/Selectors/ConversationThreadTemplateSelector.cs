using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using SnapDotNet.Core.Snapchat.Models.AppSpecific;

namespace Snapchat.Selectors
{
	public class ConversationThreadTemplateSelector : DataTemplateSelector
	{
		public DataTemplate UserHeaderDataTemplate { get; set; }
		public DataTemplate TimeSeperatorDataTemplate { get; set; }

		protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
		{
			var selectedDataTemplate = item is TimeSeperator
				? TimeSeperatorDataTemplate
				: UserHeaderDataTemplate;

			return selectedDataTemplate;
		}
	}
}
