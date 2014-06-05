using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using SnapDotNet.Core.Miscellaneous.Models;
using SnapDotNet.Core.Snapchat.Converters.Json;

namespace SnapDotNet.Core.Snapchat.Models.New
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
		: NotifyPropertyChangedBase, IComparable, IComparable<Snap>
	{
		public Snap()
		{
			Status = SnapStatus.None;
		}

		[DataMember(Name = "c_id")]
		public string ContentId
		{
			get { return _contentId; }
			set { SetField(ref _contentId, value); }
		}
		private string _contentId;

		[DataMember(Name = "id")]
		public string Id
		{
			get { return _id; }
			set { SetField(ref _id, value); }
		}
		private string _id;

		[DataMember(Name = "m")]
		public MediaType MediaType
		{
			get { return _mediaType; }
			set { SetField(ref _mediaType, value); }
		}
		private MediaType _mediaType;

		[DataMember(Name = "rp")]
		public string RecipientName
		{
			get { return _recipientName; }
			set { SetField(ref _recipientName, value); }
		}
		private string _recipientName;

		[DataMember(Name = "sn")]
		public string SenderName
		{
			get { return _senderName; }
			set { SetField(ref _senderName, value); }
		}
		private string _senderName;

		[DataMember(Name = "st")]
		public SnapStatus Status
		{
			get { return _status; }
			set
			{
				SetField(ref _status, value);
				NotifyPropertyChanged("IsDownloading");
				NotifyPropertyChanged("HasMedia");
				NotifyPropertyChanged("Id");
			}
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

		[DataMember(Name = "ts")]
		[JsonConverter(typeof(UnixDateTimeConverter))]
		public DateTime Timestamp
		{
			get { return _timestamp; }
			set { SetField(ref _timestamp, value); }
		}
		private DateTime _timestamp;

		#region IComparable<Snap> Members

		public int CompareTo(Snap other)
		{
			return Convert.ToInt32((SentTimestamp - other.SentTimestamp).TotalSeconds);
		}

		#endregion

		#region IComparable Members

		public int CompareTo(object obj)
		{
			return Convert.ToInt32((SentTimestamp - ((Snap) obj).SentTimestamp).TotalSeconds);
		}

		#endregion
	}
}
