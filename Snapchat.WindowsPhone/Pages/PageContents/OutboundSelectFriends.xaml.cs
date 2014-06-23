using Snapchat.ViewModels.PageContents;

namespace Snapchat.Pages.PageContents
{
	public sealed partial class OutboundSelectFriends
	{
		public OutboundSelectFriendsViewModel ViewModel { get; private set; }

		public OutboundSelectFriends()
		{
			InitializeComponent();
		}

		public void Reset()
		{
			DataContext = ViewModel = null;
		}

		public void Load()
		{
			DataContext = ViewModel = new OutboundSelectFriendsViewModel();
		}

		public void SelectAllFriends()
		{
			//foreach (var reciepicent in ViewModel.Friends
			//	.Where(reciepicent => reciepicent.Key is Friend ||
			//						  reciepicent.Key is SelectedRecent))
			//{
			//	// le code
			//	ViewModel.Friends[reciepicent.Key] = true;
			//}
		}

		public void DeSelectAllFriends()
		{
			//foreach (var reciepicent in ViewModel.Friends
			//	.Where(reciepicent => reciepicent.Key is Friend ||
			//						  reciepicent.Key is SelectedRecent))
			//{
			//	// le code
			//	ViewModel.Friends[reciepicent.Key] = false;
			//}
		}
	}
}
