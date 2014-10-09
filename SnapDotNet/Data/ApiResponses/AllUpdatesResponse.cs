using System;
using System.Runtime.Serialization;

namespace SnapDotNet.Data.ApiResponses
{
	[DataContract]
	internal sealed class AllUpdatesResponse
	{
		[DataMember(Name = "background_fetch_secret_key")]
		public String BackgroundFetchSecretKey { get; set; }

		[DataMember(Name = "conversations_response")]
		public ConversationResponse[] ConversationsResponse { get; set; }

		[DataMember(Name = "messaging_gateway_info")]
		public MessagingGatewayInfoResponse MessagingGatewayInfo { get; set; }

		[DataMember(Name = "stories_response")]
		public StoriesResponse StoriesResponse { get; set; }

		[DataMember(Name = "updates_response")]
		public AccountResponse AccountResponse { get; set; }
	}
}
