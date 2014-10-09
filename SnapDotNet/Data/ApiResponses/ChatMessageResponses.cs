using Newtonsoft.Json;
using SnapDotNet.Converters.Json;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SnapDotNet.Data.ApiResponses
{
	[DataContract]
	internal sealed class ChatMessageResponse
	{
		[DataMember(Name = "body")]
		public BodyResponse Body { get; set; }

		[DataMember(Name = "chat_message_id")]
		public string ChatMessageId { get; set; }

		[DataMember(Name = "header")]
		public HeaderResponse Header { get; set; }

		[DataMember(Name = "id")]
		public string Id { get; set; }

		[DataMember(Name = "saved_state")]
		public Dictionary<string, SavedStateResponse> SavedStates { get; set; }

		[DataMember(Name = "seq_num")]
		public string SequenceNumber { get; set; }

		[DataMember(Name = "timestamp")]
		[JsonConverter(typeof(UnixDateTimeConverter))]
		public DateTime PostedAt { get; set; }

		[DataMember(Name = "type")]
		public string Type { get; set; }
	}

	[DataContract]
	internal sealed class BodyResponse
	{
		[DataMember(Name = "text")]
		public string Text { get; set; }

		[DataMember(Name = "media")]
		public BodyMediaResponse Media { get; set; }

		[DataMember(Name = "type")]
		public string Type { get; set; }
	}

	[DataContract]
	internal sealed class BodyMediaResponse
	{
		[DataMember(Name = "iv")]
		public string Iv { get; set; }

		[DataMember(Name = "key")]
		public string Key { get; set; }

		[DataMember(Name = "media_id")]
		public string MediaId { get; set; }
	}

	[DataContract]
	internal sealed class HeaderResponse
	{
		[DataMember(Name = "conv_id")]
		public string ConversationId { get; set; }

		[DataMember(Name = "from")]
		public string From { get; set; }

		[DataMember(Name = "to")]
		public string[] To { get; set; }
	}

	[DataContract]
	internal sealed class SavedStateResponse
	{
		[DataMember(Name = "saved")]
		public bool Saved { get; set; }

		[DataMember(Name = "version")]
		public short Version { get; set; }
	}
}