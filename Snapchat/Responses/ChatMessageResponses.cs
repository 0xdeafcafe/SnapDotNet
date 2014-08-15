using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using SnapDotNet.Converters.Json;

namespace SnapDotNet.Responses
{
	public enum MessageBodyType
	{
		Text,
		Screenshot,
		Media
	}

	[DataContract]
	internal sealed class ChatMessageResponse
	{
		[DataMember(Name = "body")]
		public BodyResponse Body { get; set; }

		[DataMember(Name = "chat_message_id")]
		public String ChatMessageId { get; set; }

		[DataMember(Name = "header")]
		public HeaderResponse Header { get; set; }

		[DataMember(Name = "id")]
		public String Id { get; set; }

		[DataMember(Name = "saved_state")]
		public Dictionary<string, SavedStateResponse> SavedStates { get; set; }

		[DataMember(Name = "seq_num")]
		public String SequenceNumber { get; set; }

		[DataMember(Name = "timestamp")]
		[JsonConverter(typeof(UnixDateTimeConverter))]
		public DateTime PostedAt { get; set; }

		[DataMember(Name = "type")]
		public String Type { get; set; }
	}

	[DataContract]
	internal sealed class BodyResponse
	{
		[DataMember(Name = "text")]
		public String Text { get; set; }

		[DataMember(Name = "media")]
		public BodyMediaResponse Media { get; set; }

		[DataMember(Name = "type")]
		public MessageBodyType Type { get; set; }
	}

	[DataContract]
	internal sealed class BodyMediaResponse
	{
		[DataMember(Name = "iv")]
		public String Iv { get; set; }

		[DataMember(Name = "key")]
		public String Key { get; set; }

		[DataMember(Name = "media_id")]
		public String MediaId { get; set; }
	}

	[DataContract]
	internal sealed class HeaderResponse
	{
		[DataMember(Name = "conv_id")]
		public String ConversationId { get; set; }

		[DataMember(Name = "from")]
		public String From { get; set; }

		[DataMember(Name = "to")]
		public String[] To { get; set; }
	}

	[DataContract]
	internal sealed class SavedStateResponse
	{
		[DataMember(Name = "saved")]
		public Boolean Saved { get; set; }

		[DataMember(Name = "version")]
		public Int16 Version { get; set; }
	}
}
