using System;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.ViewManagement;

namespace ColdSnap.Helpers
{
    public static class StatusBarHelper
    {
		public static async Task ShowStatusBarAsync(string status)
		{
#if WINDOWS_PHONE_APP
			StatusBar.GetForCurrentView().ProgressIndicator.Text = status;
			await StatusBar.GetForCurrentView().ProgressIndicator.ShowAsync();
#else
			throw new NotImplementedException();
#endif
		}

		public static async Task HideStatusBarAsync()
		{
#if WINDOWS_PHONE_APP
			StatusBar.GetForCurrentView().ProgressIndicator.Text = String.Empty;
			await StatusBar.GetForCurrentView().ProgressIndicator.HideAsync();
#else
			throw new NotImplementedException();
#endif
		}
    }
}
