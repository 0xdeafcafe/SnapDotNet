using Snapchat.ViewModels;

namespace Snapchat.Pages.PageContents
{
	public sealed partial class SettingsPageContent
	{
		public SettingsPageContent()
		{
			InitializeComponent();
			DataContext = new SettingsViewModel();
		}
	}
}
