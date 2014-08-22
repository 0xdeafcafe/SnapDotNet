using ColdSnap.Common;
using ColdSnap.ViewModels.Sections;
using SnapDotNet;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;

namespace ColdSnap.Pages.Sections
{
	public sealed partial class FriendsSection
	{
		public FriendsSection()
		{
			InitializeComponent();
			DataContext = new FriendsSectionViewModel();
		}

		/// <summary>
		/// Gets the view model of this section.
		/// </summary>
		public FriendsSectionViewModel ViewModel
		{
			get { return DataContext as FriendsSectionViewModel; }
		}

		public override void LoadState(LoadStateEventArgs e)
		{
			if (e.NavigationParameter is Account)
				ViewModel.Account = e.NavigationParameter as Account;
		}

		public override void SaveState(SaveStateEventArgs e)
		{
			
		}

		private void AddFriendsButton_Tapped(object sender, TappedRoutedEventArgs e)
		{
			Window.Current.Navigate(typeof(ManageFriendsPage), ViewModel.Account);
		}
	}
}