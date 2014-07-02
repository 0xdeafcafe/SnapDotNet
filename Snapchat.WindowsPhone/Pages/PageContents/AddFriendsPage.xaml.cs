using Snapchat.ViewModels.PageContents;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Contacts;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Snapchat.Pages.PageContents
{
	public sealed partial class AddFriendsPage : UserControl
	{
		public AddFriendsPage()
		{
			this.InitializeComponent();
			DataContext = ViewModel = new AddFriendsViewModel();
		}

		public AddFriendsViewModel ViewModel { get; private set; }

		private void SearchBox_GotFocus(object sender, RoutedEventArgs e)
		{
			MainPage.Singleton.HideBottomAppBar();
		}

		private void SearchBox_LostFocus(object sender, RoutedEventArgs e)
		{
			MainPage.Singleton.RestoreBottomAppBar();
		}

		private async void FindFriendsButton_Tapped(object sender, TappedRoutedEventArgs e)
		{
			var store = await ContactManager.RequestStoreAsync();
			var contacts = await store.FindContactsAsync();
			foreach (var contact in contacts)
			{
				string displayName = contact.DisplayName;
				var phoneNumbers = contact.Phones;
			}

			// TODO: Invoke FindFriendsAsync in Snapchat Manager
		}
	}
}
