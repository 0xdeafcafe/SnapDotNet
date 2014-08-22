using System;
using Windows.UI.Xaml;
using ColdSnap.Common;
using ColdSnap.ViewModels;
using SnapDotNet;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using ColdSnap.ViewModels.Sections;
using ColdSnap.Controls;

namespace ColdSnap.Pages
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class MainPage
	{
		private readonly MainPageViewModel _viewModel = new MainPageViewModel();

		public MainPage()
		{
			InitializeComponent();

			NavigationHelper = new NavigationHelper(this);
			NavigationHelper.LoadState += NavigationHelper_LoadState;
			NavigationHelper.SaveState += NavigationHelper_SaveState;

			DataContext = ViewModel;
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

			if (e.PageState != null)
			{
				if (e.PageState.ContainsKey("CurrentSectionIndex"))
					this.hub.SetSectionIndex(Convert.ToInt32(e.PageState["CurrentSectionIndex"]));
			}

			foreach (SnazzyHubSection section in this.hub.Sections)
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
			e.PageState["CurrentSectionIndex"] = this.hub.CurrentSectionIndex;

			foreach (SnazzyHubSection section in this.hub.Sections)
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
