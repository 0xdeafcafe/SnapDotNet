using SnapDotNet.Core.Snapchat.Models.New;
using System.Collections.ObjectModel;

namespace Snapchat.ViewModels.PageContents
{
	public class AddFriendsViewModel
		 : BaseViewModel
	{
		public ObservableCollection<AddedFriend> FriendsWhoAddedMe
		{
			get
			{
				var addedFriends = App.SnapchatManager.Account.AddedFriends;
				return addedFriends;
			}
		}
    }
}
