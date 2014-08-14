using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace ColdSnap.Controls
{
	public sealed partial class SnazzyHub : Hub
	{
		public SnazzyHub()
		{
			InitializeComponent();
			Loaded += delegate { UpdateHeader(); };
		}

		private void SnazzyScrollViewer_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
		{

		}

		private void SnazzyScrollViewer_ViewChanging(object sender, ScrollViewerViewChangingEventArgs e)
		{
			
		}

		private void UpdateHeader()
		{

		}
	}
}
