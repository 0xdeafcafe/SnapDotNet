using System;
using Windows.UI.Xaml.Data;
using SnapDotNet.Core.Snapchat.Models;

namespace SnapDotNet.Apps.Converters
{
    public class SnapStatusToButtonStatusConverter : IValueConverter
    {
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			var snap = value as Snap;
			if (snap == null) return null;

			if (snap.SenderName == App.SnapChatManager.Account.Username)
				return false;

			switch (snap.Status)
			{
				case SnapStatus.Delivered:
					return true;

				default:
					return false;
			}
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotImplementedException();
		}
	}
}
