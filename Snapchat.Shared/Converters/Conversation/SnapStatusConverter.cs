using System;
using Windows.UI.Xaml.Data;
using SnapDotNet.Core.Snapchat.Models.New;

namespace Snapchat.Converters.Conversation
{
	public class SnapStatusConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			var snap = value as Snap;
			if (snap == null)
				return null;

			if (snap.IsIncoming)
			{
				// You recieved this
				switch (snap.Status)
				{
					case SnapStatus.Delivered:
						return App.Strings.GetString("ChatSnapStatusTapLoad"); // TAP TO LOAD

					case SnapStatus.Downloading:
						return App.Strings.GetString("ChatSnapStatusDownloading"); // DOWNLOADING

					//case SnapStatus.None:
					default:
						return App.Strings.GetString("ChatSnapStatusNone"); // oops

					case SnapStatus.Opened:
						return App.Strings.GetString("ChatSnapStatusOpened"); // OPENED

					case SnapStatus.Screenshotted:
						return App.Strings.GetString("ChatSnapStatusScreenshot"); // SCREENSHOT!
				}
			}

			// You sent this
			switch (snap.Status)
			{
				case SnapStatus.Delivered:
				case SnapStatus.Sent:
					return App.Strings.GetString("ChatSnapStatusDelivered"); // DELIVERED

				//case SnapStatus.None:
				default:
					return App.Strings.GetString("ChatSnapStatusNone"); // oops

				case SnapStatus.Opened:
					return App.Strings.GetString("ChatSnapStatusOpened"); // OPENED

				case SnapStatus.Screenshotted:
					return App.Strings.GetString("ChatSnapStatusScreenshot"); // SCREENSHOT!
			}
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotImplementedException();
		}
	}
}
