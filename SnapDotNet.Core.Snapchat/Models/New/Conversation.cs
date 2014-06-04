using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using SnapDotNet.Core.Miscellaneous.CustomTypes;
using SnapDotNet.Core.Snapchat.Converters.Json;

namespace SnapDotNet.Core.Snapchat.Models.New
{
	public class Conversation
	{
		[DataMember(Name = "conversation_state")]
		public ConversationState ConversationState { get; set; }

		[DataMember(Name = "last_chat_actions")]
		public ConversationState LastChatActions { get; set; }

		public string Id { get; set; }

		[DataMember(Name = "iter_token")]
		public string IterToken { get; set; }

		[DataMember(Name = "last_interaction")]
		[JsonConverter(typeof(UnixDateTimeConverter))]
		public DateTime LastInteraction { get; set; }

		[DataMember(Name = "last_snap")]
		public Snap LastSnap { get; set; }

		public String[] Participants { get; set; }

		[DataMember(Name = "pending_received_snaps")]
		public Snap[] PendingReceivedSnaps { get; set; }
	}

	public class ConversationState
	{
		[DataMember(Name = "user_chat_releases")]
		public ObservableDictionary<string, ObservableDictionary<string, Int64>> UserChatReleases { get; set; }

		[DataMember(Name = "user_sequences")]
		public ObservableDictionary<string, int> UserSequences { get; set; }

		[DataMember(Name = "user_snap_releases")]
		public ObservableDictionary<string, ObservableDictionary<string, Int64>> UserSnapReleases { get; set; }
	}

	public class LastSnapActions
	{
		[DataMember(Name = "last_read_timestamp")]
		[JsonConverter(typeof(UnixDateTimeConverter))]
		public DateTime LastRead { get; set; }

		[DataMember(Name = "last_reader")]
		public String LastReader { get; set; }

		[DataMember(Name = "last_write")]
		[JsonConverter(typeof (UnixDateTimeConverter))]
		public DateTime LastWrite { get; set; }

		[DataMember(Name = "last_write_type")]
		public String LastWriteType { get; set; }

		[DataMember(Name = "last_writer")]
		public String LastWriter { get; set; }
	}
}
