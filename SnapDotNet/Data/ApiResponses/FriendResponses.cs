using Newtonsoft.Json;
using SnapDotNet.Converters.Json;
using System;
using System.Runtime.Serialization;

namespace SnapDotNet.Data.ApiResponses
{
	[DataContract]
	internal sealed class AddedFriendResponse
	{
		[DataMember(Name = "ts")]
		[JsonConverter(typeof(UnixDateTimeConverter))]
		public DateTime Timestamp { get; set; }

		[DataMember(Name = "name")]
		public string Name { get; set; }

		[DataMember(Name = "display")]
		public string DisplayName { get; set; }

		[DataMember(Name = "type")]
		public int FriendRequestState { get; set; }
	}

	[DataContract]
	internal sealed class FriendResponse
	{
		[DataMember(Name = "can_see_custom_stories")]
		public bool CanSeeCustomStories { get; set; }

		[DataMember(Name = "name")]
		public string Name { get; set; }

		[DataMember(Name = "display")]
		public string DisplayName { get; set; }

		[DataMember(Name = "type")]
		public int FriendRequestState { get; set; }

		[DataMember(Name = "expiration")]
		[JsonConverter(typeof(UnixDateTimeConverter))]
		public DateTime Expiration { get; set; }

		[DataMember(Name = "dont_decay_thumbnail")]
		public bool DontDecayThumbnail { get; set; }

		[DataMember(Name = "shared_story_id")]
		public string SharedStoryId { get; set; }

		[DataMember(Name = "is_shared_story")]
		public bool IsSharedStory { get; set; }

		[DataMember(Name = "has_custom_description")]
		public bool HasCustomDescription { get; set; }

		[DataMember(Name = "venue")]
		public string Venue { get; set; }
	}
}
