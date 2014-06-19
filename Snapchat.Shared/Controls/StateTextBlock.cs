using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Snapchat.Controls
{
	public sealed class StateTextBlock : Button
	{
		public static DependencyProperty PrimaryTextDependencyProperty;
		public static DependencyProperty PrimaryTextVisibilityDependencyProperty;
		public static DependencyProperty SecondaryTextDependencyProperty;
		public static DependencyProperty SecondaryTextVisibilityDependencyProperty;

		public bool IsPrimaryContentVisible { get; private set; }

		/// <summary>
		/// 
		/// </summary>
		static StateTextBlock()
		{
			PrimaryTextDependencyProperty = DependencyProperty.Register("PrimaryText", typeof(String), typeof(StateTextBlock), new PropertyMetadata(""));
			PrimaryTextVisibilityDependencyProperty = DependencyProperty.Register("PrimaryTextVisibility", typeof(Visibility), typeof(StateTextBlock), new PropertyMetadata(Visibility.Visible));

			SecondaryTextDependencyProperty = DependencyProperty.Register("SecondaryText", typeof(String), typeof(StateTextBlock), new PropertyMetadata(""));
			SecondaryTextVisibilityDependencyProperty = DependencyProperty.Register("SecondaryTextVisibility", typeof(Visibility), typeof(StateTextBlock), new PropertyMetadata(Visibility.Collapsed));
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
		public String PrimaryText
		{
			get { return (String) GetValue(PrimaryTextDependencyProperty); }
			set { SetValue(PrimaryTextDependencyProperty, value); }
		}

		/// <summary>
		/// 
		/// </summary>
		public Visibility PrimaryTextVisibility
		{
			get { return (Visibility) GetValue(PrimaryTextVisibilityDependencyProperty); }
			set { SetValue(PrimaryTextVisibilityDependencyProperty, value); }
		}

		/// <summary>
		/// 
		/// </summary>
		public String SecondaryText
		{
			get { return (String) GetValue(SecondaryTextDependencyProperty); }
			set { SetValue(SecondaryTextDependencyProperty, value); }
		}

		/// <summary>
		/// 
		/// </summary>
		public Visibility SecondaryTextVisibility
		{
			get { return (Visibility) GetValue(SecondaryTextVisibilityDependencyProperty); }
			set { SetValue(SecondaryTextVisibilityDependencyProperty, value); }
		}
	}
}
