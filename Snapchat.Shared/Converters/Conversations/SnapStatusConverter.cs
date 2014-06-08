using System;
using Windows.UI.Xaml.Data;
using SnapDotNet.Core.Snapchat.Models.New;

namespace Snapchat.Converters.Conversations
{
	public class SnapStatusConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			var conversation = (ConversationResponse) value;
			if (conversation == null)
				return null;

			// get the snap object
			var lastSnap = conversation.LastSnap;
			if (lastSnap == null)
				return String.Empty;


			if (lastSnap.IsIncoming)
			{
				// You recieved this
				switch (lastSnap.Status)
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
			switch (lastSnap.Status)
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
