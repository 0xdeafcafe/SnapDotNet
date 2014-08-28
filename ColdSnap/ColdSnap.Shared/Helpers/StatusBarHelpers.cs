using System;
using Windows.UI.ViewManagement;

namespace ColdSnap.Helpers
{
	public static class StatusBarHelpers
	{
		public static async void ShowStatusBar()
		{
			var statusBar = StatusBar.GetForCurrentView();
			await statusBar.ShowAsync();
		}

		public static async void HideStatusBar()
		{
			var statusBar = StatusBar.GetForCurrentView();
			await statusBar.HideAsync();
		}
	}
}
