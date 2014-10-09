using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using SnapDotNet.Data;

namespace ColdSnap.Converters.SnapDotNet
{
    public sealed class FriendRequestStateToVisibilityConverter
		: IValueConverter
    {
		public FriendRequestState State { get; set; }

		public object Convert(object value, Type targetType, object parameter, string language)
		{
			return (FriendRequestState) value == State ? Visibility.Visible : Visibility.Collapsed;
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotImplementedException();
		}
	}
}
