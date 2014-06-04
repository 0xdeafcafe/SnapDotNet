using System;
using System.Linq;
using Windows.UI.Xaml.Data;
using SnapDotNet.Core.Snapchat.Models;

namespace SnapDotNet.Apps.Converters
{
    public class SnapToFriendlySnapStatusConverter : IValueConverter
    {
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			var snapId = value as string;
			if (snapId == null) return null;

			var snap = App.SnapchatManager.Account.Snaps.FirstOrDefault(s => s.Id == snapId);
			if (snap == null) return null;

			if (snap.SenderName == App.SnapchatManager.Account.Username)
				return snap.Status.ToString();

			switch (snap.Status)
			{
				case SnapStatus.Delivered:
					return snap.HasMedia ? "Tap and Hold..." : "Tap to load...";

				case SnapStatus.Downloading:
					return "Downloading...";

				case SnapStatus.Opened:
					return "Opened";

				case SnapStatus.Screenshotted:
					return "Screenshotted";

				default:
					return "Other";
			}
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotImplementedException();
		}
	}
}
