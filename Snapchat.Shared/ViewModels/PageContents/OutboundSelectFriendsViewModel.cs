using System;
using System.Collections.Generic;
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
		public OutboundSelectFriendsViewModel(byte[] imageData)
		{
			_recipientList.CollectionChanged += delegate
			{
				ExplicitOnNotifyPropertyChanged("SelectedRecipients");
			};

			ImageData = imageData;

			var friends = App.SnapchatManager.Account.Friends.Where(f => f.Type != FriendRequestState.Blocked).Select(friend => new SelectedFriend { Friend = friend }).Cast<SelectedItem>().ToList();
			friends.AddRange(App.SnapchatManager.Account.RecentFriends.Select(recent => new SelectedRecent { RecentName = recent }));
			friends.Add(new SelectedStory { StoryName = "My Story" });

			RecipientList = SelectFriendskeyGroup<SelectedItem>.CreateGroups(friends, new CultureInfo("en"));
		}
		
		public ObservableCollection<SelectFriendskeyGroup<SelectedItem>> RecipientList
		{
			get { return _recipientList; }
			set
			{
				TryChangeValue(ref _recipientList, value);
				ExplicitOnNotifyPropertyChanged("SelectedRecipients");
			}
		}
		private ObservableCollection<SelectFriendskeyGroup<SelectedItem>> _recipientList = new ObservableCollection<SelectFriendskeyGroup<SelectedItem>>();

		public Byte[] ImageData
		{
			get { return _imageData; }
			set { TryChangeValue(ref _imageData, value); }
		}
		private Byte[] _imageData;

		public String SelectedRecipients
		{
			get
			{
				var friends = new HashSet<String>();
				foreach (var recipient in RecipientList.SelectMany(recipientGroup => recipientGroup).Where(recipient => recipient.Selected))
				{
					if (recipient is SelectedStory)
						friends.Add((recipient as SelectedStory).StoryName);
					else if (recipient is SelectedRecent)
						friends.Add((recipient as SelectedRecent).RecentName);
					else if (recipient is SelectedFriend)
						friends.Add((recipient as SelectedFriend).Friend.FriendlyName);
				}
				return String.Join(", ", friends.ToArray());
			}
		}
	}
}
