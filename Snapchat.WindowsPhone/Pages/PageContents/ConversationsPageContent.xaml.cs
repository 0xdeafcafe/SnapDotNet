using Snapchat.ViewModels.PageContents;
using Windows.Phone.UI.Input;
using Windows.UI.Xaml;

namespace Snapchat.Pages.PageContents
{
	public sealed partial class ConversationsPageContent
	{
		public ConversationsViewModel ViewModel { get; private set; }

		public ConversationsPageContent()
		{
			InitializeComponent();
			ViewModel = DataContext as ConversationsViewModel;

			HardwareButtons.CameraPressed += delegate
			{
				// Remove focus from SearchBox
				Focus(FocusState.Programmatic);
			};
		}

		private void SearchBox_GotFocus(object sender, RoutedEventArgs e)
		{
			MainPage.Singleton.HideBottomAppBar();
		}

		private void SearchBox_LostFocus(object sender, RoutedEventArgs e)
		{
			MainPage.Singleton.RestoreBottomAppBar();
		}
	}
}
