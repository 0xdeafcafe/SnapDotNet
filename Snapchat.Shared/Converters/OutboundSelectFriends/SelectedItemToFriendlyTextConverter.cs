using System;
using System.Linq;
using Windows.UI.Xaml.Data;
using Snapchat.Models;

namespace Snapchat.Converters.OutboundSelectFriends
{
	public sealed class SelectedItemToFriendlyTextConverter
		: IValueConverter
	{
		#region IValueConverter Members

		public object Convert(object value, Type targetType, object parameter, string language)
		{
			if (value is SelectedFriend)
				return (value as SelectedFriend).Friend.FriendlyName;

			if (value is SelectedOther)
			{
				var selectedOther = value as SelectedOther;
				if (selectedOther.OtherType == OtherType.Recent || selectedOther.OtherType == OtherType.BestFriend)
				{
					var friend = App.SnapchatManager.Account.Friends.FirstOrDefault(f => f.Name == selectedOther.OtherName);
					return friend == null ? selectedOther.OtherName : friend.FriendlyName;
				}
				return (value as SelectedOther).OtherName;
			}


			return null;
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}