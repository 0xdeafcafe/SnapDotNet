using System.Linq;
using Windows.Phone.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Snapchat.Controls;
using Snapchat.Helpers;
using Snapchat.ViewModels.PageContents;
using WinRTXamlToolkit.Controls.Extensions;

namespace Snapchat.Pages.PageContents
{
	public sealed partial class FriendsPageContent
	{
		public FriendsViewModel ViewModel { get; private set; }

		public FriendsPageContent()
		{
			InitializeComponent();
			DataContext = ViewModel = new FriendsViewModel();

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

		private void AddFriendsIcon_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
		{
			MainPage.Singleton.GoToAddFriendsPage();
		}

		private void ExpanderView_OnExpanded(object sender, bool e)
		{
			var expander = sender as ExpanderView;
			if (expander == null) return;

			// Hide the others
			foreach (
				var itemExpander in
					FriendsListView.Items.Select(
						item =>
							VariousHelpers.FindVisualChild<ExpanderView>(FriendsListView.ItemContainerGenerator.ContainerFromItem(item))))
			{
				if (itemExpander.Tag == expander.Tag) continue;
				itemExpander.Contract();
			}
		}
	}
}
