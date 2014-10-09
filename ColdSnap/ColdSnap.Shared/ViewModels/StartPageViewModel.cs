using System;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using ColdSnap.Common;
using ColdSnap.Helpers;
using ColdSnap.Pages;
using SnapDotNet;
using SnapDotNet.Data;
using SnapDotNet.Utilities;

namespace ColdSnap.ViewModels
{
	public sealed class StartPageViewModel
		: ObservableObject
	{
		public async Task LogInAsync(string username, string password)
		{
			// Display progress indicator.
			await StatusBarHelper.ShowStatusBarAsync(App.Strings.GetString("StatusLoggingIn"));

			string errorMessage = null;
			try
			{
				// Try to log in with the given credentials.
				Account account;
				if (username.StartsWith("auth:"))
					account = await Account.AuthenticateFromTokenAsync(username.Replace("auth:", ""), password);
				else
					account = await Account.AuthenticateAsync(username, password);

				// Hide progress indicator.
				await StatusBarHelper.HideStatusBarAsync();

				// Navigate to MainPage upon success.
				Window.Current.Navigate(typeof(MainPage), account);

				return;
			}
			catch (InvalidCredentialsException ex)
			{
				errorMessage = ex.Message;
			}
#if !DEBUG
			catch
			{
				errorMessage = App.Strings.GetString("UnknownLogInException");
			}
#endif

			// Display error message if there's one.
			if (errorMessage != null)
				await new MessageDialog(errorMessage).ShowAsync();

			// Hide progress indicator.
			await StatusBarHelper.HideStatusBarAsync();
		}
	}
}
