using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using SnapDotNet.Core.Miscellaneous.CustomTypes;
using SnapDotNet.Core.Miscellaneous.Models;
using SnapDotNet.Core.Snapchat.Converters.Json;

namespace SnapDotNet.Core.Snapchat.Models.New
{
	[DataContract]
	public class ChatMessage
		: NotifyPropertyChangedBase
	{
		[DataMember(Name = "body")]
		public Body Body { get; set; }

		[DataMember(Name = "chat_message_id")]
		public String ChatMessageId { get; set; }

		[DataMember(Name = "header")]
		public Header Header { get; set; }

		[DataMember(Name = "id")]
		public String Id { get; set; }

		[DataMember(Name = "saved_state")]
		public ObservableDictionary<string, SavedState> SavedState { get; set; }
		
		[DataMember(Name = "seq_num")]
		public String SequenceNumber { get; set; }

		[DataMember(Name = "timestamp")]
		[JsonConverter(typeof(UnixDateTimeConverter))]
		public DateTime PostedAt { get; set; }

		[DataMember(Name = "type")]
		public String Type { get; set; }
	}

	[DataContract]
	public class Body
		: NotifyPropertyChangedBase
	{
		[DataMember(Name = "text")]
		public String Text { get; set; }

		[DataMember(Name = "type")]
		public String Type { get; set; }
	}

	[DataContract]
	public class Header
		: NotifyPropertyChangedBase
	{
		[DataMember(Name = "conv_id")]
		public String ConversationId { get; set; }

		[DataMember(Name = "from")]
		public String From { get; set; }

		[DataMember(Name = "to")]
		public String[] To { get; set; }
	}

	[DataContract]
	public class SavedState
		: NotifyPropertyChangedBase
	{
		[DataMember(Name = "saved")]
		public Boolean Saved { get; set; }

		[DataMember(Name = "version")]
		public Int16 Version { get; set; }
	}
}
