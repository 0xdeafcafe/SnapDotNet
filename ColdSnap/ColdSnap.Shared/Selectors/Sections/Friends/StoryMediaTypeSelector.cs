using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using SnapDotNet;

namespace ColdSnap.Selectors.Sections.Friends
{
	public class StoryMediaTypeSelector : DataTemplateSelector
	{
		public DataTemplate ImageDataTemplate { get; set; }
		public DataTemplate VideoDataTemplate { get; set; }
		
		protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
		{
			var story = item as Story;
			if (story == null) return null;

			return story.IsImage ? ImageDataTemplate : VideoDataTemplate;
		}
	}
}
