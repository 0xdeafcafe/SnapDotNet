using ColdSnap.ViewModels.Sections;

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
	}
}