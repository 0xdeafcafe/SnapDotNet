using System.Linq;
using Windows.UI.Input;
using Snapchat.ViewModels.PageContents;
using SnapDotNet.Core.Snapchat.Models.New;
using Windows.Phone.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Animation;
using Windows.System.Threading;
using System;

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
			storyboard.Completed += delegate
			{
				(frameworkElement.Resources["ConvoItemDetailCloseStoryboard"] as Storyboard).Begin();
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
			if (element != null)
			{
				MainPage.Singleton.ShowConversation(element.DataContext as ConversationResponse);
				var storyboard = element.Resources["ConvoItemDetailPeekStoryboard"] as Storyboard;
				if (storyboard != null)
				{
					storyboard.Completed += delegate
					{
						(element.Resources["ConvoItemDetailCloseStoryboard"] as Storyboard).Begin();
					};
					storyboard.SkipToFill();
				}
			}
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
					MainPage.Singleton.ShowSnapMedia(conversation.PendingReceivedSnaps.Last()); // TODO: Pass in the scroll viewer element, and disable it
			}
			else
				MainPage.Singleton.HideSnapMedia();
		}
	}
}
