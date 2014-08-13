using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Snapchat.Models
{
	public interface IConversationItem
	{
		String Id { get; set; }

		DateTime PostedAt { get; set; }

		String Sender { get; }

		ControlTemplate IconResource { get; }

		SolidColorBrush IconColourBrush { get; }

		String StatusFriendly { get; }
	}
}
