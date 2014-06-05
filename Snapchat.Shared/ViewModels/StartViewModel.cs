using System;
using System.Diagnostics;
using System.Windows.Input;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Snapchat.Common;
using Snapchat.Pages;
using SnapDotNet.Core.Miscellaneous.Models.Atlas;
using SnapDotNet.Core.Snapchat.Api.Exceptions;

namespace Snapchat.ViewModels
{
	public sealed class StartViewModel
		: BaseViewModel
	{
		public StartViewModel()
		{
			LogInCommand = new RelayCommand(LogInAsync);
		}

		/// <summary>
		/// Gets or sets the current username entered by the user.
		/// </summary>
		public string CurrentUsername
		{
			get { return _currentUsername; }
			set { TryChangeValue(ref _currentUsername, value); }
		}
		private string _currentUsername;

		/// <summary>
		/// Gets or sets the current password entered by the user.
		/// </summary>
		public string CurrentPassword
		{
			get { return _currentPassword; }
			set { TryChangeValue(ref _currentPassword, value); }
		}
		private string _currentPassword;

		/// <summary>
		/// Gets the command to log into Snapchat.
		/// </summary>
		public ICommand LogInCommand
		{
			get { return _logInCommand; }
			private set { TryChangeValue(ref _logInCommand, value); }
		}
		private ICommand _logInCommand;

		/// <summary>
		/// Attempts to log into Snapchat using the username and password stored in <see cref="CurrentUsername"/>
		/// and <see cref="CurrentPassword"/> respectively.
		/// </summary>
		private async void LogInAsync()
		{
#if DEBUG
			if (string.IsNullOrWhiteSpace(CurrentUsername) ||
				string.IsNullOrWhiteSpace(CurrentPassword))
			{
				// Show progress indicator.
				var progIndicator = StatusBar.GetForCurrentView().ProgressIndicator;
				progIndicator.Text = App.Strings.GetString("StatusLoggingIn");
				await progIndicator.ShowAsync();

				// set le auth token here
				App.SnapchatManager.UpdateAuthToken("5ee95824-c242-4716-b4e2-d50dc4609ff9");
				App.SnapchatManager.UpdateUsername("alexerax");
				await App.SnapchatManager.Endpoints.GetAllUpdatesAsync();

				// Hide progress indicator.
				progIndicator.Text = String.Empty;
				progIndicator.HideAsync();

				// Navigate to the main page.
				var frame = Window.Current.Content as Frame;
				if (frame != null) frame.Navigate(typeof(MainPage));

				return;
			}
#endif

			// Username or password cannot be null.
			if (string.IsNullOrWhiteSpace(CurrentUsername) ||
				string.IsNullOrWhiteSpace(CurrentPassword))
			{
				await (new MessageDialog(App.Strings.GetString("InvalidCredentialsExceptionFriendlyMessage"))).ShowAsync();
				return;
			}

			// Show progress indicator.
			var progress = StatusBar.GetForCurrentView().ProgressIndicator;
			progress.Text = App.Strings.GetString("StatusLoggingIn");
			await progress.ShowAsync();

			try
			{
				// Try to log into Snapchat.
				await App.SnapchatManager.Endpoints.AuthenticateAsync(CurrentUsername, CurrentPassword);

				// Good, now lets get the data from the new API
				var updates = await App.SnapchatManager.Endpoints.GetAllUpdatesAsync();

				if (updates == null || updates.UpdatesResponse == null)
					throw new InvalidHttpResponseException();

				// Register this device for push notifications.
				await
					App.MobileService.GetTable<User>()
						.InsertAsync(new User
						{
							AuthExpired = false,
							NewUser = true,
							DeviceIdent = App.DeviceId,
							SnapchatAuthToken = App.SnapchatManager.AuthToken,
							SnapchatUsername = App.SnapchatManager.AllUpdates.UpdatesResponse.Username
						});

				// Navigate to the main page.
				var frame = Window.Current.Content as Frame;
				if (frame != null) frame.Navigate(typeof(MainPage));
			}
			catch (InvalidCredentialsException)
			{
				(new MessageDialog(App.Strings.GetString("InvalidCredentialsExceptionFriendlyMessage"))).ShowAsync();
			}
			catch (InvalidHttpResponseException e)
			{
				(new MessageDialog(App.Strings.GetString("InvalidHttpResponseExceptionMessage"))).ShowAsync();
				Debug.WriteLine(e.Message);
			}
			finally
			{
				// Hide progress indicator.
				progress.Text = String.Empty;
				progress.HideAsync();

				// Clear the password field.
				CurrentPassword = null;
			}
		}
	}
}
