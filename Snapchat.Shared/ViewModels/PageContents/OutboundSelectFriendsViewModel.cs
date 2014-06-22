using System;
using System.Collections.Generic;
using System.Linq;
using SnapDotNet.Core.Miscellaneous.CustomTypes;
using SnapDotNet.Core.Snapchat.Models.AppSpecific;
using SnapDotNet.Core.Snapchat.Models.New;

namespace Snapchat.ViewModels.PageContents
{
	public class OutboundSelectFriendsViewModel
		: BaseViewModel
	{
		public OutboundSelectFriendsViewModel()
		{
			_selectedFriends.MapChanged += delegate { ExplicitOnNotifyPropertyChanged("SelectedFriends"); };

			foreach (var friend in App.SnapchatManager.Account.Friends.Where(f => f.Type != FriendRequestState.Blocked))
				SelectedFriends.Add(new KeyValuePair<object, bool>(friend, false));

			foreach (var recent in App.SnapchatManager.Account.RecentFriends)
				SelectedFriends.Add(new KeyValuePair<object, bool>(new SelectedRecent { RecentName = recent }, false));

			SelectedFriends.Add(new KeyValuePair<object, bool>(new SelectedStory { StoryName = "My Story" }, false));
		}

		public ObservableDictionary<object, Boolean> SelectedFriends
		{
			get { return _selectedFriends; }
			set { TryChangeValue(ref _selectedFriends, value); }
		}
		private ObservableDictionary<object, Boolean> _selectedFriends = new ObservableDictionary<object, Boolean>(); 
	}
}
