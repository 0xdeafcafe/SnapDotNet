using System;
using Windows.Phone.UI.Input;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using SnapDotNet.Apps.Pages.SignedIn;
using SnapDotNet.Apps.ViewModels;
using SnapDotNet.Core.Snapchat.Api.Exceptions;

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

			// TODO: remove this
			SignInModalGrid.Opacity = 0.0f;
			SignInModalGrid.Visibility = Visibility.Collapsed;

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

			//var user = new User
			//{
			//	DeviceIdent = App.DeviceIdent,
			//	AuthExpired = false,
			//	SnapchatAuthToken = new Random().Next(0xaaaaaaa, 0xfffffff).ToString("X8"),
			//	SnapchatUsername = "alexerax"
			//};

			//await App.MobileService.GetTable<User>().InsertAsync(user);
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
			ViewModel.ProgressModalVisibility = Visibility.Visible;

			StatusBar.GetForCurrentView().ProgressIndicator.Text = "Signing In...";
			await StatusBar.GetForCurrentView().ProgressIndicator.ShowAsync();

			try
			{
				await App.SnapChatManager.Endpoints.AuthenticateAsync(SignInUsernameTextBlock.Text,
					SignInPasswordTextBlock.Password);
			}
			catch (InvalidCredentialsException)
			{
				var dialog =
					new MessageDialog("The username and password combination you used to sign into snapchat is not correct.",
						"Invalid Username/Password");
				dialog.ShowAsync();
			}
			catch (InvalidHttpResponseException exception)
			{
				var dialog =
					new MessageDialog(String.Format("Unable to connect to snapchat. The server responded: \n {0}.", exception.Message),
						"Unable to connect to Snapchat");
				dialog.ShowAsync();
			}
			finally
			{
				StatusBar.GetForCurrentView().ProgressIndicator.Text = "";
				StatusBar.GetForCurrentView().ProgressIndicator.HideAsync();
				ViewModel.ProgressModalVisibility = Visibility.Collapsed;
			}

			if (App.SnapChatManager.Account == null || !App.SnapChatManager.Account.Logged)
				return;

			Frame.Navigate(typeof(MainPage));
		}

		private void OverrideButton_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
		{
			Frame.Navigate(typeof(MainPage));
		}
	}
}
