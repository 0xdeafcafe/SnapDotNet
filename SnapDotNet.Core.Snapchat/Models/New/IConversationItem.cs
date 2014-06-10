using System;

namespace SnapDotNet.Core.Snapchat.Models.New
{
	public interface IConversationItem
	{
		String Id { get; set; }

		DateTime PostedAt { get; set; }

		String Sender { get; }
	}
}
