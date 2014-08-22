using ColdSnap.Common;
using System;
using System.Collections.ObjectModel;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace ColdSnap.Controls
{
	public partial class SnazzyHubSection
	{
		public static DependencyProperty AccentColorProperty = DependencyProperty.Register("AccentColor", typeof(Color), typeof(SnazzyHubSection), null);
		public static DependencyProperty PrimaryCommandsProperty = DependencyProperty.Register("PrimaryCommands", typeof(ObservableCollection<ICommandBarElement>), typeof(SnazzyHubSection), new PropertyMetadata(new ObservableCollection<ICommandBarElement>()));
		public static DependencyProperty SecondaryCommandsProperty = DependencyProperty.Register("SecondaryCommands", typeof(ObservableCollection<ICommandBarElement>), typeof(SnazzyHubSection), new PropertyMetadata(new ObservableCollection<ICommandBarElement>()));
		public static DependencyProperty AppBarClosedDisplayModeProperty = DependencyProperty.Register("AppBarClosedDisplayMode", typeof(AppBarClosedDisplayMode), typeof(SnazzyHubSection), new PropertyMetadata(AppBarClosedDisplayMode.Minimal));

		public SnazzyHubSection()
		{
			Width = Window.Current.Bounds.Width;
			InitializeComponent();
		}

		/// <summary>
		/// Gets or sets the accent color for this section.
		/// </summary>
		public Color AccentColor
		{
			get { return (Color) GetValue(AccentColorProperty); }
			set { SetValue(AccentColorProperty, value); }
		}

		/// <summary>
		/// Gets or sets the primary app bar commands for this section.
		/// </summary>
		public ObservableCollection<ICommandBarElement> PrimaryCommands
		{
			get { return GetValue(PrimaryCommandsProperty) as ObservableCollection<ICommandBarElement>; }
			set { SetValue(PrimaryCommandsProperty, value); }
		}

		/// <summary>
		/// Gets or sets the secondary app bar commands for this section.
		/// </summary>
		public ObservableCollection<ICommandBarElement> SecondaryCommands
		{
			get { return GetValue(SecondaryCommandsProperty) as ObservableCollection<ICommandBarElement>; }
			set { SetValue(SecondaryCommandsProperty, value); }
		}

		public AppBarClosedDisplayMode AppBarClosedDisplayMode
		{
			get { return (AppBarClosedDisplayMode) GetValue(AppBarClosedDisplayModeProperty); }
			set { SetValue(AppBarClosedDisplayModeProperty, value); }
		}

		public virtual void LoadState(LoadStateEventArgs e) { }
		public virtual void SaveState(SaveStateEventArgs e) { }
	}

	public sealed class HeaderToHeaderTemplateHeightConverter
		: IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			return value == null ? 0 : 80;
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotImplementedException();
		}
	}
}
