using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using SnapDotNet.Core.Snapchat.Converters.Json;

namespace SnapDotNet.Core.Snapchat.Models
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
		public Snap()
		{
			Status = SnapStatus.None;
		}

		[DataMember(Name = "id")]
		public string Id
		{
			get { return _id; }
			set { SetField(ref _id, value); }
		}
		private string _id;

		[DataMember(Name = "rp")]
		public string RecipientName
		{
			get { return _recipientName; }
			set { SetField(ref _recipientName, value); }
		}
		private string _recipientName;

		[DataMember(Name = "sn")]
		public string ScreenName
		{
			get { return _screenName; }
			set { SetField(ref _screenName, value); }
		}
		private string _screenName;

		[DataMember(Name = "ts")]
		[JsonConverter(typeof(UnixDateTimeConverter))]
		public DateTime Timestamp
		{
			get { return _timestamp; }
			set { SetField(ref _timestamp, value); }
		}
		private DateTime _timestamp;

		[DataMember(Name = "st")]
		public SnapStatus Status
		{
			get { return _status; }
			set { SetField(ref _status, value); }
		}
		private SnapStatus _status;

		[DataMember(Name = "sts")]
		[JsonConverter(typeof(UnixDateTimeConverter))]
		public DateTime SentTimestamp
		{
			get { return _sentTimestamp; }
			set { SetField(ref _sentTimestamp, value); }
		}
		private DateTime _sentTimestamp;

		[DataMember(Name = "c_id")]
		public string MediaId
		{
			get { return _mediaId; }
			set { SetField(ref _mediaId, value); }
		}
		private string _mediaId;

		[DataMember(Name = "m")]
		public MediaType MediaType
		{
			get { return _mediaType; }
			set { SetField(ref _mediaType, value); }
		}
		private MediaType _mediaType;

		[DataMember(Name = "cap_text")]
		public string CaptionText
		{
			get { return _captionText; }
			set { SetField(ref _captionText, value); }
		}
		private string _captionText;

		[DataMember(Name = "cap_pos")]
		public double CapturePosition
		{
			get { return _capturePosition; }
			set { SetField(ref _capturePosition, value); }
		}
		private double _capturePosition;

		[DataMember(Name = "t")]
		[JsonConverter(typeof(UnixDateTimeConverter))]
		public DateTime? CaptureTime
		{
			get { return _captureTime; }
			set { SetField(ref _captureTime, value); }
		}
		private DateTime? _captureTime;

		[DataMember(Name = "oat")]
		[JsonConverter(typeof(UnixDateTimeConverter))]
		public DateTime? OpenedAt
		{
			get { return _openedAt; }
			set { SetField(ref _openedAt, value); }
		}
		private DateTime? _openedAt;

		[DataMember(Name = "rsecs")]
		public int? RemainingSeconds
		{
			get { return _remainingSeconds; }
			set { SetField(ref _remainingSeconds, value); }
		}
		private int? _remainingSeconds;

		[IgnoreDataMember]
		public bool HasMedia
		{
			get { return _hasMedia; }
			set { SetField(ref _hasMedia, value); }
		}
		private bool _hasMedia;
	}
}
