using ColdSnap.Common;
using ColdSnap.ViewModels.Sections;
using System;
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

		private void OpenConversationButton_OnClick(object sender, RoutedEventArgs e)
		{
			Window.Current.Navigate(typeof(ConversationPage), ViewModel.Account);
		}
	}
}
