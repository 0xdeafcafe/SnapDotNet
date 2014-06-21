using System;
using Windows.UI.Xaml;
using Snapchat.ViewModels.PageContents;

namespace Snapchat.Pages.PageContents
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class PreviewPageContent
	{
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
				else if (pageIndex == 0)
					ScrollViewer.ScrollToHorizontalOffset(NoOverlayGrid.ActualWidth * 4); // *4 goes to time - i should map these
			};
		}

		public void Reset()
		{
			DataContext = null;
			ImageMediaElement.Source = null;

			VisualStateManager.GoToState(this, "PendingMedia", true);
		}

		public void Load()
		{
			var context = DataContext as PreviewViewModel;
			if (context == null) return;

			ImageMediaElement.Source = context.WriteableBitmap;
		}
	}
}
