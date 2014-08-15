using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using SnapDotNet.Converters.Json;

namespace SnapDotNet.Responses
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
		public String Id { get; set; }

		[DataMember(Name = "iter_token")]
		public String IterToken { get; set; }

		[DataMember(Name = "last_interaction_ts")]
		[JsonConverter(typeof(UnixDateTimeConverter))]
		public DateTime LastInteraction { get; set; }

		[DataMember(Name = "last_snap")]
		public SnapResponse LastSnap { get; set; }

		[DataMember(Name = "participants")]
		public String[] Participants { get; set; }

		[DataMember(Name = "pending_chats_for")]
		public String[] PendingChatsFor { get; set; }

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
		public String IterToken { get; set; }
	}

	[DataContract]
	internal sealed class MessagingAuthenticationResponse
	{
		[DataMember(Name = "mac")]
		public String Mac { get; set; }

		[DataMember(Name = "payload")]
		public String Payload { get; set; }
	}

	[DataContract]
	internal sealed class ConversationStateResponse
	{
		[DataMember(Name = "user_chat_releases")]
		public Dictionary<string, Dictionary<string, Int64>> UserChatReleases { get; set; }

		[DataMember(Name = "user_sequences")]
		public Dictionary<string, int> UserSequences { get; set; }

		[DataMember(Name = "user_snap_releases")]
		public Dictionary<string, Dictionary<string, Int64>> UserSnapReleases { get; set; }
	}

	[DataContract]
	internal sealed class LastChatActionsResponse
	{
		[DataMember(Name = "last_read_timestamp")]
		[JsonConverter(typeof(UnixDateTimeConverter))]
		public DateTime LastRead { get; set; }

		[DataMember(Name = "last_reader")]
		public String LastReader { get; set; }

		[DataMember(Name = "last_write")]
		[JsonConverter(typeof(UnixDateTimeConverter))]
		public DateTime LastWrite { get; set; }

		[DataMember(Name = "last_write_type")]
		public String LastWriteType { get; set; }

		[DataMember(Name = "last_writer")]
		public String LastWriter { get; set; }
	}
}
