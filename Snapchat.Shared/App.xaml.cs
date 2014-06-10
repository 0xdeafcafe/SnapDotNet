using Windows.ApplicationModel.Resources;
using Microsoft.WindowsAzure.MobileServices;
using Snapchat.Helpers;
using Snapchat.Pages;
using System;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using SnapDotNet.Core.Snapchat.Api;
using Snapchat.ViewModels;
using SnapDotNet.Core.Miscellaneous.Helpers;

namespace Snapchat
{
	/// <summary>
	/// Provides application-specific behavior to supplement the default Application class.
	/// </summary>
	public sealed partial class App
	{
		public static readonly ResourceLoader Strings = new ResourceLoader();

		public static MobileServiceClient MobileService = new MobileServiceClient(
			"https://snapdotnet.azure-mobile.net/",
			"sTdykEmtfJsTQmafUMxrKalcdkaphW67"
		);
		public static string DeviceId;

		public static readonly SnapchatManager SnapchatManager = new SnapchatManager();

		private TransitionCollection _transitions;

		/// <summary>
		/// Initializes the singleton application object.  This is the first line of authored code
		/// executed, and as such is the logical equivalent of main() or WinMain().
		/// </summary>
		public App()
		{
			InitializeComponent();

			Suspending += OnSuspending;
			UnhandledException += OnUnhandledException;
		}

		public static Frame RootFrame { get; private set; }
		public static Type PreviousPage { get; set; }

		/// <summary>
		/// Invoked when the application is launched normally by the end user.  Other entry points
		/// will be used when the application is launched to open a specific file, to display
		/// search results, and so forth.
		/// </summary>
		/// <param name="e">Details about the launch request and process.</param>
		protected override async void OnLaunched(LaunchActivatedEventArgs e)
		{
#if DEBUG
			if (System.Diagnostics.Debugger.IsAttached)
			{
				DebugSettings.EnableFrameRateCounter = true;
			}
#endif

			// Do not repeat app initialization when the Window already has content,
			// just ensure that the window is active
			if (RootFrame == null)
			{
				// Create a Frame to act as the navigation context and navigate to the first page
				RootFrame = new Frame { CacheSize = 3 };

				if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
				{

				}

				// Place the frame in the current Window
				Window.Current.Content = RootFrame;
			}

			if (RootFrame.Content == null)
			{
				// Removes the turnstile navigation for startup.
				if (RootFrame.ContentTransitions != null)
				{
					_transitions = new TransitionCollection();
					foreach (var c in RootFrame.ContentTransitions)
					{
						_transitions.Add(c);
					}
				}

				RootFrame.ContentTransitions = null;
				RootFrame.Navigated += RootFrame_FirstNavigated;

				// Go to Main Page if the user is still authenticated.
				if (!SnapchatManager.Loaded)
					await SnapchatManager.LoadAsync();
				var destinationPage = SnapchatManager.IsAuthenticated() ? typeof (MainPage) : typeof (StartPage);

				// When the navigation stack isn't restored navigate to the first page,
				// configuring the new page by passing required information as a navigation
				// parameter
				if (!RootFrame.Navigate(destinationPage, e.Arguments))
				{
					throw new Exception("Failed to create initial page");
				}
			}

			Window.Current.Activate();
		}

		/// <summary>
		/// Restores the content transitions after the app has launched.
		/// </summary>
		/// <param name="sender">The object where the handler is attached.</param>
		/// <param name="e">Details about the navigation event.</param>
		private void RootFrame_FirstNavigated(object sender, NavigationEventArgs e)
		{
			var rootFrame = sender as Frame;
			if (rootFrame == null) return;
			rootFrame.ContentTransitions = _transitions ?? new TransitionCollection { new NavigationThemeTransition() };
			rootFrame.Navigated -= RootFrame_FirstNavigated;
		}

		/// <summary>
		/// Invoked when application execution is being suspended.  Application state is saved
		/// without knowing whether the application will be terminated or resumed with the contents
		/// of memory still intact.
		/// </summary>
		/// <param name="sender">The source of the suspend request.</param>
		/// <param name="e">Details about the suspend request.</param>
		private static async void OnSuspending(object sender, SuspendingEventArgs e)
		{
			var deferral = e.SuspendingOperation.GetDeferral();

			await MediaCaptureManager.CleanupCaptureResourcesAsync();

			deferral.Complete();
		}

		private static async void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
			await MediaCaptureManager.CleanupCaptureResourcesAsync();
		}

		#region Snapchat Data Helpers
		// TODO: Do this in a much nicer way

		public static async void UpdateSnapchatDataAsync()
		{
			// show update ui
			await ProgressHelper.ShowStatusBarAsync(Strings.GetString("StatusUpdating"));

			// update data, and hide ui
			await SnapchatManager.UpdateAllAsync(ProgressHelper.HideStatusBar);

			// Automatically download snaps if enabled
			var downloadMode = AppSettings.Get<AutomaticallyDownloadSnapsMode>("AutomaticallyDownloadSnapsMode", defaultValue: AutomaticallyDownloadSnapsMode.WiFi);
			bool shouldDownloadSnaps =
				(downloadMode == AutomaticallyDownloadSnapsMode.Always) ||
				(downloadMode == AutomaticallyDownloadSnapsMode.WiFi && !NetworkInformationHelper.OnWifiConnection());
			if (shouldDownloadSnaps)
			{
				await SnapchatManager.DownloadSnapsAsync();
			}
		}

		#endregion
	}
}