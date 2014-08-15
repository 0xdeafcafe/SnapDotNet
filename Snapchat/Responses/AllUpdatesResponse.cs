using System;
using System.Runtime.Serialization;

namespace SnapDotNet.Responses
{
	[DataContract]
	internal sealed class AllUpdatesResponse
	{
		/// <summary>
		/// 
		/// </summary>
		[DataMember(Name = "background_fetch_secret_key")]
		public String BackgroundFetchSecretKey { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[DataMember(Name = "conversations_response")]
		public ConversationResponse[] ConversationsResponse { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[DataMember(Name = "messaging_gateway_info")]
		public MessagingGatewayInfoResponse MessagingGatewayInfo { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[DataMember(Name = "stories_response")]
		public StoriesResponse StoriesResponse { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[DataMember(Name = "updates_response")]
		public AccountResponse AccountResponse { get; set; }
	}
}
