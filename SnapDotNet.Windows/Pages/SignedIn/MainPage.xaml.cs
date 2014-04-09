using System;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.ApplicationSettings;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using SnapDotNet.Apps.Attributes;
using SnapDotNet.Apps.ViewModels;
using WinRTXamlToolkit.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace SnapDotNet.Apps.Pages.SignedIn
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	[RequiresAuthentication]
	public sealed partial class MainPage : Page
	{
		public static DependencyProperty AppTitleContentProperty = DependencyProperty.Register("AppTitleContentppTitleContent", typeof(object), typeof(MainPage), new PropertyMetadata(null));

		public object AppTitleContent
		{
			get { return ((base.GetValue(MainPage.AppTitleContentProperty))); }
			set { base.SetValue(MainPage.AppTitleContentProperty, value); }
		}

		public MainPage()
		{
			DataContext = new MainViewModel();
			this.InitializeComponent();
			this.SizeChanged += (object sender, SizeChangedEventArgs e) =>
			{
				if (e.NewSize.Width <= (int) Resources["MinimalViewMaxWidth"])
					VisualStateManager.GoToState(this, "MinimalLayout", true);
				else if (e.NewSize.Width < e.NewSize.Height)
					VisualStateManager.GoToState(this, "PortraitLayout", true);
				else
					VisualStateManager.GoToState(this, "DefaultLayout", true);
			};

			// won't work at design time
			var title = new CascadingTextBlock
			{
				Text = "snapchat",
				Foreground = new SolidColorBrush(Colors.White),
				CascadeInterval = TimeSpan.FromMilliseconds(150),
				CascadeInDuration = TimeSpan.FromMilliseconds(75),
				StartDelay = 700,
			};
			title.CascadeCompleted += async delegate
			{
				if (title.CascadeIn)
				{
					title.CascadeIn = false;
					title.CascadeOut = true;
					await Task.Delay(10000);
				}
				else
				{
					title.CascadeOut = false;
					title.CascadeIn = true;
				}

				await title.BeginCascadingTransitionAsync();
			};
			AppTitleContent = title;
		}

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			SettingsPane.GetForCurrentView().CommandsRequested += OnSettingsCommandsRequested;
			base.OnNavigatedTo(e);
		}

		protected override void OnNavigatedFrom(NavigationEventArgs e)
		{
			SettingsPane.GetForCurrentView().CommandsRequested -= OnSettingsCommandsRequested;
			base.OnNavigatedFrom(e);
		}

		void OnSettingsCommandsRequested(SettingsPane sender, SettingsPaneCommandsRequestedEventArgs args)
		{
			args.Request.ApplicationCommands.Add(new SettingsCommand("account", "Account", delegate
			{
				// TODO: Open account settings flyout
			}));

			args.Request.ApplicationCommands.Add(new SettingsCommand("sign_out", "Sign out", delegate
			{
				// TODO: Sign out
				App.CurrentFrame.Navigate(typeof(StartPage));
			}));

			args.Request.ApplicationCommands.Add(new SettingsCommand("privacy_policy", "Privacy policy", delegate
			{
				// TODO: Open privacy policy
			}));
		}

		private void OnSnapSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			// This is annoying as hell
			/*(if (e.AddedItems.Count > 0)
			{
				this.BottomAppBar.IsOpen = true;
				this.BottomAppBar.IsSticky = true;
			}
			else
			{
				this.BottomAppBar.IsOpen = false;
				this.BottomAppBar.IsSticky = false;
			}*/
		}

		private void OnBottomAppBarHintEntered(object sender, PointerRoutedEventArgs e)
		{
			(sender as Grid).Background = new SolidColorBrush(Color.FromArgb(0xFF, 0xD6, 0x91, 0x11));
		}

		private void OnBottomAppBarHintExited(object sender, PointerRoutedEventArgs e)
		{
			(sender as Grid).Background = new SolidColorBrush(Color.FromArgb(0xFF, 0xFF, 0xA5, 0x00));
		}

		private void OnBottomAppBarHintTapped(object sender, TappedRoutedEventArgs e)
		{
			this.BottomAppBar.IsOpen = true;
		}

		private void OnAccountBoxTapped(object sender, TappedRoutedEventArgs e)
		{
			// TODO: Navigate to accounts page
		}
	}
}
