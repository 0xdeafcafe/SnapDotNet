using System;
using Windows.UI.Xaml.Data;
using Snapchat.Models;
using Snapchat.SnapLogic.Models.New;
using Snapchat.SnapLogic.Models.New;
using Snap = Snapchat.SnapLogic.Models.New.Snap;

namespace Snapchat.Converters.Conversations
{
	public class SnapStatusConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			var snap = value as Snap;
			if (snap == null)
			{
				var conversation = (ConversationResponse) value;
				if (conversation != null)
					snap = conversation.LastSnap;
			}
			if (snap == null)
				return null;

			if (snap.IsIncoming)
			{
				// You recieved this
				switch (snap.Status)
				{
					case SnapStatus.Delivered:
						return App.Strings.GetString("SnapStatusTapLoad");

					case SnapStatus.Downloading:
						return App.Strings.GetString("SnapStatusDownloading");

					//case SnapStatus.None:
					default:
						return App.Strings.GetString("SnapStatusNone");

					case SnapStatus.Opened:
						return App.Strings.GetString("SnapStatusOpened");

					case SnapStatus.Screenshotted:
						return App.Strings.GetString("SnapStatusScreenshot");
				}
			}

			// You sent this
			switch (snap.Status)
			{
				case SnapStatus.Delivered:
				case SnapStatus.Sent:
					return App.Strings.GetString("SnapStatusDelivered");

				//case SnapStatus.None:
				default:
					return App.Strings.GetString("SnapStatusNone");

				case SnapStatus.Opened:
					return App.Strings.GetString("SnapStatusOpened");

				case SnapStatus.Screenshotted:
					return App.Strings.GetString("SnapStatusScreenshot");
			}
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotImplementedException();
		}
	}
}
