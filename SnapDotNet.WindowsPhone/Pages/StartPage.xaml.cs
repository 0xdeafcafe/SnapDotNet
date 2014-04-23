using Windows.Phone.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Animation;
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

			DataContext = ViewModel = new StartViewModel {StartPage = this};
			HardwareButtons.BackPressed += HardwareButtonsOnBackPressed;
		}

		private void ShowSignInButton_Click(object sender, RoutedEventArgs e)
		{
			var storyboard = (Storyboard) Resources["SignInModalRevealStoryboard"];
			if (storyboard == null) return;
			storyboard.Begin();
			ViewModel.OpenSignInPageCommand.Execute(null);
		}

		private void CreateAccountButtonButton_Click(object sender, RoutedEventArgs e)
		{
			var storyboard = (Storyboard)Resources["RegistrationModalRevealStoryboard"];
			if (storyboard == null) return;
			storyboard.Begin();
			ViewModel.OpenRegisterPageCommand.Execute(null);
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
				var storyboard = (Storyboard)Resources["RegistrationModalHideStoryboard"];
				if (storyboard == null) return;
				storyboard.Begin();
				ViewModel.GoBackToStartCommand.Execute(null);
				backPressedEventArgs.Handled = true;
			}

			if (ViewModel.IsCaptchaPageVisible)
			{
				var storyboard = (Storyboard)Resources["CaptchaModalHideStoryboard"];
				if (storyboard == null) return;
				storyboard.Begin();
				ViewModel.GoBackToStartCommand.Execute(null);
				backPressedEventArgs.Handled = true;
			}
		}

		private void SignInButton_OnClick(object sender, RoutedEventArgs e)
		{
			ViewModel.SignInCommand.Execute(null);
		}

		private void ContinueRegistrationButton_OnClick(object sender, RoutedEventArgs e)
		{
			ViewModel.RegisterPhase1Command.Execute(null);
		}

		private void SubmitCaptchaButton_OnClick(object sender, RoutedEventArgs e)
		{
			ViewModel.RegisterPhase2Command.Execute(null);
		}

		public void RevealCaptchaPage()
		{
			var storyboard = (Storyboard)Resources["CaptchaModalRevealStoryboard"];
			if (storyboard == null) return;
			storyboard.Begin();
			ViewModel.OpenCaptchaPageCommand.Execute(null);
		}
	}
}
