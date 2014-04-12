using System;
using System.Linq;
using SnapDotNet.Core.Snapchat.Models;

namespace SnapDotNet.Apps.ViewModels.SignedIn
{
	public class FriendViewModel
		: ViewModelBase
	{
		public Friend SelectedFriend
		{
			get { return _selectedFriend; }
			set
			{
				SetField(ref _selectedFriend, value);

				NotifyPropertyChanged("SelectedActivity");
				NotifyPropertyChanged("SelectedStory");
			}
		}
		private Friend _selectedFriend;

		public PublicActivity SelectedActivity
		{
			get
			{
				try
				{
					PublicActivity output;
					App.SnapChatManager.PublicActivities.TryGetValue(SelectedFriend.Name, out output);
					return output;
				}
				catch (Exception)
				{
					return null;
				}
			}
		}

		public FriendStory SelectedStory
		{
			get
			{
				var friendStory = App.SnapChatManager.Stories.FriendStories.FirstOrDefault(f => f.Username == SelectedFriend.Name);
				return friendStory;
			}
		}
	}
}
