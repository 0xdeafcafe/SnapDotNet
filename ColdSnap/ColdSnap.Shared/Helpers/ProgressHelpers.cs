using System;
using System.Threading.Tasks;
using Windows.UI.ViewManagement;

namespace ColdSnap.Helpers
{
	public static class ProgressHelper
	{
		public static async Task ShowStatusBarAsync(string status)
		{
#if WINDOWS_PHONE_APP
			StatusBar.GetForCurrentView().ProgressIndicator.Text = status;
			await StatusBar.GetForCurrentView().ProgressIndicator.ShowAsync();
#endif
		}

		public static void ShowStatusBar(string status)
		{
#if WINDOWS_PHONE_APP
			StatusBar.GetForCurrentView().ProgressIndicator.Text = status;
			StatusBar.GetForCurrentView().ProgressIndicator.ShowAsync();
#endif
		}

		public static async Task HideStatusBarAsync()
		{
#if WINDOWS_PHONE_APP
			StatusBar.GetForCurrentView().ProgressIndicator.Text = String.Empty;
			await StatusBar.GetForCurrentView().ProgressIndicator.HideAsync();
#endif
		}

		public static void HideStatusBar()
		{
#if WINDOWS_PHONE_APP
			StatusBar.GetForCurrentView().ProgressIndicator.Text = String.Empty;
			StatusBar.GetForCurrentView().ProgressIndicator.HideAsync();
#endif
		}
	}
}