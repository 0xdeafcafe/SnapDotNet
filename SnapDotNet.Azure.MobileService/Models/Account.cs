using System;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using SnapDotNet.Azure.MobileService.Converters.Json;

namespace SnapDotNet.Azure.MobileService.Models
{
	public class Response
	{
		[DataMember(Name = "logged")]
		public bool Logged;
	}

	public class Account
		: Response
	{
		[DataMember(Name = "snaps")]
		public ObservableCollection<Snap> Snaps;

		[DataMember(Name = "friends")]
		public ObservableCollection<Friend> Friends;
	}

	[DataContract]
	public class Snap
		: IComparable, IComparable<Snap>
	{
		public Snap()
		{
			Status = SnapStatus.None;
		}

		[DataMember(Name = "id")]
		public string Id;

		[DataMember(Name = "rp")]
		public string RecipientName;

		[DataMember(Name = "sn")]
		public string SenderName;

		[DataMember(Name = "ts")]
		[JsonConverter(typeof(UnixDateTimeConverter))]
		public DateTime Timestamp;

		[DataMember(Name = "st")]
		public SnapStatus Status;

		[DataMember(Name = "sts")]
		[JsonConverter(typeof(UnixDateTimeConverter))]
		public DateTime SentTimestamp;

		[DataMember(Name = "c_id")]
		public string MediaId;

		[DataMember(Name = "m")]
		public MediaType MediaType;

		[DataMember(Name = "cap_text")]
		public string CaptionText;

		[DataMember(Name = "t")]
		[JsonConverter(typeof(UnixDateTimeConverter))]
		public DateTime? CaptureTime;

		[DataMember(Name = "oat")]
		[JsonConverter(typeof(UnixDateTimeConverter))]
		public DateTime? OpenedAt;

		#region IComparable<Snap> Members

		public int CompareTo(Snap other)
		{
			return Convert.ToInt32((SentTimestamp - other.SentTimestamp).TotalSeconds);
		}

		#endregion

		#region IComparable Members

		public int CompareTo(object obj)
		{
			return Convert.ToInt32((SentTimestamp - ((Snap)obj).SentTimestamp).TotalSeconds);
		}

		#endregion
	}

	[DataContract]
	public class Friend
	{
		[DataMember(Name = "name")]
		public string Name;

		public string FriendlyName { get { return String.IsNullOrEmpty(DisplayName) ? Name : DisplayName; } }

		[DataMember(Name = "display")]
		public string DisplayName;
	}

	public enum SnapStatus
	{
		None = -1,
		Sent,
		Delivered,
		Opened,
		Screenshotted
	}

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
}
