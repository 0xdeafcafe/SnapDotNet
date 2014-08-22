using System;
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

		private ScrollViewer _scrollViewer;

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

		public int CurrentSectionIndex
		{
			get
			{
				if (_scrollViewer == null)
					return 0;

				return (int) Math.Round((_scrollViewer.HorizontalOffset / ActualWidth));
			}
		}

		public SnazzyHubSection CurrentSection
		{
			get { return Sections[CurrentSectionIndex] as SnazzyHubSection; }
		}

		public void SetSectionIndex(int index)
		{
			var timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(10) };
			timer.Tick += delegate
			{
				_scrollViewer.HorizontalSnapPointsType = SnapPointsType.None;
				if (!_scrollViewer.ChangeView(ActualWidth * index, null, null, true)) return;
				timer.Stop();
				_scrollViewer.HorizontalSnapPointsType = SnapPointsType.MandatorySingle;
			};
			timer.Start();
		}

		private void SnazzyScrollViewer_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
		{
			_scrollViewer = sender as ScrollViewer;

			// Get current section.
			CurrentAccentColor = new SolidColorBrush(CurrentSection.AccentColor);
		}

		private void SetHeaderColor()
		{
			CurrentAccentColor = new SolidColorBrush(CurrentSection.AccentColor);
		}

		private void SnazzyScrollViewer_Loaded(object sender, RoutedEventArgs e)
		{
			_scrollViewer = sender as ScrollViewer;
		}
	}
}
