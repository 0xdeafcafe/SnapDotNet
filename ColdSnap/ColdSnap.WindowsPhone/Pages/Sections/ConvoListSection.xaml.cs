using ColdSnap.Common;
using ColdSnap.ViewModels.Sections;
using SnapDotNet;
using Windows.UI.Xaml;

namespace ColdSnap.Pages.Sections
{
	public sealed partial class ConvoListSection
	{
		public ConvoListSection()
		{
			InitializeComponent();
			DataContext = new ConvoListSectionViewModel();
		}

		/// <summary>
		/// Gets the view model of this section.
		/// </summary>
		public ConvoListSectionViewModel ViewModel
		{
			get { return DataContext as ConvoListSectionViewModel; }
		}

		public override void LoadState(LoadStateEventArgs e)
		{
			if (e.NavigationParameter is Account)
				ViewModel.Account = e.NavigationParameter as Account;
		}

		public override void SaveState(SaveStateEventArgs e)
		{

		}

		private void OpenConversationButton_OnClick(object sender, RoutedEventArgs e)
		{
			Window.Current.Navigate(typeof(ConversationPage), ViewModel.Account);
		}

		private void OpenSettingsButton_Click(object sender, RoutedEventArgs e)
		{
			Window.Current.Navigate(typeof(SettingsPage), ViewModel.Account);
		}
	}
}
