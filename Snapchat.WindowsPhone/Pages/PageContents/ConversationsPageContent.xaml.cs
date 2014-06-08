using Snapchat.ViewModels.PageContents;
using System;
using Windows.Foundation;
using Windows.Phone.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;

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

		private void Grid_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
		{
			var grid = sender as Grid;
			if (grid == null) return;

			var cumX = e.Cumulative.Translation.X;
			if (cumX < 0) cumX = 0;
			if (cumX > 55) cumX = 55;
			cumX = cumX > 0 ? cumX : 0;

			var translateTransform = grid.RenderTransform as TranslateTransform;
			if (translateTransform == null) return;
			translateTransform.X = cumX;
		}

		private void Grid_ManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
		{
			var grid = sender as Grid;
			if (grid == null) return;

			var translateTransform = grid.RenderTransform as TranslateTransform;
			if (translateTransform == null) return;

			if (translateTransform.X >= 55)
			{
				// TODO: Scroll to one-on-one chat page
			}

			// Translate the item back to its original state.
			var storyboard = grid.Resources["CloseChatPeekIndicatorStoryboard"] as Storyboard;
			if (storyboard == null) return;
			storyboard.Begin();
		}
	}
}
