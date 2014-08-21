using ColdSnap.Common;
using System;
using Windows.UI.Xaml;

namespace ColdSnap.Pages.Sections
{
	public sealed partial class ConvoListSection
	{
		public ConvoListSection()
		{
			InitializeComponent();
		}

		private void OpenConversationButton_OnClick(object sender, RoutedEventArgs e)
		{
			Window.Current.Navigate(typeof(ConversationPage));
		}
	}
}
