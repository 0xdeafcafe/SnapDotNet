using System;
using System.Threading.Tasks;
using Windows.UI.ViewManagement;

namespace SnapDotNet.Apps.Helpers
{
	public static class ProgressHelper
	{
		public static async Task ShowStatusBar(string status)
		{
#if WINDOWS_PHONE_APP
			StatusBar.GetForCurrentView().ProgressIndicator.Text = App.Loader.GetString("Updating");
			await StatusBar.GetForCurrentView().ProgressIndicator.ShowAsync();
#endif
		}

		public static async Task HideStatusBar()
		{
#if WINDOWS_PHONE_APP
			StatusBar.GetForCurrentView().ProgressIndicator.Text = String.Empty;
			await StatusBar.GetForCurrentView().ProgressIndicator.HideAsync();
#endif
		}
	}
}
