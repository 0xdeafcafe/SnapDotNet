using Windows.UI.Xaml;
using SnapDotNet.Apps.Common;
using System.Windows.Input;
using Windows.UI.Xaml.Controls;
using System;
using SnapDotNet.Core.Snapchat.Api.Exceptions;
using Windows.UI.Popups;
using System.Threading.Tasks;
using SnapDotNet.Core.Atlas;

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
			// Do nothing if username or password isn't filled in.
			// Maybe show dialog instead?
			if (string.IsNullOrEmpty(CurrentUsername) || string.IsNullOrEmpty(CurrentPassword))
				return;

			ProgressModalVisibility = Visibility.Visible;

			try
			{
				// Try and log into SnapChat
				await App.SnapChatManager.Endpoints.AuthenticateAsync(CurrentUsername, CurrentPassword);

#if WINDOWS_PHONE_APP
				// Register device for Push Notifications
				await
					App.MobileService.GetTable<User>()
						.InsertAsync(new User
						{
							AuthExpired = false,
							DeviceIdent = App.DeviceIdent,
							SnapchatAuthToken = App.SnapChatManager.AuthToken,
							SnapchatUsername = App.SnapChatManager.Username
						});
#endif
			}
			catch (InvalidCredentialsException)
			{
				var dialog =
					new MessageDialog("The username and password combination you used to sign into snapchat is not correct.",
						"Invalid Username/Password");
				var showDialogTask = dialog.ShowAsync();
			}
			catch (InvalidHttpResponseException exception)
			{
				var dialog =
					new MessageDialog(String.Format("Unable to connect to snapchat. The server responded: \n {0}.", exception.Message),
						"Unable to connect to Snapchat");
				var showDialogTask = dialog.ShowAsync();
			}
			finally
			{
				ProgressModalVisibility = Visibility.Collapsed;
			}

			if (nextPage != null)
			{
				if (App.SnapChatManager.Account == null || !App.SnapChatManager.Account.Logged)
					return;

				App.CurrentFrame.Navigate(nextPage.GetType());
			}
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
