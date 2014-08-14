using System;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace ColdSnap.Controls
{
	public sealed partial class SnazzyHubSection : HubSection
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
	}
}
