using System;
using System.Diagnostics;
using System.Linq;
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

			//if (e.HoldingState == HoldingState.Started)
			//{
			//	if (!conversation.HasPendingSnaps) return;
			//	MainPage.Singleton.ShowSnapMedia(conversation.PendingReceivedSnaps.Last());
			//	ScrollViewer.IsEnabled = false;
			//}
			//else
			//{
			//	MainPage.Singleton.HideSnapMedia();
			//	ScrollViewer.IsEnabled = true;
			//}
		}

		private void SetupSnapData(ConversationResponse conversation)
		{
			_isInDeep = true;
			_selectedConversation = conversation;
		}
		private void DetatchConvoData()
		{
			_isInDeep = false;
			_selectedConversation = null;
		}

		private bool _isInDeep;
		private ConversationResponse _selectedConversation;
		private void UIElement_OnPointerEntered(object sender, PointerRoutedEventArgs e)
		{
			Debug.WriteLine("UIElement_OnPointerEntered");

			var element = sender as FrameworkElement;
			if (element == null) return;
			var conversation = element.DataContext as ConversationResponse;
			if (conversation == null) return;
			SetupSnapData(conversation);

			var timer = new DispatcherTimer
			{
				Interval = new TimeSpan(0, 0, 0, 0, 300)
			};
			timer.Tick += (o, o1) =>
			{
				timer.Stop();
				if (!_isInDeep || _selectedConversation == null) return;

				//ScrollViewer.IsEnabled = false;
				MainPage.Singleton.PointerCaptureLost += (sender1, args) => Debug.WriteLine("MainPage_PointerCaptureLost");
				MainPage.Singleton.PointerReleased += (sender1, args) => Debug.WriteLine("MainPage_PointerReleased");
				MainPage.Singleton.PointerExited += (sender1, args) => Debug.WriteLine("MainPage_PointerExited");

				Debug.WriteLine("we holdin");
			};
			timer.Start();
		}

		private void UIElement_OnPointerCanceled(object sender, PointerRoutedEventArgs e)
		{
			Debug.WriteLine("UIElement_OnPointerCanceled");
			DetatchConvoData();
		}

		private void UIElement_OnPointerCaptureLost(object sender, PointerRoutedEventArgs e)
		{
			Debug.WriteLine("UIElement_OnPointerCaptureLost");
			DetatchConvoData();
		}

		private void UIElement_OnPointerExited(object sender, PointerRoutedEventArgs e)
		{
			Debug.WriteLine("UIElement_OnPointerExited");
			DetatchConvoData();
		}

		private void UIElement_OnPointerMoved(object sender, PointerRoutedEventArgs e)
		{
			Debug.WriteLine("UIElement_OnPointerMoved");
		}

		private void UIElement_OnPointerPressed(object sender, PointerRoutedEventArgs e)
		{
			Debug.WriteLine("UIElement_OnPointerPressed");
		}

		private void UIElement_OnPointerReleased(object sender, PointerRoutedEventArgs e)
		{
			Debug.WriteLine("UIElement_OnPointerReleased");
		}
	}
}
