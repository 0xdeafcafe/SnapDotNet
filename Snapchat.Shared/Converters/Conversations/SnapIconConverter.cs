using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using SnapDotNet.Core.Snapchat.Models.New;

namespace Snapchat.Converters.Conversations
{
	public class SnapIconConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			var conversation = (ConversationResponse) value;
			if (conversation == null)
				return null;

			// get the snap object
			if (conversation.LastSnap == null)
				return null; // TODO: Create a pending xaml icon

			// Workpad
			// {direction}_{status}

			var lastSnap = conversation.LastSnap;
			try
			{
				return Application.Current.Resources[String.Format("{0}_{1}", lastSnap.IsIncoming ? "recieved" : "sent",
					lastSnap.Status.ToString().ToLowerInvariant())];
			}
			catch
			{
				return null;
			}
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotImplementedException();
		}
	}
}
