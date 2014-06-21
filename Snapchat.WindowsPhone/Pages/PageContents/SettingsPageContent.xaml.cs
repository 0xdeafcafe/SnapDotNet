using Windows.System;
using Snapchat.ViewModels;
using System;
using Windows.UI.Xaml;

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
