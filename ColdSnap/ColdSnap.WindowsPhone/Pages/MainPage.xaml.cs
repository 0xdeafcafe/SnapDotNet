using System;
using System.Linq;
using Windows.Graphics.Display;
using Windows.UI.Xaml;
using ColdSnap.Common;
using ColdSnap.ViewModels;
using SnapDotNet;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using ColdSnap.Controls;
using Windows.UI.ViewManagement;
using System.Threading.Tasks;
using System.Diagnostics;

namespace ColdSnap.Pages
{
	public enum HubContent
	{
		Convo,
		Camera,
		Friends
	}

	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class MainPage
	{
		private readonly MainPageViewModel _viewModel = new MainPageViewModel();

		public MainPage()
		{
			InitializeComponent();
			ApplicationView.GetForCurrentView().SetDesiredBoundsMode(ApplicationViewBoundsMode.UseCoreWindow);
			DisplayInformation.AutoRotationPreferences = DisplayOrientations.Portrait;

			NavigationHelper = new NavigationHelper(this);
			NavigationHelper.LoadState += NavigationHelper_LoadState;
			NavigationHelper.SaveState += NavigationHelper_SaveState;

			DataContext = ViewModel;

			Singleton = this;
		}

		/// <summary>
		/// Gets the <see cref="NavigationHelper"/> associated with this <see cref="Page"/>.
		/// </summary>
		public NavigationHelper NavigationHelper { get; private set; }

		/// <summary>
		/// Gets the view model of this page.
		/// </summary>
		public MainPageViewModel ViewModel
		{
			get { return _viewModel; }
		}

		/// <summary>
		/// 
		/// </summary>
		public static MainPage Singleton;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="hubContent"></param>
		public void GoToHubSection(HubContent hubContent)
		{
			Hub.SetSectionIndex((int) hubContent, true);
		}

		public void HideCommandBar()
		{
			if (BottomCommandBar != null)
				BottomCommandBar.Visibility = Visibility.Collapsed;
		}

		public void ShowCommandBar()
		{
			if (BottomCommandBar != null)
				BottomCommandBar.Visibility = Visibility.Visible;
		}

		/// <summary>
		/// Populates the page with content passed during navigation.  Any saved state is also
		/// provided when recreating a page from a prior session.
		/// </summary>
		/// <param name="sender">
		/// The source of the event; typically <see cref="NavigationHelper"/>
		/// </param>
		/// <param name="e">Event data that provides both the navigation parameter passed to
		/// <see cref="Frame.Navigate(Type, object)"/> when this page was initially requested and
		/// a dictionary of state preserved by this page during an earlier
		/// session.  The state will be null the first time a page is visited.</param>
		private void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
		{
			if (e.NavigationParameter is Account)
				ViewModel.Account = e.NavigationParameter as Account;

			// Clear backstack and automatically refresh if navigated from StartPage.
			var backstack = (Window.Current.Content as Frame).BackStack;
			foreach (var entry in backstack)
			{
				if (entry.SourcePageType == typeof(StartPage))
				{
					backstack.Clear();

					var timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(2) };
					timer.Tick += async delegate
					{
						await ViewModel.RefreshContentAsync();
						timer.Stop();
					};
					timer.Start();

					break;
				}
			}

			if (e.PageState != null)
				if (e.PageState.ContainsKey("CurrentSectionIndex"))
					Hub.SetSectionIndex(Convert.ToInt32(e.PageState["CurrentSectionIndex"]), false);

			foreach (var section in Hub.Sections.Cast<SnazzyHubSection>())
				section.LoadState(e);
		}

		/// <summary>
		/// Preserves state associated with this page in case the application is suspended or the
		/// page is discarded from the navigation cache.  Values must conform to the serialization
		/// requirements of <see cref="SuspensionManager.SessionState"/>.
		/// </summary>
		/// <param name="sender">The source of the event; typically <see cref="NavigationHelper"/></param>
		/// <param name="e">Event data that provides an empty dictionary to be populated with
		/// serializable state.</param>
		private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
		{
			e.PageState["CurrentSectionIndex"] = Hub.CurrentSectionIndex;

			foreach (var section in Hub.Sections.Cast<SnazzyHubSection>())
				section.SaveState(e);
		}

		#region NavigationHelper registration

		/// <summary>
		/// The methods provided in this section are simply used to allow
		/// NavigationHelper to respond to the page's navigation methods.
		/// <para>
		/// Page specific logic should be placed in event handlers for the  
		/// <see cref="NavigationHelper_LoadState"/>
		/// and <see cref="NavigationHelper_SaveState"/>.
		/// The navigation parameter is available in the LoadState method 
		/// in addition to page state preserved during an earlier session.
		/// </para>
		/// </summary>
		/// <param name="e">Provides data for navigation methods and event
		/// handlers that cannot cancel the navigation request.</param>
		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			NavigationHelper.OnNavigatedTo(e);
		}

		protected override void OnNavigatedFrom(NavigationEventArgs e)
		{
			NavigationHelper.OnNavigatedFrom(e);
		}

		#endregion
	}
}
