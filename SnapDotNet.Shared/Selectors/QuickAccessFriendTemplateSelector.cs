using SnapDotNet.Core.Snapchat.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace SnapDotNet.Apps.Selectors
{
	public sealed class QuickAccessFriendTemplateSelector
		: DataTemplateSelector
	{
		public DataTemplate BestFriendTemplate { get; set; }
		public DataTemplate FriendTemplate { get; set; }
		public DataTemplate GroupTemplate { get; set; }

		protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
		{
			if (item is FriendGroup)
				return GroupTemplate;

			Friend friend = item as Friend;
			if (friend != null && App.SnapChatManager.Account.BestFriends.Contains(friend.Name))
			{
				return BestFriendTemplate;
			}

			return FriendTemplate;
		}
	}
}
