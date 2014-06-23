﻿using System;
using System.Diagnostics;
using Windows.UI;
using Windows.UI.Input;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;
using Snapchat.ViewModels.PageContents;

namespace Snapchat.Pages.PageContents
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class PreviewPageContent
	{
		public PreviewViewModel ViewModel { get; private set; }

		public PreviewPageContent()
		{
			InitializeComponent();

			ScrollViewer.Loaded += (sender, args) => ScrollViewer.ScrollToHorizontalOffset(NoOverlayGrid.ActualWidth * 2);
			ScrollViewer.ViewChanged += (sender, args) =>
			{
				// TODO: Make this cleaner, im sure you'll find a way matt. But the basic stuff is here
				var pageIndex = (int)Math.Round(ScrollViewer.HorizontalOffset / NoOverlayGrid.ActualWidth);
				var pageCount = OverlaysContainer.Children.Count;
				var pageCountIndexFriendly = pageCount - 1;

				if (pageIndex == pageCountIndexFriendly)
					ScrollViewer.ScrollToHorizontalOffset(NoOverlayGrid.ActualWidth * 2); // *2 goes to no-overlay
				else if (pageIndex == 1)
					ScrollViewer.ScrollToHorizontalOffset(NoOverlayGrid.ActualWidth * 4); // *4 goes to time - i should map these
			};
		}

		public void Reset()
		{
			DataContext = null;
			ImageMediaElement.Source = null;

			VisualStateManager.GoToState(this, "PendingMedia", true);
		}

		public async void Load(PreviewViewModel viewModel)
		{
			DataContext = ViewModel = viewModel;
			ImageMediaElement.Source = ViewModel.ImageSource;

			await StatusBar.GetForCurrentView().HideAsync();
		}

		#region Drawing Code

		private bool _isDrawing;
		private PointerPoint _oldPoint;
		private void BitmapDrawingGrid_PointerEntered(object sender, PointerRoutedEventArgs e)
		{
			Debug.WriteLine("event :: BitmapDrawingGrid_PointerEntered");
			
			if (!(DrawingToggleButton.IsChecked ?? false))
				return;

			VisualStateManager.GoToState(this, "DrawingState", true);
			ScrollViewer.IsHitTestVisible = false;
			ScrollViewer.IsEnabled = false;

			_oldPoint = e.GetCurrentPoint(DrawingCanvas);
			_isDrawing = true;
		}

		private void BitmapDrawingGrid_OnPointerMoved(object sender, PointerRoutedEventArgs e)
		{
			Debug.WriteLine("event :: BitmapDrawingGrid_OnPointerMoved");

			if (!_isDrawing)
				return;

			var point = e.GetCurrentPoint(DrawingCanvas);
			ViewModel.DrawnLines.Add(new Line
			{
				Stroke = new SolidColorBrush(Colors.Red),
				StrokeThickness = 10,
				StrokeDashCap = PenLineCap.Round,
				StrokeLineJoin = PenLineJoin.Round,
				StrokeEndLineCap = PenLineCap.Round,
				StrokeStartLineCap = PenLineCap.Round,
				X1 = point.RawPosition.X,
				Y1 = point.RawPosition.Y,
				X2 = _oldPoint.RawPosition.X,
				Y2 = _oldPoint.RawPosition.Y
			});

			_oldPoint = point;
		}

		private void BitmapDrawingGrid_OnPointerReleased(object sender, PointerRoutedEventArgs e)
		{
			Debug.WriteLine("event :: BitmapDrawingGrid_OnPointerReleased");

			_isDrawing = false;
			VisualStateManager.GoToState(this, "PendingState", true);
			ScrollViewer.IsHitTestVisible = true;
			ScrollViewer.IsEnabled = true;
		}

		private void DrawingToggleButton_CheckedChanged(object sender, RoutedEventArgs e)
		{
			var isChecked = DrawingToggleButton.IsChecked ?? false;
			BitmapDrawingGrid.Visibility = isChecked ? Visibility.Visible : Visibility.Collapsed;
		}

		#endregion
		
		private void SelectFriendsButton_OnClick(object sender, RoutedEventArgs e)
		{
			MainPage.Singleton.GoToOutboundFriendSelection();
		}
	}
}
