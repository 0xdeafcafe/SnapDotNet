using System.Diagnostics;
using System.Linq;
using Windows.UI.Xaml;
using Snapchat.CustomTypes;
using Snapchat.ViewModels.PageContents;
using SnapDotNet.Core.Snapchat.Models.AppSpecific;

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

		private bool _isInMassOperation;
		private bool _isCheckingOtherShit;
		public void SelectAllFriends()
		{
			_isInMassOperation = true;
			foreach (var recipient in ViewModel.RecipientList.Where(r => r.GroupType != GroupType.Stories).SelectMany(recipientGroup => recipientGroup))
				recipient.Selected = true;
			_isInMassOperation = false;

			ViewModel.ExplicitOnNotifyPropertyChanged("SelectedRecipients");
		}

		public void DeSelectAllFriends()
		{
			_isInMassOperation = true;
			foreach (var recipient in ViewModel.RecipientList.Where(r => r.GroupType != GroupType.Stories).SelectMany(recipientGroup => recipientGroup))
				recipient.Selected = false;
			_isInMassOperation = false;

			ViewModel.ExplicitOnNotifyPropertyChanged("SelectedRecipients");
		}

		private void SelectionCheckBox_OnCheckChanged(object sender, RoutedEventArgs e)
		{
			// TODO: Fix this bugg shitfest

			var element = sender as FrameworkElement;
			if (element == null) return;
			var item = element.DataContext as SelectedItem;
			if (item == null) return;

			if (_isCheckingOtherShit) return;
			if (_isInMassOperation) return;

			item.Selected = !item.Selected;
			ViewModel.ExplicitOnNotifyPropertyChanged("SelectedRecipients");
			_isCheckingOtherShit = true;

			if (item is SelectedRecent)
			{
				// Do friend processing
				CheckYo((item as SelectedRecent).RecentName, GroupType.Friends, item.Selected);
			}
			else if (item is SelectedFriend)
			{
				// Do recent processing
				CheckYo((item as SelectedFriend).Friend.FriendlyName, GroupType.Recents, item.Selected);
			}

			_isCheckingOtherShit = false;
		}

		private void CheckYo(string name, GroupType groupType, bool selectedState)
		{
			foreach (var recipient in ViewModel.RecipientList.Where(r => r.GroupType == groupType).SelectMany(recipientGroup => recipientGroup))
			{
				switch (groupType)
				{
					case GroupType.Recents:
					{
						if (!(recipient is SelectedRecent)) continue;
						var recent = recipient as SelectedRecent;
						if (recent.RecentName == name)
						{
							recent.Selected = selectedState;
							Debug.WriteLine("Set Recent {0} to {1}", recent.RecentName, recent.Selected);
						}
					}
						break;
					case GroupType.Friends:
					{
						if (!(recipient is SelectedFriend)) continue;
						var friend = recipient as SelectedFriend;
						if (friend.Friend.FriendlyName == name)
						{
							friend.Selected = selectedState;
							Debug.WriteLine("Set Friend {0} to {1}", friend.Friend.FriendlyName, friend.Selected);
						}
					}
						break;
				}
			}

			// We no doin this, no mo
			ViewModel.ExplicitOnNotifyPropertyChanged("SelectedRecipients");
		}
	}
}
