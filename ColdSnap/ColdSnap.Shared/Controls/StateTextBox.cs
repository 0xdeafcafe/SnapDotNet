using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace ColdSnap.Controls
{
	public sealed class StateTextBlock : Button
	{
		public static DependencyProperty PrimaryTextDependencyProperty;
		public static DependencyProperty PrimaryTextVisibilityDependencyProperty;
		public static DependencyProperty PrimaryFontSizeDependencyProperty;
		public static DependencyProperty SecondaryTextDependencyProperty;
		public static DependencyProperty SecondaryTextVisibilityDependencyProperty;
		public static DependencyProperty SecondaryFontSizeDependencyProperty;
		public static DependencyProperty SecondaryMarginCorrectionDependencyProperty;

		public bool IsPrimaryContentVisible { get; private set; }

		/// <summary>
		/// 
		/// </summary>
		static StateTextBlock()
		{
			PrimaryTextDependencyProperty = DependencyProperty.Register("PrimaryText", typeof(string), typeof(StateTextBlock), new PropertyMetadata(""));
			PrimaryTextVisibilityDependencyProperty = DependencyProperty.Register("PrimaryTextVisibility", typeof(Visibility), typeof(StateTextBlock), new PropertyMetadata(Visibility.Visible));
			PrimaryFontSizeDependencyProperty = DependencyProperty.Register("PrimaryFontSize", typeof(int), typeof(StateTextBlock), new PropertyMetadata(40));

			SecondaryTextDependencyProperty = DependencyProperty.Register("SecondaryText", typeof(string), typeof(StateTextBlock), new PropertyMetadata(""));
			SecondaryTextVisibilityDependencyProperty = DependencyProperty.Register("SecondaryTextVisibility", typeof(Visibility), typeof(StateTextBlock), new PropertyMetadata(Visibility.Collapsed));
			SecondaryFontSizeDependencyProperty = DependencyProperty.Register("SecondaryFontSize", typeof(int), typeof(StateTextBlock), new PropertyMetadata(40));
			SecondaryMarginCorrectionDependencyProperty = DependencyProperty.Register("SecondaryMarginCorrection", typeof(Thickness), typeof(StateTextBlock), new PropertyMetadata(new Thickness(0,0,0,0)));
		}

		/// <summary>
		/// 
		/// </summary>
		public StateTextBlock()
		{
			IsPrimaryContentVisible = true;

			Click += (sender, args) =>
			{
				IsPrimaryContentVisible = !IsPrimaryContentVisible;

				if (IsPrimaryContentVisible)
				{
					PrimaryTextVisibility = Visibility.Visible;
					SecondaryTextVisibility = Visibility.Collapsed;
				}
				else
				{
					PrimaryTextVisibility = Visibility.Collapsed;
					SecondaryTextVisibility = Visibility.Visible;
				}
			};
		}

		/// <summary>
		/// 
		/// </summary>
		public string PrimaryText
		{
			get { return (string)GetValue(PrimaryTextDependencyProperty); }
			set { SetValue(PrimaryTextDependencyProperty, value); }
		}

		/// <summary>
		/// 
		/// </summary>
		public Visibility PrimaryTextVisibility
		{
			get { return (Visibility)GetValue(PrimaryTextVisibilityDependencyProperty); }
			set { SetValue(PrimaryTextVisibilityDependencyProperty, value); }
		}

		/// <summary>
		/// 
		/// </summary>
		public int PrimaryFontSize
		{
			get { return (int)GetValue(PrimaryFontSizeDependencyProperty); }
			set { SetValue(PrimaryFontSizeDependencyProperty, value); }
		}

		/// <summary>
		/// 
		/// </summary>
		public string SecondaryText
		{
			get { return (string)GetValue(SecondaryTextDependencyProperty); }
			set { SetValue(SecondaryTextDependencyProperty, value); }
		}

		/// <summary>
		/// 
		/// </summary>
		public Visibility SecondaryTextVisibility
		{
			get { return (Visibility)GetValue(SecondaryTextVisibilityDependencyProperty); }
			set { SetValue(SecondaryTextVisibilityDependencyProperty, value); }
		}

		/// <summary>
		/// 
		/// </summary>
		public int SecondaryFontSize
		{
			get { return (int)GetValue(SecondaryFontSizeDependencyProperty); }
			set { SetValue(SecondaryFontSizeDependencyProperty, value); }
		}

		/// <summary>
		/// 
		/// </summary>
		public Thickness SecondaryMarginCorrection
		{
			get { return (Thickness)GetValue(SecondaryMarginCorrectionDependencyProperty); }
			set { SetValue(SecondaryMarginCorrectionDependencyProperty, value); }
		}
	}
}