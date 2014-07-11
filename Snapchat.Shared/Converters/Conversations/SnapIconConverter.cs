using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Snapchat.SnapLogic.Models.New;
using Snapchat.SnapLogic.Models.New;

namespace Snapchat.Converters.Conversations
{
	public class SnapIconConverter : IValueConverter
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

			try
			{
				// Workpad
				// {direction}_{status}
				var resourceName = String.Format("{0}_{1}", snap.IsIncoming ? "recieved" : "sent",
					snap.Status.ToString().ToLowerInvariant());

				return Application.Current.Resources[resourceName];
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
