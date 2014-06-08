﻿using Snapchat.ViewModels.PageContents;
using Windows.Phone.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

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
			var indicator = (sender as Grid).Children[0] as Grid;
			double cumX = e.Cumulative.Translation.X;
			indicator.Width = cumX > 0 ? cumX : 0;
		}

		private void Grid_ManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
		{
			var container = sender as Grid;
			var indicator = container.Children[0] as Grid;

			if (indicator.ActualWidth >= container.ActualWidth) 
			{
				// TODO: Make main page ScrollViewer scroll to the newly available individual chat page.
			}
			else
			{
				// TODO: Go back to 0 with easing (invoke storyboard or something?)
			}
		}
	}
}
