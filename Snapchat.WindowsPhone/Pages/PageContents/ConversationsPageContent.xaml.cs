using System;
using System.Diagnostics;
using System.Linq;
using Windows.UI.Input;
using Snapchat.ViewModels.PageContents;
using SnapDotNet.Core.Snapchat.Models.New;
using Windows.Phone.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Controls;
using Windows.Foundation;

namespace Snapchat.Pages.PageContents
{
	public sealed partial class ConversationsPageContent
	{
		private bool _isFingerDown;
		private ConversationResponse _selectedConversation;
		private readonly DispatcherTimer _holdingTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(300) };
		private bool _alreadyRefreshed;

		public ConversationsPageContent()
		{
			InitializeComponent();
			DataContext = ViewModel = new ConversationsViewModel();

			HardwareButtons.CameraPressed += delegate
			{
				// Remove focus from SearchBox
				MainPage.Singleton.CapturePhotoButton.Focus(FocusState.Programmatic);
			};
			HardwareButtons.BackPressed += (sender, args) =>
			{
				DetatchConvoData();
				TryHideMediaContent();
				args.Handled = true;
			};

			_holdingTimer.Tick += (o, o1) =>
			{
				_holdingTimer.Stop();
				if (!_isFingerDown || _selectedConversation == null) return;

				// show nudes
				MainPage.Singleton.ShowSnapMedia(_selectedConversation.PendingReceivedSnaps.Last());
				ScrollViewer.IsEnabled = false;

				Debug.WriteLine("we holdin");
			};
		}

		public ConversationsViewModel ViewModel { get; private set; }

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
				SetupSnapData(conversation);
			else
				TryHideMediaContent();
		}

		private void SetupSnapData(ConversationResponse conversation)
		{
			_isFingerDown = true;
			_selectedConversation = conversation;
		}

		private void DetatchConvoData()
		{
			_isFingerDown = false;
			_selectedConversation = null;
			ScrollViewer.IsEnabled = true;
			_holdingTimer.Stop();
		}

		private void TryHideMediaContent()
		{
			MainPage.Singleton.HideSnapMedia();
			ScrollViewer.IsEnabled = true;
		}

		#region Old holding stuff xo

		//private void UIElement_OnPointerEntered(object sender, PointerRoutedEventArgs e)
		//{
		//	//Debug.WriteLine("UIElement_OnPointerEntered");

		//	var element = sender as FrameworkElement;
		//	if (element == null) return;
		//	var conversation = element.DataContext as ConversationResponse;
		//	if (conversation == null || !conversation.HasPendingSnaps) return;
		//	SetupSnapData(conversation);
		//	_holdingTimer.Start();
		//}

		//private void UIElement_OnPointerCanceled(object sender, PointerRoutedEventArgs e)
		//{
		//	//Debug.WriteLine("UIElement_OnPointerCanceled");
		//	DetatchConvoData();
		//}

		//private void UIElement_OnPointerCaptureLost(object sender, PointerRoutedEventArgs e)
		//{
		//	//Debug.WriteLine("UIElement_OnPointerCaptureLost");
		//	DetatchConvoData();
		//}

		//private void UIElement_OnPointerExited(object sender, PointerRoutedEventArgs e)
		//{
		//	//Debug.WriteLine("UIElement_OnPointerExited");
		//	DetatchConvoData();
		//}

		//private void UIElement_OnPointerMoved(object sender, PointerRoutedEventArgs e)
		//{
		//	//Debug.WriteLine("UIElement_OnPointerMoved");
		//}

		//private void UIElement_OnPointerPressed(object sender, PointerRoutedEventArgs e)
		//{
		//	//Debug.WriteLine("UIElement_OnPointerPressed");
		//}

		//private void UIElement_OnPointerReleased(object sender, PointerRoutedEventArgs e)
		//{
		//	Debug.WriteLine("UIElement_OnPointerReleased");
		//	DetatchConvoData();
		//	TryHideMediaContent();
		//	Debug.WriteLine("Ending _holdTimer");
		//}

		#endregion

		private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			var textBox = sender as TextBox;
			if (textBox != null) ViewModel.FilterText = textBox.Text;
		}

		private void flashy_Loaded(object sender, RoutedEventArgs e)
		{
			var flashy = sender as UIElement;
			if (flashy == null) return;

			var refreshTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(100) };
			refreshTimer.Tick += delegate
			{
				Point screenCoords = flashy.TransformToVisual(Window.Current.Content).TransformPoint(new Point(0, 0));
				if (screenCoords.Y > 0 && !_alreadyRefreshed)
				{
					_alreadyRefreshed = true;
					App.UpdateSnapchatDataAsync();
				}
				else if (screenCoords.Y < 0)
				{
					_alreadyRefreshed = false;
				}
			};
			refreshTimer.Start();
		}
	}
}

