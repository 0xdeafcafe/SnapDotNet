using System;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using SnapDotNet.Apps.ViewModels.SignedIn;
using SnapDotNet.Core.Snapchat.Models;

namespace SnapDotNet.Apps.Converters
{
    public class FriendCountToVisibilityConverter : IValueConverter
    {
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			var viewModel = value as FriendsViewModel;
			if (viewModel == null) return null;

			var friends = viewModel.Account.Friends.Where(f => f.FriendRequestState == Filter);
			return friends.Any() ? Visibility.Collapsed : Visibility.Visible;
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotImplementedException();
		}

		public FriendRequestState Filter { get; set; }
	}
}
