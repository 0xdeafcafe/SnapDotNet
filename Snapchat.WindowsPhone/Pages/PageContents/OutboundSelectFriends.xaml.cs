using System;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;
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
		public void SelectAllFriends()
		{
			_isInMassOperation = true;
			foreach (var recipient in ViewModel.RecipientList.Where(r => r.GroupType != GroupType.Stories).SelectMany(recipientGroup => recipientGroup))
				recipient.Selected = true;
			_isInMassOperation = false;
		}

		public void DeSelectAllFriends()
		{
			_isInMassOperation = true;
			foreach (var recipient in ViewModel.RecipientList.Where(r => r.GroupType != GroupType.Stories).SelectMany(recipientGroup => recipientGroup))
				recipient.Selected = false;
			_isInMassOperation = false;
		}

		private void ItemGrid_OnTapped(object sender, TappedRoutedEventArgs e)
		{
			var element = sender as FrameworkElement;
			if (element == null) return;
			var item = element.DataContext as SelectedItem;
			if (item == null) return;
			item.Selected = !item.Selected;

			CheckOtherShit(item);
		}

		private void SelectionCheckBox_OnCheckChanged(object sender, RoutedEventArgs e)
		{
			var element = sender as FrameworkElement;
			if (element == null) return;
			var item = element.DataContext as SelectedItem;
			if (item == null) return;
			item.Selected = !item.Selected;

			CheckOtherShit(item);
		}

		private void CheckOtherShit(SelectedItem item)
		{
			if (_isInMassOperation) return;
			if (item is SelectedStory) return;
			var checkFriends = (item is SelectedRecent);
			string name;

			if (item is SelectedRecent)
				name = (item as SelectedRecent).RecentName;
			else if (item is SelectedFriend)
				name = (item as SelectedFriend).Friend.FriendlyName;
			else
				throw new InvalidOperationException();

			// find the friend, and attach
			foreach (var recipient in ViewModel.RecipientList.Where(r => r.Key != "STORIES").SelectMany(recipientGroup => recipientGroup))
			{
				if (checkFriends)
				{
					if (!(recipient is SelectedFriend)) continue;
					var friend = recipient as SelectedFriend;
					if (friend.Friend.FriendlyName == name)
						friend.Selected = item.Selected;
				}
				else
				{
					if (!(recipient is SelectedRecent)) continue;
					var recent = recipient as SelectedRecent;
					if (recent.RecentName == name)
						recent.Selected = item.Selected;
				}
			}
		}
	}
}
