using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Background;
using Windows.ApplicationModel.Resources;
using Windows.Networking.PushNotifications;
using Windows.System.Profile;
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
using SnapDotNet.Core.Miscellaneous.Crypto;
using SnapDotNet.Core.Snapchat.Api;
using Snapchat.ViewModels;
using SnapDotNet.Core.Miscellaneous.Helpers;
using System.Diagnostics;
using Windows.UI.Core;

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

		public static string DeviceIdent =
			Sha.Sha256(BitConverter.ToString(HardwareIdentification.GetPackageSpecificToken(null).Id.ToArray()));

		public static readonly SnapchatManager SnapchatManager = new SnapchatManager();

		public static readonly CameraManager Camera = new CameraManager();

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
			Resuming += OnResuming;
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
			if (Debugger.IsAttached)
			{
				DebugSettings.EnableFrameRateCounter = true;
			}
#endif

			// Do not repeat app initialization when the Window already has content,
			// just ensure that the window is active
			if (RootFrame == null)
			{
				// Create a Frame to act as the navigation context and navigate to the first page
				RootFrame = new Frame { CacheSize = 1 };

				if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
				{
					
				}

				// Place the frame in the current Window
				Window.Current.Content = RootFrame;
				Window.Current.VisibilityChanged += OnCurrentWindowVisibilityChanged;
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

				// Register for Push Notifications
				RegisterPushChannel();

				// When the navigation stack isn't restored navigate to the first page,
				// configuring the new page by passing required information as a navigation
				// parameter
				if (!RootFrame.Navigate(destinationPage, e.Arguments))
				{
					throw new Exception("Failed to create initial page");
				}
			}
			
			// yolo rite
			Window.Current.Activate();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="o"></param>
		private static void OnResuming(object sender, object o)
		{
			Debug.WriteLine("Resuming app");
			UpdateSnapchatDataAsync();
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
		private static void OnSuspending(object sender, SuspendingEventArgs e)
		{
			Debug.WriteLine("Suspending app");

			var deferral = e.SuspendingOperation.GetDeferral();

			// Save state here

			deferral.Complete();
		}
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private static void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
			if (Camera != null)
				Camera.Dispose();

			Current.Exit();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private static async void OnCurrentWindowVisibilityChanged(object sender, VisibilityChangedEventArgs e)
		{
			if (e.Visible)
			{
				await Camera.StartPreviewAsync();
			}
			else
			{
				await Camera.StopPreviewAsync();
			}
		}

		#region Snapchat Data Helpers

		// TODO: Do this in a much nicer way
		public static async void UpdateSnapchatDataAsync()
		{
			// TODO: check if we're signed in, if not skip this shit

			// show update ui
			await ProgressHelper.ShowStatusBarAsync(Strings.GetString("StatusUpdating"));

			// update data, and hide ui
			await SnapchatManager.UpdateAllAsync(ProgressHelper.HideStatusBar);

			// Automatically download snaps if enabled
			var downloadMode = AppSettings.Get("AutomaticallyDownloadSnapsMode", AutomaticallyDownloadSnapsMode.WiFi);
			var shouldDownloadSnaps =
				(downloadMode == AutomaticallyDownloadSnapsMode.Always) ||
				(downloadMode == AutomaticallyDownloadSnapsMode.WiFi && !NetworkInformationHelper.OnWifiConnection());
			if (shouldDownloadSnaps) await SnapchatManager.DownloadSnapsAsync();
		}

		#endregion
		
		#region Push Channel

		private static async void RegisterPushChannel()
		{
			// Setup background task
			await BackgroundExecutionManager.RequestAccessAsync();

			// Get rid of existing registrations.  
			foreach (var task in BackgroundTaskRegistration.AllTasks)
			{
				try
				{
					task.Value.Unregister(false);
				}
				catch (Exception ex)
				{
					SnazzyDebug.WriteLine(ex);
				}
			}
			var builder = new BackgroundTaskBuilder
			{
				Name = "Background Push Task",
				TaskEntryPoint = typeof(BackgroundPushTask.BackgroundPushTask).FullName
			};
			builder.SetTrigger(new PushNotificationTrigger());
			builder.Register();

			var channel = await PushNotificationChannelManager.CreatePushNotificationChannelForApplicationAsync();
			try
			{
				await MobileService.GetPush().RegisterNativeAsync(channel.Uri, new[] {"SnapDotNetUser", DeviceIdent});
			}
			catch (Exception ex)
			{
				SnazzyDebug.WriteLine(ex);
			}
		}  

		#endregion
	}
}