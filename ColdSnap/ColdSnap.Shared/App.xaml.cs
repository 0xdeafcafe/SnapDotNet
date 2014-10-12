using System;
using System.Diagnostics;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Background;
using Windows.ApplicationModel.Resources;
using Windows.Networking.PushNotifications;
using Windows.System.Profile;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using ColdSnap.Pages;
using ColdSnap.Utilities;
using Microsoft.WindowsAzure.MobileServices;
using SnapDotNet.Utilities;

namespace ColdSnap
{
    public sealed partial class App
    {
		public static readonly ResourceLoader Strings = new ResourceLoader();

		public static MobileServiceClient MobileService = new MobileServiceClient(
			"https://snapdotnet.azure-mobile.net/",
			"sTdykEmtfJsTQmafUMxrKalcdkaphW67"
		);

		public static string DeviceIdent =
			Sha.Sha256(BitConverter.ToString(HardwareIdentification.GetPackageSpecificToken(null).Id.ToArray()));

        public App()
        {
            InitializeComponent();
            Suspending += OnSuspending;
        }

		internal static async Task StopPushNotificationsAsync()
		{
			// TODO
		}

		private static async Task RegisterPushChannelAsync()
		{
			if (await BackgroundExecutionManager.RequestAccessAsync() == BackgroundAccessStatus.Denied)
			{
				Debug.WriteLine("[SettingsPageViewModel] Background task access request was denied");
				await new MessageDialog(App.Strings.GetString("BackgroundAccessStatusDeniedMessage")).ShowAsync();
				return;
			}

			// Get rid of existing registrations.
			foreach (var task in BackgroundTaskRegistration.AllTasks)
				task.Value.Unregister(false);

			// Register a background task.
			var builder = new BackgroundTaskBuilder
			{
				Name = "Notifications Push Task",
				TaskEntryPoint = typeof(BackgroundPushTask.BackgroundPushTask).FullName
			};
			builder.SetTrigger(new PushNotificationTrigger());
			builder.Register();

			var channel = await PushNotificationChannelManager.CreatePushNotificationChannelForApplicationAsync();
			await MobileService.GetPush().RegisterNativeAsync(channel.Uri, new[] { "SnapDotNetUser", App.DeviceIdent });

			Debug.WriteLine("[SettingsPageViewModel] Background task registered");
		}

        protected override async void OnLaunched(LaunchActivatedEventArgs e)
        {
#if DEBUG
            if (Debugger.IsAttached)
                DebugSettings.EnableFrameRateCounter = true;
#endif

			// Create navigation context and set it as the window's content.
            var rootFrame = Window.Current.Content as Frame;
            if (rootFrame == null)
            {
	            rootFrame = new Frame {CacheSize = 2};

	            if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    // TODO: Load state from previously suspended application
                }

                Window.Current.Content = rootFrame;
            }

			// Navigate to the starting page when the navigation stack isn't restored.
            if (rootFrame.Content == null)
            {
	            var destination = await StorageManager.Local.FileExistsAsync("account")
		            ? typeof (MainPage)
		            : typeof (StartPage);

				// Register for push notifications.
				RegisterPushChannelAsync();

                if (!rootFrame.Navigate(destination))
                    throw new Exception("Failed to create initial page");
            }

			// Activate the window.
            Window.Current.Activate();
        }

        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();

            // TODO: Save application state and stop any background activity
            deferral.Complete();
        }
    }
}