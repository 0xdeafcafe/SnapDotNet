using System.Linq;
using Windows.UI.Input;
using Snapchat.ViewModels.PageContents;
using SnapDotNet.Core.Snapchat.Models.New;
using Windows.Phone.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Animation;

namespace Snapchat.Pages.PageContents
{
	public sealed partial class ConversationsPageContent
	{
		public ConversationsViewModel ViewModel { get; private set; }

		public ConversationsPageContent()
		{
			InitializeComponent();
			DataContext = ViewModel = new ConversationsViewModel();

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

		private async void ConvoItem_Tapped(object sender, TappedRoutedEventArgs e)
		{
			var frameworkElement = sender as FrameworkElement;
			if (frameworkElement == null) return;
			var storyboard = frameworkElement.Resources["ConvoItemDetailPeekStoryboard"] as Storyboard;
			if (storyboard != null) storyboard.Begin();
			if (storyboard != null)
				storyboard.Completed += delegate
				{
					var storyboardClose = frameworkElement.Resources["ConvoItemDetailCloseStoryboard"] as Storyboard;
					if (storyboardClose != null)
						storyboardClose.Begin();
				};

			var conversation = frameworkElement.DataContext as ConversationResponse;
			if (conversation == null) return;
			var pendingSnaps = conversation.PendingReceivedSnaps;
			if (pendingSnaps == null || !pendingSnaps.Any()) return;
			await conversation.LastPendingSnap.DownloadSnapBlobAsync(App.SnapchatManager);
		}

		private void ConvoItem_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
		{
			var element = sender as FrameworkElement;
			if (element == null) return;
			MainPage.Singleton.ShowConversation(element.DataContext as ConversationResponse);
			var storyboard = element.Resources["ConvoItemDetailPeekStoryboard"] as Storyboard;
			if (storyboard == null) return;

			storyboard.Completed += delegate
			{
				var storyboardClose = element.Resources["ConvoItemDetailCloseStoryboard"] as Storyboard;
				if (storyboardClose != null)
					storyboardClose.Begin();
			};
			storyboard.SkipToFill();
		}

		private void ConvoItem_OnHolding(object sender, HoldingRoutedEventArgs e)
		{
			var element = sender as FrameworkElement;
			if (element == null) return;
			var conversation = element.DataContext as ConversationResponse;
			if (conversation == null) return;

			if (e.HoldingState == HoldingState.Started)
			{
				if (conversation.HasPendingSnaps)
				{
					MainPage.Singleton.ShowSnapMedia(conversation.PendingReceivedSnaps.Last());
					ScrollViewer.IsEnabled = false;
				}
			}
			else
			{
				MainPage.Singleton.HideSnapMedia();
				ScrollViewer.IsEnabled = true;
			}
		}
	}
}
