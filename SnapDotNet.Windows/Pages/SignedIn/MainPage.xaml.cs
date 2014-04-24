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
using SnapDotNet.Apps.ViewModels.SignedIn;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.ViewManagement;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace SnapDotNet.Apps.Pages.SignedIn
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	[RequiresAuthentication]
	public sealed partial class MainPage : Page
	{
		public MainPage()
		{
			DataContext = new MainViewModel();
			InitializeComponent();
			SizeChanged += (sender, e) =>
			{
				if (e.NewSize.Width <= (int) Resources["MinimalViewMaxWidth"])
					VisualStateManager.GoToState(this, "MinimalLayout", true);
				else if (e.NewSize.Width < e.NewSize.Height)
					VisualStateManager.GoToState(this, "PortraitLayout", true);
				else
					VisualStateManager.GoToState(this, "DefaultLayout", true);
			};

#if !DEBUG
			Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().IsScreenCaptureEnabled = false;
#endif

			/*BottomAppBar.Closed += delegate
			{
				
			};*/
		}

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			SettingsPane.GetForCurrentView().CommandsRequested += OnSettingsCommandsRequested;
			BackgroundAmbienceStoryboard.RepeatBehavior = new RepeatBehavior { Type = RepeatBehaviorType.Forever };
			BackgroundAmbienceStoryboard.Begin();
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
				(DataContext as MainViewModel).SignOutCommand.Execute(null);
			}));

			args.Request.ApplicationCommands.Add(new SettingsCommand("privacy_policy", "Privacy policy", delegate
			{
				// TODO: Open privacy policy
			}));
		}

		/*private void OnBottomAppBarHintEntered(object sender, PointerRoutedEventArgs e)
		{
			(sender as Grid).Background = new SolidColorBrush(Color.FromArgb(0xFF, 0xD6, 0x91, 0x11));
		}

		private void OnBottomAppBarHintExited(object sender, PointerRoutedEventArgs e)
		{
			(sender as Grid).Background = new SolidColorBrush(Color.FromArgb(0xFF, 0xFF, 0xA5, 0x00));
		}

		private void OnBottomAppBarHintTapped(object sender, TappedRoutedEventArgs e)
		{
			BottomAppBar.IsOpen = true;
		}*/

		private void OnSettingsTapped(object sender, RoutedEventArgs e)
		{
			SettingsPane.Show();
		}
	}
}
