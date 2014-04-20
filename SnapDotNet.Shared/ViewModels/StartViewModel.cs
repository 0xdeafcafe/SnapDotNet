using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using SnapDotNet.Apps.Common;
using System.Windows.Input;
using Windows.UI.Xaml.Controls;
using System;
using SnapDotNet.Apps.Pages.SignedIn;
using SnapDotNet.Core.Snapchat.Api.Exceptions;
using Windows.UI.Popups;
using SnapDotNet.Core.Snapchat.Models;
using WinRTXamlToolkit.Controls;
using Snap = SnapDotNet.Core.Snapchat.Models.Snap;
using SnapDotNet.Apps.Pages;
#if WINDOWS_PHONE_APP
using SnapDotNet.Core.Miscellaneous.Models.Atlas;
using Windows.UI.ViewManagement;
#endif

namespace SnapDotNet.Apps.ViewModels
{
	public sealed class StartViewModel
		: NotifyPropertyChangedBase
	{
		public StartViewModel()
		{
			IsStartPageVisible = true;

			#region Commands

			OpenSignInPageCommand = new RelayCommand(
				() =>
				{
					IsSignInPageVisible = true;
					IsStartPageVisible = false;
				},
				() => IsStartPageVisible);

			OpenRegisterPageCommand = new RelayCommand(
				() =>
				{
					IsRegisterPageVisible = true;
					IsStartPageVisible = false;
				},
				() => IsStartPageVisible);

			GoBackToStartCommand = new RelayCommand(() =>
			{
				IsSignInPageVisible = false;
				IsRegisterPageVisible = false;
				IsStartPageVisible = true;
			});

			SignInCommand = new RelayCommand<Page>(SignIn);
			RegisterCommand = new RelayCommand<Page>(Register);

			#endregion
		}

		/// <summary>
		/// Gets the command to open the sign in form.
		/// </summary>
		public ICommand OpenSignInPageCommand
		{
			get { return _openSignInPageCommand; }
			private set { SetField(ref _openSignInPageCommand, value); }
		}
		private ICommand _openSignInPageCommand;

		/// <summary>
		/// Gets the command to open the registration form.
		/// </summary>
		public ICommand OpenRegisterPageCommand
		{
			get { return _openRegisterPageCommand; }
			private set { SetField(ref _openRegisterPageCommand, value); }
		}
		private ICommand _openRegisterPageCommand;

		/// <summary>
		/// Gets the command to go back to the start page.
		/// </summary>
		public ICommand GoBackToStartCommand
		{
			get { return _goBackToStartCommand; }
			private set { SetField(ref _goBackToStartCommand, value); }
		}
		private ICommand _goBackToStartCommand;

		/// <summary>
		/// Gets the command to sign in.
		/// </summary>
		public ICommand SignInCommand
		{
			get { return _signInCommand; }
			private set { SetField(ref _signInCommand, value); }
		}
		private ICommand _signInCommand;

		/// <summary>
		/// Gets the command to register.
		/// </summary>
		public ICommand RegisterCommand
		{
			get { return _registerCommand; }
			private set { SetField(ref _registerCommand, value); }
		}
		private ICommand _registerCommand;

		/// <summary>
		/// Gets whether the sign in form should be visible.
		/// </summary>
		public bool IsSignInPageVisible
		{
			get { return _isSignInPageVisible; }
			private set
			{
				SetField(ref _isSignInPageVisible, value);
			}
		}
		private bool _isSignInPageVisible;

		/// <summary>
		/// Gets whether the registration form should be visible.
		/// </summary>
		public bool IsRegisterPageVisible
		{
			get { return _isRegisterPageVisible; }
			private set { SetField(ref _isRegisterPageVisible, value); }
		}
		private bool _isRegisterPageVisible;

		public bool ProgressModalIsVisible
		{
			get { return _progressModalIsVisible; }
			set { SetField(ref _progressModalIsVisible, value); }
		}
		private bool _progressModalIsVisible;

		public Visibility ProgressModalVisibility
		{
			get { return _progressModalVisibility; }
			set { SetField(ref _progressModalVisibility, value); }
		}
		private Visibility _progressModalVisibility = Visibility.Collapsed;

		/// <summary>
		/// Gets whether the starting menu should be visible.
		/// </summary>
		public bool IsStartPageVisible
		{
			get { return _isStartPageVisible; }
			private set { SetField(ref _isStartPageVisible, value); }
		}
		private bool _isStartPageVisible;

		/// <summary>
		/// Gets or sets the current username.
		/// </summary>
		public string CurrentUsername
		{
			get { return _currentUsername; }
			set { SetField(ref _currentUsername, value); }
		}
		private string _currentUsername;

		/// <summary>
		/// Gets or sets the current password.
		/// </summary>
		public string CurrentPassword
		{
			get { return _currentPassword; }
			set { SetField(ref _currentPassword, value); }
		}
		private string _currentPassword;

		/// <summary>
		/// Gets or sets the current email.
		/// </summary>
		public string CurrentEmail
		{
			get { return _currentEmail; }
			set { SetField(ref _currentEmail, value); }
		}
		private string _currentEmail;

		/// <summary>
		/// Gets or sets the desired password.
		/// </summary>
		public string DesiredPassword
		{
			get { return _desiredPassword; }
			set { SetField(ref _desiredPassword, value); }
		}
		private string _desiredPassword;

		/// <summary>
		/// Gets or sets the current birthday.
		/// </summary>
		public DateTimeOffset CurrentBirthday
		{
			get { return _currentBirthday; }
			set { SetField(ref _currentBirthday, value); }
		}
		private DateTimeOffset _currentBirthday;


		/// <summary>
		/// Attempts to sign the user into Snapchat.
		/// </summary>
		private async void SignIn(Page nextPage)
		{
//			Uncomment this to force login with a foreign Auth Token
//
//			App.SnapChatManager.Account = new Account
//			{
//				Friends = new ObservableCollection<Friend>(),
//				AddedFriends = new ObservableCollection<AddedFriend>(),
//				BestFriends = new ObservableCollection<string>(),
//				Snaps = new ObservableCollection<Snap>(),
//				AuthToken = "",
//				Username = "alexerax"
//			};
//			App.SnapChatManager.AuthToken = "";
//			App.SnapChatManager.Username = "alexerax";
//			await App.SnapChatManager.UpdateAllAsync(() => { }, App.Settings);
//			App.CurrentFrame.Navigate((typeof (MainPage)));
//#if WINDOWS_PHONE_APP
//			await
//					App.MobileService.GetTable<User>()
//						.InsertAsync(new User
//						{
//							AuthExpired = false,
//							NewUser = true,
//							DeviceIdent = App.DeviceIdent,
//							SnapchatAuthToken = App.SnapChatManager.AuthToken,
//							SnapchatUsername = App.SnapChatManager.Username
//						});
//#endif
//			return;

			try
			{
				if (string.IsNullOrEmpty(CurrentUsername) || string.IsNullOrEmpty(CurrentPassword))
				{
					var dialog =
						new MessageDialog("The username and password combination you used to sign into snapchat is not correct.",
							"Invalid Username/Password");
					await dialog.ShowAsync();
					return;
				}

#if WINDOWS_PHONE_APP
				// Tell UI we're Signing In
				StatusBar.GetForCurrentView().ProgressIndicator.Text = "Signing in...";
				await StatusBar.GetForCurrentView().ProgressIndicator.ShowAsync();
#endif
				ProgressModalVisibility = Visibility.Visible;
				ProgressModalIsVisible = true;

				// Try and log into SnapChat
				await App.SnapChatManager.Endpoints.AuthenticateAsync(CurrentUsername, CurrentPassword);
				try
				{
					await App.SnapChatManager.UpdateAllAsync(() => { }, App.Settings);
				}
				catch (InvalidHttpResponseException exception)
				{
					if (exception.Message == "Unauthorized")
					{
						var dialog = new MessageDialog("Your sign in infomation has expired. Please sign in again.", "You are Unauthorized");
						dialog.ShowAsync();
					}
				}

#if WINDOWS_PHONE_APP
				// Register device for Push Notifications
				await
					App.MobileService.GetTable<User>()
						.InsertAsync(new User
						{
							AuthExpired = false,
							NewUser = true,
							DeviceIdent = App.DeviceIdent,
							SnapchatAuthToken = App.SnapChatManager.AuthToken,
							SnapchatUsername = App.SnapChatManager.Username
						});
#endif
			}
			catch (InvalidCredentialsException)
			{
				var dialog =
					new MessageDialog("The username and password combination you used to sign into Snapchat is not correct.",
						"Invalid Username/Password");
				dialog.ShowAsync();
			}
			catch (InvalidHttpResponseException exception)
			{
				var dialog =
					new MessageDialog(String.Format("Unable to connect to Snapchat. The server responded: \n {0}.", exception.Message),
						"Unable to connect to Snapchat");
				dialog.ShowAsync();
			}
			finally
			{
				// Tell UI we're not Signing In no mo'
#if WINDOWS_PHONE_APP
				StatusBar.GetForCurrentView().ProgressIndicator.Text = String.Empty;
				StatusBar.GetForCurrentView().ProgressIndicator.HideAsync();
#endif
				ProgressModalVisibility = Visibility.Collapsed;
				ProgressModalIsVisible = false;
			}

			if ( App.SnapChatManager.Account == null || 
				!App.SnapChatManager.Account.Logged ||
				!App.SnapChatManager.IsAuthenticated())
			{
				App.CurrentFrame.Navigate(typeof(StartPage), "removeBackStack");
				return;
			}

			App.CurrentFrame.Navigate(nextPage == null ? typeof(MainPage) : nextPage.GetType(), "removeBackStack");
		}

		/// <summary>
		/// Attempts to create a new account.
		/// </summary>
		private void Register(Page nextPage)
		{
			// TODO: API stuff
			App.CurrentFrame.Navigate(nextPage.GetType());
		}
	}
}
