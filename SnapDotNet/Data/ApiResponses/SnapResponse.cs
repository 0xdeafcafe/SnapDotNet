using Newtonsoft.Json;
using SnapDotNet.Converters.Json;
using System;
using System.Diagnostics.Contracts;
using System.Runtime.Serialization;

namespace SnapDotNet.Data.ApiResponses
{
	[DataContract]
	internal sealed class SnapResponse
		: IComparable, IComparable<SnapResponse>
	{
		[DataMember(Name = "id")]
		public string Id { get; set; }

		[DataMember(Name = "rp")]
		public string RecipientName { get; set; }

		[DataMember(Name = "sn")]
		public string SenderName { get; set; }

		[DataMember(Name = "ts")]
		[JsonConverter(typeof(UnixDateTimeConverter))]
		public DateTime Timestamp { get; set; }

		[DataMember(Name = "st")]
		public int Status { get; set; }

		[DataMember(Name = "sts")]
		[JsonConverter(typeof(UnixDateTimeConverter))]
		public DateTime SentTimestamp { get; set; }

		[DataMember(Name = "c_id")]
		public string MediaId { get; set; }

		[DataMember(Name = "m")]
		public int MediaType { get; set; }

		[DataMember(Name = "cap_text")]
		public string CaptionText { get; set; }

		[DataMember(Name = "cap_pos")]
		public double CapturePosition { get; set; }

		[DataMember(Name = "t")]
		[JsonConverter(typeof(UnixDateTimeConverter))]
		public DateTime? CaptureTime { get; set; }

		[DataMember(Name = "oat")]
		[JsonConverter(typeof(UnixDateTimeConverter))]
		public DateTime? OpenedAt { get; set; }

		[DataMember(Name = "timer")]
		public double Timer { get; set; }

		[DataMember(Name = "broadcast")]
		public int Broadcast { get; set; }

		[DataMember(Name = "broadcast_action_text")]
		public string BroadcastActionText { get; set; }

		[DataMember(Name = "broadcast_hide_timer")]
		public bool BroadcastHideTimer { get; set; }

		[DataMember(Name = "broadcast_url")]
		public string BroadcastUrl { get; set; }

		[DataMember(Name = "zipped")]
		public bool Zipped { get; set; }

		#region IComparable<SnapResponse> Members

		public int CompareTo(SnapResponse other)
		{
			return Convert.ToInt32((SentTimestamp - other.SentTimestamp).TotalSeconds);
		}

		#endregion

		#region IComparable Members

		public int CompareTo(object obj)
		{
			return Convert.ToInt32((SentTimestamp - (obj as SnapResponse).SentTimestamp).TotalSeconds);
		}

		#endregion
	}
}
