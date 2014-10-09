using Newtonsoft.Json;
using SnapDotNet.Converters.Json;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SnapDotNet.Data.ApiResponses
{
	[DataContract]
	internal sealed class ConversationResponse
	{
		[DataMember(Name = "conversation_messages")]
		public ConversationMessagesResponse ConversationMessages { get; set; }

		[DataMember(Name = "conversation_state")]
		public ConversationStateResponse ConversationState { get; set; }

		[DataMember(Name = "last_chat_actions")]
		public LastChatActionsResponse LastChatActions { get; set; }

		[DataMember(Name = "id")]
		public string Id { get; set; }

		[DataMember(Name = "iter_token")]
		public string IterToken { get; set; }

		[DataMember(Name = "last_interaction_ts")]
		[JsonConverter(typeof(UnixDateTimeConverter))]
		public DateTime LastInteraction { get; set; }

		[DataMember(Name = "last_snap")]
		public SnapResponse LastSnap { get; set; }

		[DataMember(Name = "participants")]
		public string[] Participants { get; set; }

		[DataMember(Name = "pending_chats_for")]
		public string[] PendingChatsFor { get; set; }

		[DataMember(Name = "pending_received_snaps")]
		public SnapResponse[] PendingReceivedSnaps { get; set; }
	}

	[DataContract]
	internal sealed class ConversationMessagesResponse
	{
		[DataMember(Name = "messages")]
		public MessageContainerResponse[] Messages { get; set; }

		[DataMember(Name = "messaging_auth")]
		public MessagingAuthenticationResponse MessagingAuthentication { get; set; }
	}

	[DataContract]
	internal sealed class MessageContainerResponse
	{
		[DataMember(Name = "snap")]
		public SnapResponse Snap { get; set; }

		[DataMember(Name = "chat_message")]
		public ChatMessageResponse ChatMessage { get; set; }

		[DataMember(Name = "iter_token")]
		public string IterToken { get; set; }
	}

	[DataContract]
	internal sealed class MessagingAuthenticationResponse
	{
		[DataMember(Name = "mac")]
		public string Mac { get; set; }

		[DataMember(Name = "payload")]
		public string Payload { get; set; }
	}

	[DataContract]
	internal sealed class ConversationStateResponse
	{
		[DataMember(Name = "user_chat_releases")]
		public Dictionary<string, Dictionary<string, long>> UserChatReleases { get; set; }

		[DataMember(Name = "user_sequences")]
		public Dictionary<string, int> UserSequences { get; set; }

		[DataMember(Name = "user_snap_releases")]
		public Dictionary<string, Dictionary<string, long>> UserSnapReleases { get; set; }
	}

	[DataContract]
	internal sealed class LastChatActionsResponse
	{
		[DataMember(Name = "last_read_timestamp")]
		[JsonConverter(typeof(UnixDateTimeConverter))]
		public DateTime LastRead { get; set; }

		[DataMember(Name = "last_reader")]
		public string LastReader { get; set; }

		[DataMember(Name = "last_write")]
		[JsonConverter(typeof(UnixDateTimeConverter))]
		public DateTime LastWrite { get; set; }

		[DataMember(Name = "last_write_type")]
		public string LastWriteType { get; set; }

		[DataMember(Name = "last_writer")]
		public string LastWriter { get; set; }
	}
}
