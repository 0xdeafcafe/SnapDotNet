using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using SnapDotNet.Core.Snapchat.Models;

namespace SnapDotNet.Apps.Converters
{
	public class SnapToSnapStatusIconConverter : IValueConverter
    {
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			var snap = value as Snap;
			if (snap == null) return null;

			if (snap.SenderName == null ||
				snap.SenderName == App.SnapchatManager.Account.Username)
			{
				// You sent this!
				switch (snap.Status)
				{
					case SnapStatus.Opened:
						return Application.Current.Resources["SentImageSnapOpenedTemplate"];

					case SnapStatus.Screenshotted:
						return Application.Current.Resources["SentImageSnapScreenshottedTemplate"];

					default:
						return Application.Current.Resources["SentImageSnapDeliveredTemplate"];
				}
			}

			// You recieved this!
			switch (snap.Status)
			{
				case SnapStatus.Opened:
					return Application.Current.Resources["RecievedImageSnapOpenedTemplate"];

				default:
					return Application.Current.Resources["RecievedImageSnapDeliveredTemplate"];
			}
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotImplementedException();
		}
	}
}
