using Windows.Phone.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Snapchat.ViewModels.PageContents;

namespace Snapchat.Pages.PageContents
{
	public sealed partial class FriendsPageContent
	{
		public FriendsViewModel ViewModel { get; private set; }

		public FriendsPageContent()
		{
			InitializeComponent();
			DataContext = ViewModel = new FriendsViewModel();
			((CollectionViewSource) Resources["SortedFriends"]).Source = ViewModel.Account.SortedFriends;

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
