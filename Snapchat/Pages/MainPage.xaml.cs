using System.Linq;
using Windows.System.Threading;
using Windows.UI.Core;
using Microsoft.Xaml.Interactivity;
using Snapchat.Common;
using System;
using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Snapchat.Helpers;
using Snapchat.Pages.PageContents;
using Snapchat.ViewModels;

namespace Snapchat.Pages
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class MainPage
	{
		#region App Bar Buttons

		private static readonly Uri FlashOnUri = new Uri("ms-appx:///Assets/Icons/appbar.camera.flash.png");
		private static readonly Uri FlashOffUri = new Uri("ms-appx:///Assets/Icons/appbar.camera.flash.off.png");

		private readonly AppBarButton _refreshAppBarButton = new AppBarButton
		{
			Label = App.Strings.GetString("RefreshAppBarButtonLabel")
		};

		private readonly AppBarButton _settingsAppBarButton = new AppBarButton
		{
			Label = App.Strings.GetString("SettingsAppBarButtonLabel"),
			Command = new RelayCommand(() =>
			{
				var frame = Window.Current.Content as Frame;
				if (frame != null) frame.Navigate(typeof (SettingsPage));
			})
		};

		private readonly AppBarButton _flipCameraAppBarButton = new AppBarButton
		{
			Icon = new BitmapIcon {UriSource = new Uri("ms-appx:///Assets/Icons/appbar.camera.flip.png")},
			Label = App.Strings.GetString("FlipCameraAppBarButtonLabel")
		};

		private readonly AppBarButton _toggleFlashAppBarButton = new AppBarButton
		{
			Icon = new BitmapIcon {UriSource = new Uri("ms-appx:///Assets/Icons/appbar.camera.flash.png")},
			Label = App.Strings.GetString("ToggleFlashAppBarButtonLabel"),
		};

		private readonly AppBarButton _downloadAllSnapsAppBarButton = new AppBarButton
		{
			Label = App.Strings.GetString("DownloadAllSnapsAppBarButtonLabel")
		};

		private readonly AppBarButton _importPictureAppBarButton = new AppBarButton
		{
			Label = App.Strings.GetString("ImportPictureAppBarButtonLabel")
		};

		private readonly AppBarButton _sendMessageAppBarButton = new AppBarButton
		{
			Icon = new SymbolIcon {Symbol = Symbol.Send},
			Label = App.Strings.GetString("SendCommentAppBarButtonLabel")
		};

		#endregion

		public MainViewModel ViewModel { get; private set; }

		public MainPage()
		{
			InitializeComponent();

			DataContext = ViewModel = new MainViewModel(ScrollViewer);

			ScrollViewer.ViewChanged += ScrollViewer_ViewChanged;
			ScrollViewer.ViewChanging += delegate
			{
				if (BottomAppBar != null)
					BottomAppBar.IsOpen = false;
			};

			PagesVisualStateGroup.CurrentStateChanged += delegate { UpdateBottomAppBar(); };

			_toggleFlashAppBarButton.Command = new RelayCommand(() =>
			{
				MediaCaptureManager.IsFlashEnabled = !MediaCaptureManager.IsFlashEnabled;
				// TODO: Change icon
			});

			_flipCameraAppBarButton.Command = new RelayCommand(async () =>
			{
				await MediaCaptureManager.ToggleCameraAsync();
				UpdateBottomAppBar();
			});

			CameraPage.Children.Clear();
			CameraPage.Children.Add(new CameraPageContent(ViewModel));
		}

		protected override async void OnNavigatedTo(NavigationEventArgs e)
		{
			// Must defer ChangeView execution by at least 10ms for it to work. Yeah, I know... wtf?
			ThreadPoolTimer.CreateTimer(async source =>
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
			var pageIndex = (int) Math.Round(ScrollViewer.HorizontalOffset/CameraPage.ActualWidth);
			var frameworkElement = PagesContainer.Children[pageIndex] as FrameworkElement;
			if (frameworkElement != null)
			{
				var currentPage = frameworkElement.Tag.ToString();

				// Change visual state to current page.
				VisualStateManager.GoToState(VisualStateUtilities.FindNearestStatefulControl(ScrollViewer), currentPage, true);
			}
			UpdateBottomAppBar();
		}

		private void UpdateBottomAppBar()
		{
			if (BottomAppBar == null)
				BottomAppBar = new CommandBar();
			var appBar = BottomAppBar as CommandBar;
			if (appBar == null) return;

			var primaryCommands = new Collection<ICommandBarElement>();
			var secondaryCommands = new Collection<ICommandBarElement>();
			var displayMode = appBar.ClosedDisplayMode;

			if (PagesVisualStateGroup.CurrentState != null)
			{
				var currentState = PagesVisualStateGroup.CurrentState.Name;
				switch (currentState)
				{
					case "Conversations":
						secondaryCommands.Add(_downloadAllSnapsAppBarButton);
						displayMode = AppBarClosedDisplayMode.Minimal;
						break;

					case "Camera":
						if (MediaCaptureManager.HasFrontCamera)
							primaryCommands.Add(_flipCameraAppBarButton);
					
						if (MediaCaptureManager.IsFlashSupported)
							primaryCommands.Add(_toggleFlashAppBarButton);

						secondaryCommands.Add(_importPictureAppBarButton);
						displayMode = AppBarClosedDisplayMode.Compact;
						break;

					case "Stories":
						displayMode = AppBarClosedDisplayMode.Minimal;
						break;

					case "ManageFriends":
						displayMode = AppBarClosedDisplayMode.Minimal;
						break;
				}
			}

			// Add global commands.
			secondaryCommands.Add(_refreshAppBarButton);
			secondaryCommands.Add(_settingsAppBarButton);

			// Update the app bar if commands have changed (to avoid animation glitches).
			bool updatePrimary = (primaryCommands.Count == 0), updateSecondary = (secondaryCommands.Count == 0);
			if (primaryCommands.Where((t, i) => appBar.PrimaryCommands.Count != primaryCommands.Count || !appBar.PrimaryCommands[i].Equals(t)).Any())
				updatePrimary = true;

			if (secondaryCommands.Where((t, i) => appBar.SecondaryCommands.Count != secondaryCommands.Count || !appBar.SecondaryCommands[i].Equals(t)).Any())
				updateSecondary = true;

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
