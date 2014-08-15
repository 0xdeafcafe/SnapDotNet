using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using SnapDotNet.Converters.Json;

namespace SnapDotNet.Responses
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
	public class AddedFriend
	{
		/// <summary>
		/// Gets or sets the timestamp specifying the date and time the friendship started.
		/// </summary>
		[DataMember(Name = "ts")]
		[JsonConverter(typeof(UnixDateTimeConverter))]
		public DateTime Timestamp { get; set; }

		/// <summary>
		/// Gets or sets the name of this friend.
		/// </summary>
		[DataMember(Name = "name")]
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets the display name of this friend.
		/// </summary>
		[DataMember(Name = "display")]
		public string DisplayName { get; set; }

		/// <summary>
		/// Gets or sets the state of the friend request sent to this person.
		/// </summary>
		[DataMember(Name = "type")]
		public FriendRequestState FriendRequestState { get; set; }
	}

	/// <summary>
	/// Represents a friend.
	/// </summary>
	[DataContract]
	public class Friend
	{
		/// <summary>
		/// Gets or sets whether this friend allows you to see custom stories.
		/// </summary>
		[DataMember(Name = "can_see_custom_stories")]
		public bool CanSeeCustomStories { get; set; }

		/// <summary>
		/// Gets or sets the name of this friend.
		/// </summary>
		[DataMember(Name = "name")]
		public string Name { get; set; }
		
		/// <summary>
		/// Gets or sets the display name of this friend.
		/// </summary>
		[DataMember(Name = "display")]
		public string DisplayName { get; set; }

		/// <summary>
		/// Gets or sets the state of the friend request sent to this person.
		/// </summary>
		[DataMember(Name = "type")]
		public FriendRequestState FriendRequestState { get; set; }
	}
}
