using System;
using System.Collections.ObjectModel;
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
				new PropertyMetadata(new SolidColorBrush(Colors.CornflowerBlue)));

		public static DependencyProperty GlobalCommandsProperty =
			DependencyProperty.Register("GlobalCommands", typeof(ObservableCollection<ICommandBarElement>), typeof(SnazzyHub),
			new PropertyMetadata(new ObservableCollection<ICommandBarElement>()));

		private Page _parentPage;
		private ScrollViewer _scrollViewer;

		public SnazzyHub()
		{
			InitializeComponent();
			Loaded += delegate
			{
				CurrentAccentColor = new SolidColorBrush(CurrentSection.AccentColor);

				// Find a reference to the Page that owns this hub.
				var parent = Parent as FrameworkElement;
				while (parent != null)
				{
					if (parent is Page)
					{
						_parentPage = parent as Page;
						break;
					}
					parent = parent.Parent as FrameworkElement;
				}

				UpdateBottomAppBar();
			};
		}

		/// <summary>
		/// Gets or sets the global app bar commands.
		/// </summary>
		public ObservableCollection<ICommandBarElement> GlobalCommands
		{
			get { return GetValue(GlobalCommandsProperty) as ObservableCollection<ICommandBarElement>; }
			set { SetValue(GlobalCommandsProperty, value); }
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

		public CommandBar BottomAppBar
		{
			get { return _parentPage.BottomAppBar as CommandBar; }
			set { _parentPage.BottomAppBar = value; }
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
			CurrentAccentColor = new SolidColorBrush(CurrentSection.AccentColor);
			UpdateBottomAppBar();
		}

		private void SnazzyScrollViewer_ViewChanging(object sender, ScrollViewerViewChangingEventArgs e)
		{
			if (BottomAppBar != null)
				BottomAppBar.IsOpen = false;
		}

		private void UpdateBottomAppBar()
		{
			BottomAppBar.Background = CurrentAccentColor;
			BottomAppBar.Foreground = new SolidColorBrush(Colors.White);
			BottomAppBar.ClosedDisplayMode = CurrentSection.AppBarClosedDisplayMode;
		}

		private void SnazzyScrollViewer_Loaded(object sender, RoutedEventArgs e)
		{
			_scrollViewer = sender as ScrollViewer;
		}
	}
}
