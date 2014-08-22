using ColdSnap.Common;
using System;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace ColdSnap.Controls
{
	public partial class SnazzyHubSection
	{
		public static DependencyProperty AccentColorProperty = DependencyProperty.Register("AccentColor", typeof(Color), typeof(SnazzyHubSection), null);

		public SnazzyHubSection()
		{
			Width = Window.Current.Bounds.Width;
			InitializeComponent();
		}

		/// <summary>
		/// Gets or sets the accent color of this section.
		/// </summary>
		public Color AccentColor
		{
			get { return (Color) GetValue(AccentColorProperty); }
			set { SetValue(AccentColorProperty, value); }
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
