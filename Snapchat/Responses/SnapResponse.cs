using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using SnapDotNet.Converters.Json;

namespace SnapDotNet.Responses
{
	/// <summary>
	/// Indicates the status of a snap.
	/// </summary>
	public enum SnapStatus
	{
		None = -1,
		Sent,
		Delivered,
		Opened,
		Screenshotted,
		Downloading = 0xDEAD,
	}

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
		public SnapStatus Status { get; set; }

		[DataMember(Name = "sts")]
		[JsonConverter(typeof(UnixDateTimeConverter))]
		public DateTime SentTimestamp { get; set; }

		[DataMember(Name = "c_id")]
		public string MediaId { get; set; }

		[DataMember(Name = "m")]
		public MediaType MediaType { get; set; }

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
		
		#region IComparable<Snap> Members

		public int CompareTo(SnapResponse other)
		{
			return Convert.ToInt32((SentTimestamp - other.SentTimestamp).TotalSeconds);
		}

		#endregion

		#region IComparable Members

		public int CompareTo(object obj)
		{
			return Convert.ToInt32((SentTimestamp - ((SnapResponse)obj).SentTimestamp).TotalSeconds);
		}

		#endregion
	}
}