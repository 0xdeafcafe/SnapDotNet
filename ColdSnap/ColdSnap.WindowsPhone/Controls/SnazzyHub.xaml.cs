using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace ColdSnap.Controls
{
	public sealed partial class SnazzyHub
	{
		public static readonly DependencyProperty CurrentAccentColorProperty =
			DependencyProperty.Register("CurrentAccentColor", typeof (SolidColorBrush), typeof (SnazzyHub),
				new PropertyMetadata(new SolidColorBrush(Colors.Blue)));

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
			// Get current section.
			int sectionIndex = (int) ((sender as ScrollViewer).HorizontalOffset / ActualWidth);
			var currentSection = (Sections[sectionIndex] as SnazzyHubSection);
			CurrentAccentColor = new SolidColorBrush(currentSection.AccentColor);
		}

		private void SetHeaderColor()
		{
			CurrentAccentColor = new SolidColorBrush(CurrentSection.AccentColor);
		}
	}
}
