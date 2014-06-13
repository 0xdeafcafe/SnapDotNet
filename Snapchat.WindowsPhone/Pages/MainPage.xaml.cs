using System.Linq;
using Windows.ApplicationModel;
using Windows.Phone.UI.Input;
using Windows.Storage.Streams;
using Windows.UI;
using Windows.UI.ViewManagement;
using Microsoft.Xaml.Interactivity;
using Snapchat.Attributes;
using Snapchat.Common;
using System;
using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Snapchat.Helpers;
using Snapchat.ViewModels;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Animation;
using SnapDotNet.Core.Miscellaneous.Extensions;
using SnapDotNet.Core.Snapchat.Models.New;

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
			Label = App.Strings.GetString("RefreshAppBarButtonLabel"),
			Command = new RelayCommand(App.UpdateSnapchatDataAsync)
		};

		private readonly AppBarButton _settingsAppBarButton = new AppBarButton
		{
			Label = App.Strings.GetString("SettingsAppBarButtonLabel"),
			Command = new RelayCommand(() =>
			{
				VisualStateManager.GoToState(VisualStateUtilities.FindNearestStatefulControl(Singleton.ScrollViewer), "Settings", true);
			})
		};

		private readonly AppBarButton _flipCameraAppBarButton = new AppBarButton
		{
			Icon = new BitmapIcon { UriSource = new Uri("ms-appx:///Assets/Icons/appbar.camera.flip.png") },
			Label = App.Strings.GetString("FlipCameraAppBarButtonLabel"),
			Command = new RelayCommand(async () =>
			{
				await MediaCaptureManager.ToggleCameraAsync();
				Singleton.UpdateBottomAppBar();
			})
		};

		private readonly AppBarButton _toggleFlashAppBarButton = new AppBarButton
		{
			Icon = new BitmapIcon { UriSource = FlashOnUri },
			Label = App.Strings.GetString("ToggleFlashAppBarButtonLabel"),
			Command = new RelayCommand(() =>
			{
				MediaCaptureManager.IsFlashEnabled = !MediaCaptureManager.IsFlashEnabled;
				// TODO: Change icon
			})
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

		private readonly AppBarButton _logoutAppBarButton = new AppBarButton
		{
			Icon = new SymbolIcon { Symbol = Symbol.LeaveChat },
			Label = App.Strings.GetString("LogOutAppBarButtonLabel"),
			Command = new RelayCommand(() =>
			{
				App.RootFrame.Navigate(typeof(StartPage));
			})
		};

		#endregion

		private static readonly SolidColorBrush AppBarBackgroundBrush = new SolidColorBrush(Color.FromArgb(0xFF, 0x3C, 0xB2, 0xE2));

		private double _previousScrollViewerOffset;
		private bool _goingToCamera;
		private AppBar _hiddenCommandBar;

		public MainPage()
		{
			InitializeComponent();
			Singleton = this;

			// Setup ALL the datas :D
			DataContext = ViewModel = new MainViewModel(ScrollViewer);

			// Delete the camera button tip if necessary.
			if (AppSettings.Get("FirstTime", true))
			{
				AppSettings.Set("FirstTime", false);
			}
			else
			{
				CameraPage.Children.Remove(FirstRunPrompt);
			}

			// Set up the scroll viewer
			ScrollViewer.ViewChanged += ScrollViewer_ViewChanged;
			ScrollViewer.ViewChanging += ScrollViewer_ViewChanging;
			PagesVisualStateGroup.CurrentStateChanged += delegate { UpdateBottomAppBar(); };
		}

		public static MainPage Singleton { get; private set; }
		public MainViewModel ViewModel { get; private set; }

		public void HideBottomAppBar()
		{
			_hiddenCommandBar = BottomAppBar;
			BottomAppBar = null;
		}

		public void RestoreBottomAppBar()
		{
			if (_hiddenCommandBar == null)
				return;

			BottomAppBar = _hiddenCommandBar;
			_hiddenCommandBar = null;
		}

		public void ShowConversation(ConversationResponse conversation)
		{
			ConvoPage.DataContext = new ConversationViewModel(conversation);
			VisualStateManager.GoToState(VisualStateUtilities.FindNearestStatefulControl(ScrollViewer), "Conversation", true);
		}

		public void GoToAddFriendsPage()
		{
			var timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(10) };
			timer.Tick += delegate
			{
				if (!ScrollViewer.ChangeView(CameraPage.ActualWidth * 3, null, null, false)) return;
				timer.Stop();
			};
			timer.Start();
		}

		protected override async void OnNavigatedTo(NavigationEventArgs e)
		{
			string destination = e.Parameter as string ?? "";
			//if (e.NavigationMode == NavigationMode.Back)
			//	destination = "Conversations";

			if (string.IsNullOrEmpty(destination))
				ConversationsPage.Opacity = 0;

			// Keep trying to change the ScrollViewer's view until it succeeds.
			var timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(100) };
			timer.Tick += delegate
			{
				if (!ScrollViewer.ChangeView(CameraPage.ActualWidth, null, null, true)) return;

				ViewModel.ActualWidth = ActualWidth;
				ConversationsPage.Opacity = 1;
				timer.Stop();

				
				switch (destination)
				{
					case "Conversations":
						ScrollViewer.ChangeView(0, null, null, true);
						UpdateBottomAppBar();
						BottomAppBar.ClosedDisplayMode = AppBarClosedDisplayMode.Minimal;
						break;
				}
			};
			timer.Start();

			// Start the camera.
			if (!DesignMode.DesignModeEnabled)
			{
				try
				{
					await MediaCaptureManager.StartPreviewAsync(CapturePreview);
				}
				catch { }
				var storyboard = CapturePreview.Resources["FadeInStoryboard"] as Storyboard;
				if (storyboard != null)
					storyboard.Begin();
			}

			// Load data
			if (!App.SnapchatManager.Loaded)
				await App.SnapchatManager.LoadAsync();

			// Setup hardware events
			HardwareButtons.BackPressed += HardwareButtons_BackPressed;
			HardwareButtons.CameraPressed += HardwareButtons_CameraPressed;

			// Set the status bar
			SetStatusBar();
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

				case "Friends":
					ScrollViewer.HorizontalSnapPointsType = SnapPointsType.None;
					ScrollViewer.ChangeView(CameraPage.ActualWidth, null, null, false); // go to camera
					e.Handled = true;
					break;

				case "AddFriends":
					ScrollViewer.ChangeView(CameraPage.ActualWidth * 2, null, null, false); // go to friends
					e.Handled = true;
					break;

				case "Conversation":
				case "Settings":
					// Determine the page that's currently in view.
					var pageIndex = (int) Math.Round(ScrollViewer.HorizontalOffset / CameraPage.ActualWidth);
					var frameworkElement = PagesContainer.Children[pageIndex] as FrameworkElement;
					if (frameworkElement != null)
					{
						var currentPage = frameworkElement.Tag.ToString();

						// Change visual state to current page.
						VisualStateManager.GoToState(VisualStateUtilities.FindNearestStatefulControl(ScrollViewer), currentPage, true);
					}
					UpdateBottomAppBar();
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

			// Fix scroll viewer after scrolling all the way back to camera from the manage friends page.
			if (pageIndex != 1) return;
			_goingToCamera = false;
			if (!e.IsIntermediate)
				ScrollViewer.HorizontalSnapPointsType = SnapPointsType.MandatorySingle;
		}

		private static void SetStatusBar()
		{
			ApplicationView.GetForCurrentView().SetDesiredBoundsMode(ApplicationViewBoundsMode.UseCoreWindow);

			var statusBar = StatusBar.GetForCurrentView();
			statusBar.BackgroundOpacity = 0.0f;
			statusBar.BackgroundColor = Colors.Transparent;
			statusBar.ForegroundColor = Colors.White;
		}

		private void UpdateBottomAppBar()
		{
			if (BottomAppBar == null)
				BottomAppBar = new CommandBar { Background = AppBarBackgroundBrush };
			var appBar = BottomAppBar as CommandBar;
			if (appBar == null) return;

			appBar.Foreground = new SolidColorBrush(Colors.White);

			var primaryCommands = new Collection<ICommandBarElement>();
			var secondaryCommands = new Collection<ICommandBarElement>();
			var displayMode = appBar.ClosedDisplayMode;

			string currentState = null;
			if (PagesVisualStateGroup.CurrentState != null)
			{
				currentState = PagesVisualStateGroup.CurrentState.Name;
				switch (currentState)
				{
					case "Conversations":
						secondaryCommands.Add(_downloadAllSnapsAppBarButton);
						displayMode = AppBarClosedDisplayMode.Minimal;
						appBar.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0x3C, 0xB2, 0xE2));
						break;

					case "Camera":
						if (MediaCaptureManager.HasFrontCamera)
							primaryCommands.Add(_flipCameraAppBarButton);

						if (MediaCaptureManager.IsFlashSupported)
							primaryCommands.Add(_toggleFlashAppBarButton);

						secondaryCommands.Add(_importPictureAppBarButton);
						displayMode = AppBarClosedDisplayMode.Compact;
						appBar.Background = new SolidColorBrush(Color.FromArgb(0x66, 0x33, 0x33, 0x33));
						break;

					case "Friends":
						displayMode = AppBarClosedDisplayMode.Minimal;
						appBar.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0x9b, 0x55, 0xa0));
						break;

					case "AddFriends":
						displayMode = AppBarClosedDisplayMode.Minimal;
						appBar.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0xFF, 0x80, 0x00));
						break;

					case "Settings":
						primaryCommands.Add(_logoutAppBarButton);
						displayMode = AppBarClosedDisplayMode.Compact;
						appBar.Background = new SolidColorBrush(Colors.Black);
						break;

					case "Conversation":
						displayMode = AppBarClosedDisplayMode.Compact;
						appBar.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0xE7, 0x3B, 0x45));
						break;
				}
			}

			// Add global commands.
			if (currentState != "Settings")
			{
				secondaryCommands.Add(_refreshAppBarButton);
				secondaryCommands.Add(_settingsAppBarButton);
			}

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

		private void CapturePhotoButton_Tapped(object sender, TappedRoutedEventArgs e)
		{
			// TODO: Take snapshot
		}

		private void CapturePhotoButton_Holding(object sender, HoldingRoutedEventArgs e)
		{
			// Play storyboard that shows a red ring circling the capture button
		}

		private void CapturePhotoButton_PointerPressed(object sender, PointerRoutedEventArgs e)
		{
			// TODO: Start recording
		}

		#region Snap Media Viewer

		public async void ShowSnapMedia(Snap snap)
		{
			// Check if we have the snap blob in storage
			if (!snap.HasMedia || snap.Status != SnapStatus.Delivered || !snap.IsIncoming) return;

			// Set Snap Media
			if (snap.IsImage)
				SnapMediaImage.Source = (await snap.OpenSnapBlobAsync()).ToBitmapImage();
			else
			{
				SnapMediaVideo.SetSource((await (await snap.OpenSnapBlobAsync()).ToInMemoryRandomAccessStream()), "video/mp4");
				SnapMediaVideo.Play();
			}

			// Reveal (hue) UI
			SnapMediaOverlayGrid.Visibility = Visibility.Visible;
			HideBottomAppBar();
		}

		public void HideSnapMedia()
		{
			if (SnapMediaOverlayGrid.Visibility != Visibility.Visible) return;

			// Snap is visible, lets clean up
			SnapMediaImage.Source = null;
			SnapMediaOverlayGrid.Visibility = Visibility.Collapsed;
			RestoreBottomAppBar();
		}

		#endregion
	}
}
