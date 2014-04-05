using System;
using System.Runtime.Serialization;

namespace SnapDotNet.Core.SnapchatApi.DataModels
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
	}

	/// <summary>
	/// Indicates the media type of a snap.
	/// </summary>
	public enum MediaType
	{
		Image,
		Video,
		VideoNoAudio,
		FriendRequest,
		FriendRequestImage,
		FriendRequestVideo,
		FriendRequestVideoNoAudio
	}

	[DataContract]
	public class Snap
		: Response
	{
		[DataMember(Name = "id")]
		public string Id { get; set; }

		[DataMember(Name = "rp")]
		public string RecipientName { get; set; }

		[DataMember(Name = "sn")]
		public string ScreenName { get; set; }

		[DataMember(Name = "ts")]
		public DateTime Timestamp { get; set; }

		[DataMember(Name = "st")]
		public SnapStatus Status { get; set; }

		[DataMember(Name = "sts")]
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
		public DateTime? CaptureTime { get; set; }

		[DataMember(Name = "oat")]
		public DateTime? OpenedAt { get; set; }

		[DataMember(Name = "rsecs")]
		public int? RemainingSeconds { get; set; }
	}
}
