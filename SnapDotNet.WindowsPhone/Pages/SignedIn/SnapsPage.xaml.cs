using System.Diagnostics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;
using SnapDotNet.Apps.Attributes;
using SnapDotNet.Apps.Common;
using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using SnapDotNet.Apps.ViewModels.SignedIn;
using SnapDotNet.Core.Miscellaneous.Extensions;
using SnapDotNet.Core.Snapchat.Models;

namespace SnapDotNet.Apps.Pages.SignedIn
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	[RequiresAuthentication]
	public sealed partial class SnapsPage
	{
		private readonly NavigationHelper _navigationHelper;
		private readonly ObservableDictionary _defaultViewModel = new ObservableDictionary();

		public SnapsViewModel ViewModel { get; private set; }

		public SnapsPage()
		{
			InitializeComponent();

			DataContext = ViewModel = new SnapsViewModel();

			_navigationHelper = new NavigationHelper(this);
			_navigationHelper.LoadState += NavigationHelper_LoadState;
			_navigationHelper.SaveState += NavigationHelper_SaveState;
			
#if !DEBUG
			Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().IsScreenCaptureEnabled = false;
#endif

			_holdingTimer = new DispatcherTimer {Interval = new TimeSpan(0, 0, 0, 0, 200)};
			_holdingTimer.Tick += HoldingTimerOnTick;

			// Make this default later
			MediaGrid.Visibility = Visibility.Collapsed;
		}
		
		#region NavigationHelper registration

		/// <summary>
		/// Gets the <see cref="NavigationHelper"/> associated with this <see cref="Page"/>.
		/// </summary>
		public NavigationHelper NavigationHelper
		{
			get { return _navigationHelper; }
		}

		/// <summary>
		/// Gets the view model for this <see cref="Page"/>.
		/// This can be changed to a strongly typed view model.
		/// </summary>
		public ObservableDictionary DefaultViewModel
		{
			get { return _defaultViewModel; }
		}

		/// <summary>
		/// Populates the page with content passed during navigation.  Any saved state is also
		/// provided when recreating a page from a prior session.
		/// </summary>
		/// <param name="sender">
		/// The source of the event; typically <see cref="NavigationHelper"/>
		/// </param>
		/// <param name="e">Event data that provides both the navigation parameter passed to
		/// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested and
		/// a dictionary of state preserved by this page during an earlier
		/// session.  The state will be null the first time a page is visited.</param>
		private static void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
		{
		}

		/// <summary>
		/// Preserves state associated with this page in case the application is suspended or the
		/// page is discarded from the navigation cache.  Values must conform to the serialization
		/// requirements of <see cref="SuspensionManager.SessionState"/>.
		/// </summary>
		/// <param name="sender">The source of the event; typically <see cref="NavigationHelper"/></param>
		/// <param name="e">Event data that provides an empty dictionary to be populated with
		/// serializable state.</param>
		private static void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
		{
		}

		/// <summary>
		/// The methods provided in this section are simply used to allow
		/// NavigationHelper to respond to the page's navigation methods.
		/// <para>
		/// Page specific logic should be placed in event handlers for the  
		/// <see cref="NavigationHelper.LoadState"/>
		/// and <see cref="NavigationHelper.SaveState"/>.
		/// The navigation parameter is available in the LoadState method 
		/// in addition to page state preserved during an earlier session.
		/// </para>
		/// </summary>
		/// <param name="e">Provides data for navigation methods and event
		/// handlers that cannot cancel the navigation request.</param>
		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			_navigationHelper.OnNavigatedTo(e);
		}

		protected override void OnNavigatedFrom(NavigationEventArgs e)
		{
			_navigationHelper.OnNavigatedFrom(e);
		}

		#endregion
		
		private readonly DispatcherTimer _holdingTimer;
		private bool _isMediaOpen;
		private bool _isFingerDown;
		private double _scrollYIndex;
		private Snap _relevantSnap;

		private async void ButtonSnap_Holding(object sender, HoldingRoutedEventArgs e)
		{
			Debug.WriteLine("le holding");

			if (_isMediaOpen)
			{
				// Hide media
				DisposeMediaTidily();
				return;
			}

			var button = sender as Button;
			if (button == null) return;
			var snap = button.DataContext as Snap;
			if (snap == null) return;
			_relevantSnap = snap;

			// Show Media
			_isMediaOpen = true;
			MediaGrid.Visibility = Visibility.Visible;
			if (BottomAppBar != null) BottomAppBar.Visibility = Visibility.Collapsed;
			var media = await _relevantSnap.GetSnapBlobAsync();

			if (_relevantSnap.MediaType == MediaType.Image ||
				_relevantSnap.MediaType == MediaType.FriendRequestImage)
			{
				MediaImage.Source = media.ToBitmapImage();
			}
		}

		private void DisposeMediaTidily()
		{
			_isMediaOpen = false;
			MediaGrid.Visibility = Visibility.Collapsed;
			if (BottomAppBar != null) BottomAppBar.Visibility = Visibility.Visible;
			MediaImage.Source = null;
		}

		private void ButtonSnap_OnManipulationStarted(object sender, ManipulationStartedRoutedEventArgs e)
		{
			Debug.WriteLine("ButtonSnap_OnManipulationStarted");
			var button = sender as Button;
			if (button == null) return;
			var snap = button.DataContext as Snap;
			if (snap == null) return;
			_relevantSnap = snap;

			_isFingerDown = true;
			//_scrollYIndex = ScrollViewer.VerticalOffset;
			_holdingTimer.Start();
		}

		private void ButtonSnap_OnManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
		{
			Debug.WriteLine("ButtonSnap_OnManipulationCompleted");
			_isFingerDown = false;
			if (!_isMediaOpen)
			{
				_isMediaOpen = false;
				return;
			}

			DisposeMediaTidily();
		}

		private async void HoldingTimerOnTick(object sender, object o)
		{
			Debug.WriteLine("HoldingTimerOnTick");
			_holdingTimer.Stop();
			if (!_isFingerDown)
				return;

			Debug.WriteLine("HoldingTimerOnTick :: _isFingerDown");

			//var diff = _scrollYIndex - ScrollViewer.VerticalOffset;
			//if (diff < -10 || diff > 10)
			//	return;

			// Start Media
			_isMediaOpen = true;
			MediaGrid.Visibility = Visibility.Visible;
			if (BottomAppBar != null) BottomAppBar.Visibility = Visibility.Collapsed;

			var media = await _relevantSnap.GetSnapBlobAsync();

			if (_relevantSnap.MediaType == MediaType.Image ||
				_relevantSnap.MediaType == MediaType.FriendRequestImage)
			{
				MediaImage.Source = media.ToBitmapImage();
			}
		}
	}
}
