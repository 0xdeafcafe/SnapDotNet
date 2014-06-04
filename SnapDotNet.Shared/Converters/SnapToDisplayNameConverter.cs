using System;
using System.Linq;
using Windows.UI.Xaml.Data;
using SnapDotNet.Core.Snapchat.Models;

namespace SnapDotNet.Apps.Converters
{
    public class SnapToDisplayNameConverter : IValueConverter
    {
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			var snap = value as Snap;
			if (snap == null) return null;

			var name = snap.RecipientName ?? snap.SenderName;

			var output = name;
			foreach (var friend in App.SnapchatManager.Account.Friends.Where(friend => friend.Name == name))
				output = friend.FriendlyName;
			return output;
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotImplementedException();
		}
	}
}
