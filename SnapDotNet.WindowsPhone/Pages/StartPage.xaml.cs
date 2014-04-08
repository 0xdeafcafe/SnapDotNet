using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556
using SnapDotNet.Core.Atlas;

namespace SnapDotNet.Apps.Pages
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class StartPage
	{
		public StartPage()
		{
			InitializeComponent();
		}

		/// <summary>
		/// Invoked when this page is about to be displayed in a Frame.
		/// </summary>
		/// <param name="e">Event data that describes how this page was reached.
		/// This parameter is typically used to configure the page.</param>
		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
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
