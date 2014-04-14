using System.Linq;
using System.Reflection;
using Windows.UI.Core;
using Windows.UI.Xaml.Navigation;
using SnapDotNet.Apps.Attributes;
using SnapDotNet.Apps.Common;
using SnapDotNet.Apps.Helpers;
using SnapDotNet.Apps.Pages;
using System;
using System.Diagnostics;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using SnapDotNet.Apps.Pages.SignedIn;
using SnapDotNet.Core.Miscellaneous.Helpers;
using SnapDotNet.Core.Snapchat.Api;
using SnapDotNet.Core.Snapchat.Api.Exceptions;
#if WINDOWS_PHONE_APP
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.System.Profile;
using Microsoft.WindowsAzure.MobileServices;
using Windows.Networking.PushNotifications;
using SnapDotNet.Core.Miscellaneous.Crypto;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.ViewManagement;
#endif

namespace SnapDotNet.Apps
{
	/// <summary>
	///     Provides application-specific behavior to supplement the default Application class.
	/// </summary>
	public sealed partial class App
	{
#if WINDOWS_PHONE_APP
		public static MobileServiceClient MobileService = new MobileServiceClient(
			"https://snapdotnet.azure-mobile.net/",
			"sTdykEmtfJsTQmafUMxrKalcdkaphW67"
		);

		public static String DeviceIdent;
		private TransitionCollection _transitions;
#endif

		/// <summary>
		/// 
		/// </summary>
		public static readonly ResourceLoader Loader = new ResourceLoader();

		/// <summary>
		/// 
		/// </summary>
		public static readonly SnapChatManager SnapChatManager = new SnapChatManager();

		/// <summary>
		/// 
		/// </summary>
		public static readonly ApplicationSettings Settings = new ApplicationSettings();

		/// <summary>
		///     Initializes the singleton application object.  This is the first line of authored code
		///     executed, and as such is the logical equivalent of main() or WinMain().
		/// </summary>
		public App()
		{
			InitializeComponent();
			Suspending += OnSuspending;
			Resuming += OnResuming;
		}

		/// <summary>
		/// Gets the current frame.
		/// </summary>
		public static Frame CurrentFrame { get { return (Frame) Window.Current.Content; } }

		/// <summary>
		///     Invoked when the application is launched normally by the end user.  Other entry points
		///     will be used when the application is launched to open a specific file, to display
		///     search results, and so forth.
		/// </summary>
		/// <param name="e">Details about the launch request and process.</param>
		protected override void OnLaunched(LaunchActivatedEventArgs e)
		{
#if DEBUG
			if (Debugger.IsAttached)
			{
				DebugSettings.EnableFrameRateCounter = true;
			}
#endif

			var rootFrame = Window.Current.Content as Frame;

			// Do not repeat app initialization when the Window already has content,
			// just ensure that the window is active
			if (rootFrame == null)
			{
				// Create a Frame to act as the navigation context and navigate to the first page
				rootFrame = new Frame
				{
					// TODO: change this value to a cache size
					CacheSize = 1
				};

				if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
				{
					// TODO: Load state from previously suspended instance
				}

				// Place the frame in the current Window
				Window.Current.Content = rootFrame;
			}
			
			// Create events
			CurrentFrame.Navigating += CurrentFrameOnNavigating;

			if (rootFrame.Content == null)
			{
#if WINDOWS_PHONE_APP
				// Removes the turnstile navigation for startup.
				if (rootFrame.ContentTransitions != null)
				{
					_transitions = new TransitionCollection();
					foreach (var c in rootFrame.ContentTransitions)
					{
						_transitions.Add(c);
					}
				}

				rootFrame.ContentTransitions = null;
				rootFrame.Navigated += RootFrame_FirstNavigated;
#endif

				// When the navigation stack isn't restored navigate to the first page,
				// configuring the new page by passing required information as a navigation
				// parameter
				if (!rootFrame.Navigate(typeof (MainPage), e.Arguments))
				{
					throw new Exception("Failed to create initial page");
				}
			}
			
			// Register for Push Notifications
			InitNotificationsAsync();

			// Get Snapchat Updates
			UpdateSnapchatData();
			
#if WINDOWS_PHONE_APP
			// Hide StatusBar background for entire application
			ApplicationView.GetForCurrentView().SetDesiredBoundsMode(ApplicationViewBoundsMode.UseCoreWindow);
#endif

			// Ensure the current window is active
			Window.Current.Activate();
		}

		private static void CurrentFrameOnNavigating(object sender, NavigatingCancelEventArgs navigatingCancelEventArgs)
		{
			var requiresAuthentication =
				navigatingCancelEventArgs.SourcePageType.GetTypeInfo()
					.CustomAttributes.FirstOrDefault(a => a.AttributeType == typeof (RequiresAuthentication)) != null;

			if (!requiresAuthentication || SnapChatManager.IsAuthenticated()) return;

			CurrentFrame.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
					() => CurrentFrame.Navigate(typeof(StartPage)));
		}

#if WINDOWS_PHONE_APP
		/// <summary>
		///     Restores the content transitions after the app has launched.
		/// </summary>
		/// <param name="sender">The object where the handler is attached.</param>
		/// <param name="e">Details about the navigation event.</param>
		private void RootFrame_FirstNavigated(object sender, NavigationEventArgs e)
		{
			var rootFrame = sender as Frame;
			if (rootFrame == null) return;

			rootFrame.ContentTransitions = _transitions ?? new TransitionCollection {new NavigationThemeTransition()};
			rootFrame.Navigated -= RootFrame_FirstNavigated;
		}
#endif

		/// <summary>
		/// 
		/// </summary>
		private static async void InitNotificationsAsync()
		{
#if WINDOWS_PHONE_APP
			DeviceIdent = Sha.Sha256(BitConverter.ToString(HardwareIdentification.GetPackageSpecificToken(null).Id.ToArray()));
			var channel = await PushNotificationChannelManager.CreatePushNotificationChannelForApplicationAsync();
			channel.PushNotificationReceived += (sender, args) =>
			{
				switch (args.NotificationType)
				{
					case PushNotificationType.Toast:
						if (args.ToastNotification == null) return;

						// TODO: Do logic work
						args.ToastNotification.Group = "Dev";
						break;

					default:
						throw new NotImplementedException();
				}
			};

			try
			{
				// Request a push notification channel.
				await MobileService.GetPush().RegisterNativeAsync(channel.Uri, new[] { DeviceIdent });
			}
			catch (Exception exception)
			{
				if (exception.HResult == 0x803E0103) // register request is already in progress
					return;
				throw;
			}
#endif
		}
		

		/// <summary>
		///     Invoked when application execution is being suspended.  Application state is saved
		///     without knowing whether the application will be terminated or resumed with the contents
		///     of memory still intact.
		/// </summary>
		/// <param name="sender">The source of the suspend request.</param>
		/// <param name="e">Details about the suspend request.</param>
		private static void OnSuspending(object sender, SuspendingEventArgs e)
		{
			var deferral = e.SuspendingOperation.GetDeferral();

			// TODO: Save application state and stop any background activity
			deferral.Complete();
		}

		private static void OnResuming(object sender, object o)
		{
			UpdateSnapchatData();
		}

		public static async void UpdateSnapchatData()
		{
			if (!SnapChatManager.IsAuthenticated()) return;

			// Get Snapchat Updates
			try
			{
				await ProgressHelper.ShowStatusBar("Updating...");
				await SnapChatManager.UpdateAllAsync(async () => { await ProgressHelper.HideStatusBar(); }, Settings);
			}
			catch (Exception exception)
			{
				SnazzyDebug.WriteLine(exception);
				var logoutTask = SnapChatManager.Endpoints.LogoutAsync();
				var changePageTask = CurrentFrame.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => CurrentFrame.Navigate(typeof(StartPage)));
			}
		}
	}
}