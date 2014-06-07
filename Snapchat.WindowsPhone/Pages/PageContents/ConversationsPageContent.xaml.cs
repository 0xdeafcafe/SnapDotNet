using Snapchat.ViewModels.PageContents;

namespace Snapchat.Pages.PageContents
{
	public sealed partial class ConversationsPageContent
	{
		public ConversationsViewModel ViewModel { get; private set; }

		public ConversationsPageContent()
		{
			InitializeComponent();

			DataContext = ViewModel = new ConversationsViewModel();
		}
	}
}
