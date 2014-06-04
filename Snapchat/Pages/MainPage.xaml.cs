using System.Diagnostics;
using Windows.Devices.Enumeration;
using Windows.Media.Capture;
using Windows.System.Threading;
using Windows.UI.Core;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Microsoft.Xaml.Interactivity;
using Snapchat.Common;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Snapchat.Helpers;

namespace Snapchat.Pages
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class MainPage : Page
	{
		#region App Bar Buttons

		private static readonly Uri FlashOnUri = new Uri("ms-appx:///Assets/Icons/appbar.camera.flash.png");
		private static readonly Uri FlashOffUri = new Uri("ms-appx:///Assets/Icons/appbar.camera.flash.off.png");

		private readonly AppBarButton RefreshAppBarButton = new AppBarButton
		{
			Label = App.Strings.GetString("RefreshAppBarButtonLabel")
		};

		private readonly AppBarButton SettingsAppBarButton = new AppBarButton
		{
			Label = App.Strings.GetString("SettingsAppBarButtonLabel"),
			Command = new RelayCommand(() => { (Window.Current.Content as Frame).Navigate(typeof (SettingsPage)); })
		};

		private readonly AppBarButton FlipCameraAppBarButton = new AppBarButton
		{
			Icon = new BitmapIcon {UriSource = new Uri("ms-appx:///Assets/Icons/appbar.camera.flip.png")},
			Label = App.Strings.GetString("FlipCameraAppBarButtonLabel")
		};

		private readonly AppBarButton ToggleFlashAppBarButton = new AppBarButton
		{
			Icon = new BitmapIcon {UriSource = new Uri("ms-appx:///Assets/Icons/appbar.camera.flash.png")},
			Label = App.Strings.GetString("ToggleFlashAppBarButtonLabel"),
		};

		private readonly AppBarButton DownloadAllSnapsAppBarButton = new AppBarButton
		{
			Label = App.Strings.GetString("DownloadAllSnapsAppBarButtonLabel")
		};

		private readonly AppBarButton ImportPictureAppBarButton = new AppBarButton
		{
			Label = App.Strings.GetString("ImportPictureAppBarButtonLabel")
		};

		private readonly AppBarButton SendMessageAppBarButton = new AppBarButton
		{
			Icon = new SymbolIcon {Symbol = Symbol.Send},
			Label = App.Strings.GetString("SendCommentAppBarButtonLabel")
		};

		#endregion

		public MainPage()
		{
			InitializeComponent();

			ScrollViewer.ViewChanged += ScrollViewer_ViewChanged;
			ScrollViewer.ViewChanging += delegate
			{
				if (BottomAppBar != null)
					BottomAppBar.IsOpen = false;
			};

			PagesVisualStateGroup.CurrentStateChanged += delegate { UpdateBottomAppBar(); };

			ToggleFlashAppBarButton.Command = new RelayCommand(() =>
			{
				MediaCaptureManager.IsFlashEnabled = !MediaCaptureManager.IsFlashEnabled;
				// TODO: Change icon
			});

			FlipCameraAppBarButton.Command = new RelayCommand(async () =>
			{
				await MediaCaptureManager.ToggleCameraAsync();
				UpdateBottomAppBar();
			});
		}

		protected override async void OnNavigatedTo(NavigationEventArgs e)
		{
			// Must defer ChangeView execution by at least 10ms for it to work. Yeah, I know... wtf?
			ThreadPoolTimer.CreateTimer(async (source) =>
			{
				await Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
					() => ScrollViewer.ChangeView(CameraPage.ActualWidth, null, null, true));
			},
			TimeSpan.FromMilliseconds(50));

			if (!App.SnapchatManager.Loaded)
				await App.SnapchatManager.LoadAsync();
		}

		protected async override void OnNavigatedFrom(NavigationEventArgs e)
		{
			await MediaCaptureManager.StopPreviewAsync();
		}

		private void ScrollViewer_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
		{
			// Determine the page that's currently in view.
			int pageIndex = (int) Math.Round(ScrollViewer.HorizontalOffset/CameraPage.ActualWidth);
			string currentPage = (PagesContainer.Children[pageIndex] as FrameworkElement).Tag.ToString();

			// Change visual state to current page.
			VisualStateManager.GoToState(VisualStateUtilities.FindNearestStatefulControl(ScrollViewer), currentPage, true);
			UpdateBottomAppBar();
		}

		private void UpdateBottomAppBar()
		{
			if (BottomAppBar == null)
				BottomAppBar = new CommandBar();
			var appBar = BottomAppBar as CommandBar;

			var primaryCommands = new Collection<ICommandBarElement>();
			var secondaryCommands = new Collection<ICommandBarElement>();
			AppBarClosedDisplayMode displayMode = appBar.ClosedDisplayMode;

			var currentState = PagesVisualStateGroup.CurrentState.Name;
			switch (currentState)
			{
				case "Conversations":
					secondaryCommands.Add(DownloadAllSnapsAppBarButton);
					displayMode = AppBarClosedDisplayMode.Minimal;
					break;

				case "Camera":
					if (MediaCaptureManager.HasFrontCamera)
						primaryCommands.Add(FlipCameraAppBarButton);
					
					if (MediaCaptureManager.IsFlashSupported)
						primaryCommands.Add(ToggleFlashAppBarButton);

					secondaryCommands.Add(ImportPictureAppBarButton);
					displayMode = AppBarClosedDisplayMode.Compact;
					break;

				case "Stories":
					displayMode = AppBarClosedDisplayMode.Minimal;
					break;

				case "ManageFriends":
					displayMode = AppBarClosedDisplayMode.Minimal;
					break;
			}

			// Add global commands.
			secondaryCommands.Add(RefreshAppBarButton);
			secondaryCommands.Add(SettingsAppBarButton);

			// Update the app bar if commands have changed (to avoid animation glitches).
			bool updatePrimary = (primaryCommands.Count == 0), updateSecondary = (secondaryCommands.Count == 0);
			for (int i = 0; i < primaryCommands.Count; i++)
			{
				if (appBar.PrimaryCommands.Count != primaryCommands.Count || !appBar.PrimaryCommands[i].Equals(primaryCommands[i]))
				{
					updatePrimary = true;
					break;
				}
			}
			for (int i = 0; i < secondaryCommands.Count; i++)
			{
				if (appBar.SecondaryCommands.Count != secondaryCommands.Count || !appBar.SecondaryCommands[i].Equals(secondaryCommands[i]))
				{
					updateSecondary = true;
					break;
				}
			}
			if (updatePrimary)
			{
				appBar.PrimaryCommands.Clear();
				foreach (var command in primaryCommands)
					appBar.PrimaryCommands.Add(command);
			}
			if (updateSecondary)
			{
				appBar.SecondaryCommands.Clear();
				foreach (var command in secondaryCommands)
					appBar.SecondaryCommands.Add(command);
			}
			appBar.ClosedDisplayMode = displayMode;
		}
	}
}
