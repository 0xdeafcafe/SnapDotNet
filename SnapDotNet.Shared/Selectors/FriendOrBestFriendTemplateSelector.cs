using SnapDotNet.Core.Snapchat.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace SnapDotNet.Apps.Selectors
{
	public sealed class FriendOrBestFriendTemplateSelector
		: DataTemplateSelector
	{
		public DataTemplate BestFriendTemplate { get; set; }
		public DataTemplate FriendTemplate { get; set; }

		protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
		{
			string friend = item as string;
			if (friend != null && App.SnapChatManager.Account.BestFriends.Contains(friend))
			{
				return BestFriendTemplate;
			}

			return FriendTemplate;
		}
	}
}
