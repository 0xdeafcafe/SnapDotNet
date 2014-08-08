using System;
using System.Linq;
using Windows.UI;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using Snapchat.Models;

namespace Snapchat.Converters.Conversations
{
	//public class SnapColourConverter : IValueConverter
	//{
	//	public object Convert(object value, Type targetType, object parameter, string language)
	//	{
	//		var snap = value as Snap;
	//		if (snap == null)
	//		{

	//			var conversation = (Models.Conversation) value;
	//			if (conversation != null)
	//				snap = conversation.ConversationMessages.Snaps.First();
	//		}
	//		if (snap == null) 
	//			return null;

	//		return snap.IsImage
	//			? new SolidColorBrush(Color.FromArgb(0xFF, 0xE9, 0x27, 0x54))
	//			: new SolidColorBrush(Color.FromArgb(0xFF, 0x9B, 0x55, 0xA0));
	//	}

	//	public object ConvertBack(object value, Type targetType, object parameter, string language)
	//	{
	//		throw new NotImplementedException();
	//	}
	//}
}
