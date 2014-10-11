using System;
using System.Diagnostics;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Resources;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using ColdSnap.Pages;
using SnapDotNet.Utilities;

namespace ColdSnap
{
    public sealed partial class App : Application
    {
		public static readonly ResourceLoader Strings = new ResourceLoader();

        public App()
        {
            InitializeComponent();
            Suspending += OnSuspending;
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

                if (!rootFrame.Navigate(destination))
                    throw new Exception("Failed to create initial page");
            }

			// Activate the window.
            Window.Current.Activate();

			// Memory usage monitoring
#if DEBUG
			Debug.WriteLine("[App] Allocated memory: {0} MB", MemoryManager.AppMemoryUsageLimit / 1024 / 1024);
	        MemoryManager.AppMemoryUsageIncreased += delegate
	        {
		        Debug.WriteLine("[App] Memory usage increased to {0} (allocated memory: {1} MB)", MemoryManager.AppMemoryUsageLevel.ToString().ToUpperInvariant(), MemoryManager.AppMemoryUsageLimit / 1024 / 1024);
	        };
			MemoryManager.AppMemoryUsageDecreased += delegate
			{
				Debug.WriteLine("[App] Memory usage decreased to {0} (allocated memory: {1} MB)", MemoryManager.AppMemoryUsageLevel.ToString().ToUpperInvariant(), MemoryManager.AppMemoryUsageLimit / 1024 / 1024);
			};
			MemoryManager.AppMemoryUsageLimitChanging += (sender, args) =>
			{
				Debug.WriteLine("[App] Memory usage limit set to {0} MB (previously {1} MB)", args.NewLimit / 1024 / 1024, args.OldLimit / 1024 / 1024);
			};
#endif
        }

        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();

            // TODO: Save application state and stop any background activity
            deferral.Complete();
        }
    }
}