using System.Linq;
using Windows.ApplicationModel;
using Windows.Phone.UI.Input;
using Windows.System.Threading;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Microsoft.Xaml.Interactivity;
using Snapchat.Attributes;
using Snapchat.Common;
using System;
using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Snapchat.Helpers;

namespace Snapchat.Pages
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	[RequiresAuthentication]
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

		private double _previousScrollViewerOffset = 0;
		private bool _goingToCamera = false;

		public MainPage()
		{
			InitializeComponent();

			ScrollViewer.ViewChanged += ScrollViewer_ViewChanged;
			ScrollViewer.ViewChanging += ScrollViewer_ViewChanging;

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
		}

		protected override async void OnNavigatedTo(NavigationEventArgs e)
		{
			// Must defer ChangeView execution by at least 10ms for it to work. Yeah, I know... wtf?
			ThreadPoolTimer.CreateTimer(async source =>
			{
				await Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
					() => ScrollViewer.ChangeView(CameraPage.ActualWidth, null, null, true));
			},
			TimeSpan.FromMilliseconds(200));

			// Start the camera.
			if (!DesignMode.DesignModeEnabled)
				await MediaCaptureManager.StartPreviewAsync(CapturePreview);

			if (!App.SnapchatManager.Loaded)
				await App.SnapchatManager.LoadAsync();

			HardwareButtons.BackPressed += HardwareButtons_BackPressed;
			HardwareButtons.CameraPressed += HardwareButtons_CameraPressed;
		}

		protected async override void OnNavigatedFrom(NavigationEventArgs e)
		{
			HardwareButtons.BackPressed -= HardwareButtons_BackPressed;
			HardwareButtons.CameraPressed -= HardwareButtons_CameraPressed;
			await MediaCaptureManager.StopPreviewAsync();
		}

		private void HardwareButtons_CameraPressed(object sender, CameraEventArgs e)
		{
			if (PagesVisualStateGroup.CurrentState == null)
				return;

			var currentState = PagesVisualStateGroup.CurrentState.Name;
			if (currentState == "Camera")
			{
				// TODO: Take a picture
			}
			else
			{
				ScrollViewer.HorizontalSnapPointsType = SnapPointsType.None;
				_goingToCamera = true;
				ScrollViewer.ChangeView(CameraPage.ActualWidth, null, null, false); // go to camera
			}
		}

		private void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
		{
			if (PagesVisualStateGroup.CurrentState == null)
				return;

			var currentState = PagesVisualStateGroup.CurrentState.Name;
			switch (currentState)
			{
				case "Conversations":
					ScrollViewer.ChangeView(CameraPage.ActualWidth, null, null, false); // go to camera
					e.Handled = true;
					break;

				case "Camera":
					// exit the app
					break;

				case "Stories":
					ScrollViewer.HorizontalSnapPointsType = SnapPointsType.None;
					ScrollViewer.ChangeView(CameraPage.ActualWidth, null, null, false); // go to camera
					e.Handled = true;
					break;

				case "ManageFriends":
					ScrollViewer.ChangeView(CameraPage.ActualWidth * 2, null, null, false); // go to stories
					e.Handled = true;
					break;
			}
		}

		private void ScrollViewer_ViewChanging(object sender, ScrollViewerViewChangingEventArgs e)
		{
			if (BottomAppBar != null)
				BottomAppBar.IsOpen = false;

			// Fix scroll viewer after scrolling all the way back to camera from the manage friends page.
			if ((ScrollViewer.HorizontalOffset > _previousScrollViewerOffset && !_goingToCamera) || ScrollViewer.HorizontalOffset < CameraPage.ActualWidth)
				ScrollViewer.HorizontalSnapPointsType = SnapPointsType.MandatorySingle;
			_previousScrollViewerOffset = ScrollViewer.HorizontalOffset;
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

			// Prevent stories and convo icons from retaining their touch response colors whenever the user slips their finger.
			StoriesIcon.Background = new SolidColorBrush(Colors.Transparent);
			ConversationsIcon.Background = new SolidColorBrush(Colors.Transparent);

			// Fix scroll viewer after scrolling all the way back to camera from the manage friends page.
			if (pageIndex == 1)
			{
				_goingToCamera = false;
				if (!e.IsIntermediate)
					ScrollViewer.HorizontalSnapPointsType = SnapPointsType.MandatorySingle;
			}
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

		private void ConversationsIcon_Tapped(object sender, TappedRoutedEventArgs e)
		{
			ThreadPoolTimer.CreateTimer(async source =>
			{
				await Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
					() => ScrollViewer.ChangeView(0, null, null, false));
			},
			TimeSpan.FromMilliseconds(25));

			ConversationsIcon.Background = new SolidColorBrush(Colors.Transparent);
		}

		private void StoriesIcon_Tapped(object sender, TappedRoutedEventArgs e)
		{
			ThreadPoolTimer.CreateTimer(async source =>
			{
				await Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
					() => ScrollViewer.ChangeView(CameraPage.ActualWidth * 2, null, null, false));
			},
			TimeSpan.FromMilliseconds(25));

			StoriesIcon.Background = new SolidColorBrush(Colors.Transparent);
		}

		private void ConversationsIcon_PointerPressed(object sender, PointerRoutedEventArgs e)
		{
			ConversationsIcon.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0x00, 0xA2, 0xFF));
		}

		private void ConversationsIcon_PointerExited(object sender, PointerRoutedEventArgs e)
		{
			ConversationsIcon.Background = new SolidColorBrush(Colors.Transparent);
		}

		private void StoriesIcon_PointerPressed(object sender, PointerRoutedEventArgs e)
		{
			StoriesIcon.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0x00, 0xA2, 0xFF));
		}

		private void StoriesIcon_PointerExited(object sender, PointerRoutedEventArgs e)
		{
			StoriesIcon.Background = new SolidColorBrush(Colors.Transparent);
		}
	}
}
