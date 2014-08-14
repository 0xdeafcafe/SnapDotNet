using ColdSnap.Common;
using ColdSnap.Dialogs;
using ColdSnap.ViewModels;
using SnapDotNet;
using System;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;

namespace ColdSnap.Pages
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class StartPage
	{
		private readonly NavigationHelper _navigationHelper;
		private readonly StartPageViewModel _viewModel = new StartPageViewModel();

		private LogInDialog _logInDialog;
		private SignUpDialog _signUpDialog;

		public StartPage()
		{
			InitializeComponent();
			DataContext = ViewModel;

			_navigationHelper = new NavigationHelper(this);
			_navigationHelper.LoadState += NavigationHelper_LoadState;
			_navigationHelper.SaveState += NavigationHelper_SaveState;

			Loaded += StartPage_Loaded;
		}

		/// <summary>
		/// Gets the <see cref="NavigationHelper"/> associated with this <see cref="Page"/>.
		/// </summary>
		public NavigationHelper NavigationHelper
		{
			get { return _navigationHelper; }
		}

		public StartPageViewModel ViewModel
		{
			get { return _viewModel; }
		}

		#region NavigationHelper registration

		/// <summary>
		/// The methods provided in this section are simply used to allow
		/// NavigationHelper to respond to the page's navigation methods.
		/// <para>
		/// Page specific logic should be placed in event handlers for the  
		/// <see cref="NavigationHelper.LoadState"/>
		/// and <see cref="NavigationHelper.SaveState"/>.
		/// The navigation parameter is available in the LoadState method 
		/// in addition to page state preserved during an earlier session.
		/// </para>
		/// </summary>
		/// <param name="e">Provides data for navigation methods and event
		/// handlers that cannot cancel the navigation request.</param>
		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			_navigationHelper.OnNavigatedTo(e);
		}

		protected override void OnNavigatedFrom(NavigationEventArgs e)
		{
			_navigationHelper.OnNavigatedFrom(e);
		}

		#endregion

		/// <summary>
		/// Populates the page with content passed during navigation.  Any saved state is also
		/// provided when recreating a page from a prior session.
		/// </summary>
		/// <param name="sender">
		/// The source of the event; typically <see cref="NavigationHelper"/>
		/// </param>
		/// <param name="e">Event data that provides both the navigation parameter passed to
		/// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested and
		/// a dictionary of state preserved by this page during an earlier
		/// session.  The state will be null the first time a page is visited.</param>
		private void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
		{
		}

		/// <summary>
		/// Preserves state associated with this page in case the application is suspended or the
		/// page is discarded from the navigation cache.  Values must conform to the serialization
		/// requirements of <see cref="SuspensionManager.SessionState"/>.
		/// </summary>
		/// <param name="sender">The source of the event; typically <see cref="NavigationHelper"/></param>
		/// <param name="e">Event data that provides an empty dictionary to be populated with
		/// serializable state.</param>
		private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
		{
		}

		private async void LogInButton_Tapped(object sender, TappedRoutedEventArgs e)
		{
			await _logInDialog.ShowAsync();
		}

		private async void SignUpButton_Tapped(object sender, TappedRoutedEventArgs e)
		{
			await _signUpDialog.ShowAsync();
		}

		private async void StartPage_Loaded(object sender, RoutedEventArgs e)
		{
			// Preload content dialogs
			await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
			{
				_logInDialog = new LogInDialog();
				_logInDialog.PrimaryButtonClick += LogInDialog_PrimaryButtonClick;

				_signUpDialog = new SignUpDialog();
			});
		}

		private async void LogInDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
		{
			var deferral = args.GetDeferral();
			sender.IsEnabled = sender.IsPrimaryButtonEnabled = sender.IsSecondaryButtonEnabled = false;

			// Display progress indicator.
			var progress = StatusBar.GetForCurrentView().ProgressIndicator;
			progress.Text = App.Strings.GetString("StatusLoggingIn");
			await progress.ShowAsync();

			string errorMessage = null;
			try
			{
				// Try to log in with the given credentials.
				var account = await ViewModel.LogInAsync(_logInDialog.Username, _logInDialog.Password);

				// Navigate to MainPage upon success.
				Window.Current.Navigate(typeof(MainPage), account);
			}
			catch (InvalidCredentialsException)
			{
				errorMessage = App.Strings.GetString("InvalidCredentialsException");
			}
			catch
			{
				errorMessage = App.Strings.GetString("UnknownLogInException");
			}

			// Display error message if there's one.
			if (errorMessage != null)
				await new MessageDialog(errorMessage).ShowAsync();

			// Clear the password field.
			_logInDialog.Password = String.Empty;

			// Hide progress indicator.
			progress.Text = String.Empty;
			progress.HideAsync();

			sender.IsEnabled = sender.IsPrimaryButtonEnabled = sender.IsSecondaryButtonEnabled = true;
			deferral.Complete();
		}
	}
}
