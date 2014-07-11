using System;

namespace Snapchat.Models
{
	public interface IConversationItem
	{
		String Id { get; set; }

		DateTime PostedAt { get; set; }

		String Sender { get; }
	}
}
