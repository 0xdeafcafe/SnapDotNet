using Windows.Phone.UI.Input;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using SnapDotNet.Apps.ViewModels;

namespace SnapDotNet.Apps.Pages
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class StartPage
	{
		public StartViewModel ViewModel { get; private set; }

		public StartPage()
		{
			InitializeComponent();

			DataContext = ViewModel = new StartViewModel();
			HardwareButtons.BackPressed += HardwareButtonsOnBackPressed;
		}

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			StatusBar.GetForCurrentView().BackgroundColor = new Color { A = 0x00, R = 0x00, G = 0x00, B = 0x00,  };
			ApplicationView.GetForCurrentView().SetDesiredBoundsMode(ApplicationViewBoundsMode.UseCoreWindow);
		}

		private void ShowSignInButton_Click(object sender, RoutedEventArgs e)
		{
			var storyboard = (Storyboard) Resources["SignInModalRevealStoryboard"];
			if (storyboard == null) return;
			storyboard.Begin();
			ViewModel.OpenSignInPageCommand.Execute(null);
		}

		private void HardwareButtonsOnBackPressed(object sender, BackPressedEventArgs backPressedEventArgs)
		{
			if (ViewModel.IsSignInPageVisible)
			{
				var storyboard = (Storyboard)Resources["SignInModalHideStoryboard"];
				if (storyboard == null) return;
				storyboard.Begin();
				ViewModel.GoBackToStartCommand.Execute(null);
				backPressedEventArgs.Handled = true;
			}

			if (ViewModel.IsRegisterPageVisible)
			{
				backPressedEventArgs.Handled = true;
			}
		}

		private async void SignInButton_OnClick(object sender, RoutedEventArgs e)
		{
			//StatusBar.GetForCurrentView().ProgressIndicator.Text = "Signing in...";
			//await StatusBar.GetForCurrentView().ProgressIndicator.ShowAsync();

			ViewModel.SignInCommand.Execute(null);

			//StatusBar.GetForCurrentView().ProgressIndicator.HideAsync();
		}
	}
}
