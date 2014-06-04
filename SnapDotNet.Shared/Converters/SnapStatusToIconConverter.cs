using SnapDotNet.Core.Snapchat.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace SnapDotNet.Apps.Converters
{
    public sealed class SnapStatusToIconConverter
		: IValueConverter
    {
		public Symbol SentDeliveredIcon { get; set; }
		public Symbol ScreenshottedIcon { get; set; }
		public Symbol OpenedIcon { get; set; }
		public Symbol DownloadingIcon { get; set; }
		public Symbol SentIcon { get; set; }
		public Symbol ReceivedDeliveredIcon { get; set; }

		public object Convert(object value, Type targetType, object parameter, string language)
		{
			var snap = value as Snap;
			if (snap == null) return null;

			var snapStatus = (SnapStatus) snap.Status;
			switch (snapStatus)
			{
				case SnapStatus.Delivered:
					return (snap.SenderName == App.SnapchatManager.Account.Username) ? SentDeliveredIcon : ReceivedDeliveredIcon;

				case SnapStatus.Downloading:
					return DownloadingIcon;

				case SnapStatus.Opened:
					return OpenedIcon;

				case SnapStatus.Screenshotted:
					return ScreenshottedIcon;

				case SnapStatus.Sent:
					return SentIcon;

				default:
					return null;
			}

			/*if (snap.SenderName == App.SnapChatManager.Account.Username)
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

			// You sent this!
			switch (snap.Status)
			{
				case SnapStatus.Opened:
					return Application.Current.Resources["RecievedImageSnapOpenedTemplate"];

				default:
					return Application.Current.Resources["RecievedImageSnapDeliveredTemplate"];
			}*/
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotImplementedException();
		}
	}
}
