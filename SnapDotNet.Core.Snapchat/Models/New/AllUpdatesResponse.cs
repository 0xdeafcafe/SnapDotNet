﻿using System;
using System.Runtime.Serialization;

namespace SnapDotNet.Core.Snapchat.Models.New
{
	[DataContract]
	public class AllUpdatesResponse
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
		public ConversationResponse[] ConversationResponse { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[DataMember(Name = "messaging_gateway_info")]
		public MessagingGatewayInfo MessagingGatewayInfo { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[DataMember(Name = "stories_response")]
		public StoriesResponse StoriesResponse { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[DataMember(Name = "updates_response")]
		public UpdatesResponse UpdatesResponse { get; set; }
	}
}
