using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using SnapDotNet.Converters.Json;

namespace SnapDotNet.Responses
{
	/// <summary>
	/// Represents a friend that was added.
	/// </summary>
	[DataContract]
	internal sealed class AddedFriendResponse
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
		public int FriendRequestState { get; set; }
	}

	/// <summary>
	/// Represents a friend.
	/// </summary>
	[DataContract]
	internal sealed class FriendResponse
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
		public int FriendRequestState { get; set; }

		/// <summary>
		/// Gets or sets the time the friend expires from your friends list.
		/// </summary>
		/// <remarks>
		/// This only takes effect on shared stories.
		/// </remarks>
		[DataMember(Name = "expiration")]
		[JsonConverter(typeof(UnixDateTimeConverter))]
		public DateTime Expiration { get; set; }

		/// <summary>
		/// Gets or sets if the preview thumbnail of for the friend expires over time.
		/// </summary>
		/// <remarks>
		/// This only takes effect on shared stories.
		/// </remarks>
		[DataMember(Name = "dont_decay_thumbnail")]
		public bool DontDecayThumbnail { get; set; }

		/// <summary>
		/// Gets or sets the id of the shared story the friend owns.
		/// </summary>
		/// <remarks>
		/// This only takes effect on shared stories.
		/// </remarks>
		[DataMember(Name = "shared_story_id")]
		public string SharedStoryId { get; set; }

		/// <summary>
		/// Gets or sets if the friend is a shared story.
		/// </summary>
		/// <remarks>
		/// This only takes effect on shared stories.
		/// </remarks>
		[DataMember(Name = "is_shared_story")]
		public bool IsSharedStory { get; set; }

		/// <summary>
		/// Gets or sets if the friend has a custom description.
		/// </summary>
		/// <remarks>
		/// This only takes effect on shared stories.
		/// </remarks>
		[DataMember(Name = "has_custom_description")]
		public bool HasCustomDescription { get; set; }

		/// <summary>
		/// Gets or sets the venue that the friend is being hosted at.
		/// </summary>
		/// <remarks>
		/// This only takes effect on shared stories.
		/// </remarks>
		[DataMember(Name = "venue")]
		public string Venue { get; set; }
	}
}
