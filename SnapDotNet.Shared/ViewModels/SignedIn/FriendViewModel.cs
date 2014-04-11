using SnapDotNet.Core.Snapchat.Models;

namespace SnapDotNet.Apps.ViewModels.SignedIn
{
	public class FriendViewModel
		: ViewModelBase
	{
		public Friend SelectedFriend
		{
			get { return _selectedFriend; }
			set { SetField(ref _selectedFriend, value); }
		}
		private Friend _selectedFriend;

		public PublicActivity SelectedActivity
		{
			get
			{
				PublicActivity publicActivity;
				App.SnapChatManager.PublicActivities.TryGetValue(SelectedFriend.Name, out publicActivity);
				return publicActivity;
			}
		}
	}
}
