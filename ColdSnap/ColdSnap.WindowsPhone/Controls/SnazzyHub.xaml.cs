using System;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace ColdSnap.Controls
{
	public sealed partial class SnazzyHub : Hub
	{
		public static readonly DependencyProperty CurrentAccentColorProperty = DependencyProperty.Register("CurrentAccentColor", typeof(SolidColorBrush), typeof(SnazzyHub), new PropertyMetadata(new SolidColorBrush(Colors.Blue)));

		public SnazzyHub()
		{
			InitializeComponent();
			Loaded += delegate
			{
				SetHeaderColor();
			};
		}

		public SolidColorBrush CurrentAccentColor
		{
			get { return (SolidColorBrush) GetValue(CurrentAccentColorProperty); }
			set { SetValue(CurrentAccentColorProperty, value); }
		}

		public SnazzyHubSection CurrentSection
		{
			get { return SectionsInView[SectionsInView.Count > 1 ? 1 : 0] as SnazzyHubSection; }
		}

		private void SnazzyScrollViewer_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
		{
			if (!e.IsIntermediate)
				SetHeaderColor();
		}

		private void SnazzyScrollViewer_ViewChanging(object sender, ScrollViewerViewChangingEventArgs e)
		{
			var scrollViewer = sender as ScrollViewer;
			
			// TODO: Set header color to 
		}

		private void SetHeaderColor()
		{
			CurrentAccentColor = new SolidColorBrush(CurrentSection.AccentColor);
		}
	}
}
