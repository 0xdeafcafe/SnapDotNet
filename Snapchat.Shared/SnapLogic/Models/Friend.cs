using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using SnapDotNet.Core.Miscellaneous.Models;
using SnapDotNet.Core.Snapchat.Converters.Json;

namespace SnapDotNet.Core.Snapchat.Models
{
	/// <summary>
	/// Actions concerning other snapchat users
	/// </summary>
	public enum FriendAction
	{
		Add,
		Delete,
		Block,
		Unblock
	}

	/// <summary>
	/// Indicates the state of a friend request.
	/// </summary>
	public enum FriendRequestState
	{
		Accepted,
		Pending,
		Blocked
	}

	/// <summary>
	/// Represents a friend that was added.
	/// </summary>
	[DataContract]
	public class AddedFriend : NotifyPropertyChangedBase
	{
		/// <summary>
		/// Gets or sets the timestamp specifying the date and time the friendship started.
		/// </summary>
		[DataMember(Name = "ts")]
		[JsonConverter(typeof(UnixDateTimeConverter))]
		public DateTime Timestamp
		{
			get { return _timestamp; }
			set { SetField(ref _timestamp, value); }
		}
		private DateTime _timestamp;

		/// <summary>
		/// Gets or sets the name of this friend.
		/// </summary>
		[DataMember(Name = "name")]
		public string Name
		{
			get { return _name; }
			set
			{
				SetField(ref _name, value);
				NotifyPropertyChanged("FriendlyName");
			}
		}
		private string _name;

		/// <summary>
		/// Gets the friendly name
		/// </summary>
		[IgnoreDataMember]
		public string FriendlyName { get { return _displayName ?? _name; } }

		/// <summary>
		/// Gets or sets the display name of this friend.
		/// </summary>
		[DataMember(Name = "display")]
		public string DisplayName
		{
			get { return _displayName; }
			set
			{
				SetField(ref _displayName, value);
				NotifyPropertyChanged("FriendlyName");
			}
		}
		private string _displayName;

		/// <summary>
		/// Gets or sets the state of the friend request sent to this person.
		/// </summary>
		[DataMember(Name = "type")]
		public FriendRequestState FriendRequestState
		{
			get { return _friendRequestState; }
			set { SetField(ref _friendRequestState, value); }
		}
		private FriendRequestState _friendRequestState;
	}

	/// <summary>
	/// Represents a friend.
	/// </summary>
	[DataContract]
	public class Friend
		: NotifyPropertyChangedBase
	{
		/// <summary>
		/// Gets or sets whether this friend allows you to see custom stories.
		/// </summary>
		[DataMember(Name = "can_see_custom_stories")]
		public bool CanSeeCustomStories
		{
			get { return _canSeeCustomStories; }
			set { SetField(ref _canSeeCustomStories, value); }
		}
		private bool _canSeeCustomStories;

		/// <summary>
		/// Gets or sets the name of this friend.
		/// </summary>
		[DataMember(Name = "name")]
		public string Name
		{
			get { return _name; }
			set
			{
				SetField(ref _name, value);
				NotifyPropertyChanged("FriendlyName");
				NotifyPropertyChanged("DisplayName");
			}
		}
		private string _name;

		/// <summary>
		/// Gets the friendly name
		/// </summary>
		[IgnoreDataMember]
		public string FriendlyName
		{
			get { return String.IsNullOrEmpty(DisplayName) ? Name : DisplayName; }
			set { SetField(ref _displayName, value); }
		}

		/// <summary>
		/// Gets or sets the display name of this friend.
		/// </summary>
		[DataMember(Name = "display")]
		public string DisplayName
		{
			get { return _displayName; }
			set
			{
				SetField(ref _displayName, value);
				NotifyPropertyChanged("FriendlyName");
				NotifyPropertyChanged("Name");
			}
		}
		private string _displayName;

		/// <summary>
		/// Gets or sets the state of the friend request sent to this person.
		/// </summary>
		[DataMember(Name = "type")]
		public FriendRequestState FriendRequestState
		{
			get { return _friendRequestState; }
			set { SetField(ref _friendRequestState, value); }
		}
		private FriendRequestState _friendRequestState;
	}
}
