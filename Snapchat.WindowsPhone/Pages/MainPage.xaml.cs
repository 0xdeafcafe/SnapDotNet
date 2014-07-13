using System.Linq;
using Windows.ApplicationModel;
using Windows.Phone.UI.Input;
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
using Snapchat.Models;
using Snapchat.ViewModels;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Animation;
using Snapchat.ViewModels.PageContents;
using Windows.Storage.Pickers;
using Windows.Storage;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;
using SnapDotNet.Core.Miscellaneous.Extensions;

namespace Snapchat.Pages
{
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
			Command = new RelayCommand(() => VisualStateManager.GoToState(VisualStateUtilities.FindNearestStatefulControl(Singleton.ScrollViewer), "Settings", true))
		};

		private readonly AppBarButton _flipCameraAppBarButton = new AppBarButton
		{
			Icon = new BitmapIcon { UriSource = new Uri("ms-appx:///Assets/Icons/appbar.camera.flip.png") },
			Label = App.Strings.GetString("FlipCameraAppBarButtonLabel")
		};
		
		private readonly AppBarButton _toggleFlashAppBarButton = new AppBarButton
		{
			Icon = new BitmapIcon { UriSource = FlashOnUri },
			Label = App.Strings.GetString("ToggleFlashAppBarButtonLabel"),
			Command = new RelayCommand(() =>
			{
				App.Camera.IsFlashEnabled = !App.Camera.IsFlashEnabled;
				// TODO: Change icon
			})
		};

		private readonly AppBarButton _downloadAllSnapsAppBarButton = new AppBarButton
		{
			Label = App.Strings.GetString("DownloadAllSnapsAppBarButtonLabel")
		};

		private readonly AppBarButton _importPictureAppBarButton = new AppBarButton
		{
			Label = App.Strings.GetString("ImportPictureAppBarButtonLabel"),
			Command = new RelayCommand(() =>
			{
				FileOpenPicker picker = new FileOpenPicker();
				picker.FileTypeFilter.Add(".jpg");
				picker.FileTypeFilter.Add(".bmp");
				picker.FileTypeFilter.Add(".png");
				picker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
				picker.ViewMode = PickerViewMode.Thumbnail;
				picker.PickSingleFileAndContinue();
			})
		};

		private readonly AppBarButton _logoutAppBarButton = new AppBarButton
		{
			Icon = new SymbolIcon { Symbol = Symbol.LeaveChat },
			Label = App.Strings.GetString("LogOutAppBarButtonLabel"),
			Command = new RelayCommand(() => App.RootFrame.Navigate(typeof(StartPage)))
		};

		#endregion

		private static readonly SolidColorBrush AppBarBackgroundBrush = new SolidColorBrush(Color.FromArgb(0xFF, 0x3C, 0xB2, 0xE2));

		private double _previousScrollViewerOffset;
		private bool _goingToCamera;
		private AppBar _hiddenCommandBar;

		public MainPage()
		{
			InitializeComponent();
			NavigationCacheMode = NavigationCacheMode.Required;
			Singleton = this;

			// Setup ALL the datas :D
			DataContext = ViewModel = new MainViewModel(ScrollViewer);

			// Set the Camera Tip
			if (!AppSettings.Get("FirstTime", true))
				CameraPage.Children.Remove(FirstRunPrompt);

			// Set up the scroll viewer
			ScrollViewer.ViewChanged += ScrollViewer_ViewChanged;
			ScrollViewer.ViewChanging += ScrollViewer_ViewChanging;
			PagesVisualStateGroup.CurrentStateChanged += delegate { UpdateBottomAppBar(); };

			Loaded += delegate
			{
				App.Camera.SetPreviewSourceAsync(CapturePreview);
			};
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

		public void ShowConversation(Conversation conversation)
		{
			ConvoPage.DataContext = new ConversationViewModel(conversation);
			ConvoPage.Load();
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

		public void GoToOutboundFriendSelection(byte[] imageData)
		{
			OutboundSelectFriendsPage.Load(imageData);
			VisualStateManager.GoToState(this, "OutboundSelectFriends", true);
		}

		protected override async void OnNavigatedTo(NavigationEventArgs e)
		{
			var destination = e.Parameter as string ?? "";
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
						if (BottomAppBar != null) BottomAppBar.ClosedDisplayMode = AppBarClosedDisplayMode.Minimal;
						break;
				}
			};
			timer.Start();

			// Start the camera.
			if (!DesignMode.DesignModeEnabled)
			{
				var storyboard = CapturePreview.Resources["FadeInStoryboard"] as Storyboard;
				if (storyboard != null)
					storyboard.Begin();
			}

			// Load data
			if (!App.SnapchatManager.Loaded)
				App.SnapchatManager.LoadAsync();

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
			await App.Camera.StopPreviewAsync();
		}

		private void HardwareButtons_CameraPressed(object sender, CameraEventArgs e)
		{
			if (PagesVisualStateGroup.CurrentState == null)
				return;

			var currentState = PagesVisualStateGroup.CurrentState.Name;
			if (currentState == "Camera")
			{
				CapturePhotoButton_Tapped(CapturePhotoButton, null);
			}
			else
			{
				ScrollViewer.HorizontalSnapPointsType = SnapPointsType.None;
				_goingToCamera = true;
				ScrollViewer.ChangeView(CameraPage.ActualWidth, null, null, false); // go to camera
			}
		}

		private async void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
		{
			if (PagesVisualStateGroup.CurrentState == null)
				return;

			if (SnapMediaOverlayGrid.Visibility == Visibility.Visible)
			{
				// Enable scrollviewer Snap, then handle
				e.Handled = true;
				ScrollViewer.IsEnabled = true;
				return;
			}

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

				case "OutboundSelectFriends":
					OutboundSelectFriendsPage.Reset();
					VisualStateManager.GoToState(VisualStateUtilities.FindNearestStatefulControl(ScrollViewer), "Preview", true);
					UpdateBottomAppBar();
					await StatusBar.GetForCurrentView().ShowAsync();
					e.Handled = true;
					break;

				case "Conversation":
				case "Settings":
				case "Preview":
					// Determine the page that's currently in view.
					var pageIndex = (int)Math.Round(ScrollViewer.HorizontalOffset / CameraPage.ActualWidth);
					var frameworkElement = PagesContainer.Children[pageIndex] as FrameworkElement;
					if (frameworkElement != null)
					{
						var currentPage = frameworkElement.Tag.ToString();

						// Change visual state to current page.
						VisualStateManager.GoToState(VisualStateUtilities.FindNearestStatefulControl(ScrollViewer), currentPage, true);
					}
					UpdateBottomAppBar();
					await StatusBar.GetForCurrentView().ShowAsync();
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
			var pageIndex = (int)Math.Round(ScrollViewer.HorizontalOffset / CameraPage.ActualWidth);
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
						_flipCameraAppBarButton.Command = new RelayCommand(ToggleCamera);
						primaryCommands.Add(_flipCameraAppBarButton);

						if (App.Camera.HasFrontCamera)
							primaryCommands.Add(_flipCameraAppBarButton);

						if (App.Camera.IsFlashSupported)
							primaryCommands.Add(_toggleFlashAppBarButton);

						secondaryCommands.Add(_importPictureAppBarButton);
						displayMode = AppBarClosedDisplayMode.Compact;
						appBar.Background = new SolidColorBrush(Color.FromArgb(0x66, 0x33, 0x33, 0x33));
						break;

					case "OutboundSelectFriends":
					case "Preview":
						BottomAppBar = null;
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
						displayMode = AppBarClosedDisplayMode.Minimal;
						appBar.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0xD0, 0x2D, 0x01));
						break;

					case "Conversation":
						displayMode = AppBarClosedDisplayMode.Minimal;
						appBar.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0xE7, 0x3B, 0x45));
						break;
				}
			}

			// Add global commands.
			if (currentState != "Settings" && currentState != "Preview" && currentState != "OutboundSelectFriends")
			{
				secondaryCommands.Add(_refreshAppBarButton);
				secondaryCommands.Add(_settingsAppBarButton);
			}

			if (BottomAppBar == null)
			{
				appBar.PrimaryCommands.Clear();
				appBar.SecondaryCommands.Clear();
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

		public async Task ImportPhotoAsync(StorageFile file)
		{
			if (file == null) return;

			VisualStateManager.GoToState(VisualStateUtilities.FindNearestStatefulControl(ScrollViewer), "Preview", true);
			PreviewPage.Reset();

			var stream = await file.OpenAsync(FileAccessMode.Read);
			var bmp = new BitmapImage();
			await bmp.SetSourceAsync(stream);

			PreviewPage.Load(new PreviewViewModel(bmp));
		}

		private async void CapturePhotoButton_Tapped(object sender, TappedRoutedEventArgs e)
		{
			VisualStateManager.GoToState(VisualStateUtilities.FindNearestStatefulControl(ScrollViewer), "Preview", true);
			PreviewPage.Reset();

			// Remove the Camera Tip
			AppSettings.Set("FirstTime", false);
			CameraPage.Children.Remove(FirstRunPrompt);

			var writeableBitmap = await App.Camera.CapturePhotoAsync();
			PreviewPage.Load(new PreviewViewModel(writeableBitmap));
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

			// Block ScrollViewer
			ScrollViewer.IsEnabled = false;
		}

		public void HideSnapMedia()
		{
			if (SnapMediaOverlayGrid.Visibility != Visibility.Visible) return;

			// Snap is visible, lets clean up
			SnapMediaImage.Source = null;
			SnapMediaOverlayGrid.Visibility = Visibility.Collapsed;
			RestoreBottomAppBar();

			// Un-Block ScrollViewer
			ScrollViewer.IsEnabled = true;
		}

		#endregion

		private void CameraPage_OnDoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
		{
			ToggleCamera();
		}

		private async void ToggleCamera()
		{
			await App.Camera.FlipCameraAsync();
			Singleton.UpdateBottomAppBar();
		}

		public void GoToVisualState(string stateName)
		{
			VisualStateManager.GoToState(this, stateName, true);
		}
	}
}
