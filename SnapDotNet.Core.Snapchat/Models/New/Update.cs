using System;
using System.Runtime.Serialization;

namespace SnapDotNet.Core.Snapchat.Models.New
{
	public class Update
	{
		[DataMember(Name = "background_fetch_secret_key")]
		public String BackgroundFetchSecretKey { get; set; }

		[DataMember(Name = "conversations_response")]
		public Conversation ConversationResponse { get; set; }
	}
}
