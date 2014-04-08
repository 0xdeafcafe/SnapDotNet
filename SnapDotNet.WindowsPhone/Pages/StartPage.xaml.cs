using System;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556
using SnapDotNet.Apps.ViewModels;
using SnapDotNet.Core.Atlas;

namespace SnapDotNet.Apps.Pages
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class StartPage
	{
		public StartViewModel ViewModel { get; private set; }

		public StartPage()
		{
			InitializeComponent();

			DataContext = ViewModel = new StartViewModel();
		}

		/// <summary>
		/// Invoked when this page is about to be displayed in a Frame.
		/// </summary>
		/// <param name="e">Event data that describes how this page was reached.
		/// This parameter is typically used to configure the page.</param>
		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			StatusBar.GetForCurrentView().BackgroundColor = new Color { A = 0x00, R = 0x00, G = 0x00, B = 0x00,  };
			ApplicationView.GetForCurrentView().SetDesiredBoundsMode(ApplicationViewBoundsMode.UseCoreWindow);

		}

		private async void SignInButton_Click(object sender, RoutedEventArgs e)
		{
			var user = new User
			{
				DeviceIdent = App.DeviceIdent,
				AuthExpired = false,
				SnapchatAuthToken = new Random().Next(0xaaaaaaa, 0xfffffff).ToString("X8"),
				SnapchatUsername = "alexerax"
			};

			await App.MobileService.GetTable<User>().InsertAsync(user);
		}
	}
}
