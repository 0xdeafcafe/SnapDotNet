using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using Snapchat.CustomTypes;
using SnapDotNet.Core.Snapchat.Models.AppSpecific;
using SnapDotNet.Core.Snapchat.Models.New;

namespace Snapchat.ViewModels.PageContents
{
	public class OutboundSelectFriendsViewModel
		: BaseViewModel
	{
		public OutboundSelectFriendsViewModel()
		{
			_recipientList.CollectionChanged += delegate { ExplicitOnNotifyPropertyChanged("SelectedFriends"); };

			var friends = App.SnapchatManager.Account.Friends.Where(f => f.Type != FriendRequestState.Blocked).Select(friend => new SelectedFriend { Friend = friend }).Cast<SelectedItem>().ToList();
			friends.AddRange(App.SnapchatManager.Account.RecentFriends.Select(recent => new SelectedRecent { RecentName = recent }));
			friends.Add(new SelectedStory { StoryName = "My Story" });

			RecipientList = SelectFriendskeyGroup<SelectedItem>.CreateGroups(friends, new CultureInfo("en"));
		}
		
		public ObservableCollection<SelectFriendskeyGroup<SelectedItem>> RecipientList
		{
			get { return _recipientList; }
			set { TryChangeValue(ref _recipientList, value); }
		}
		private ObservableCollection<SelectFriendskeyGroup<SelectedItem>> _recipientList = new ObservableCollection<SelectFriendskeyGroup<SelectedItem>>();
	}
}
