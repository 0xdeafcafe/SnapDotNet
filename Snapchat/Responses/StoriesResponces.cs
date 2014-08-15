using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using SnapDotNet.Converters.Json;

namespace SnapDotNet.Responses
{
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
	internal sealed class StoriesResponse
	{
		[DataMember(Name = "friend_stories")]
		public FriendStoryResponse[] FriendStories { get; set; }

		[DataMember(Name = "mature_content_text")]
		public MatureContentTextResponse MatureContentText { get; set; }

		[DataMember(Name = "story_extras")]
		public MyStoryResponse[] MyStories { get; set; }

		// TODO: map out 'my_group_stories'
	}

	[DataContract]
	internal sealed class MatureContentTextResponse
	{
		[DataMember(Name = "title")]
		public String Title { get; set; }

		[DataMember(Name = "message")]
		public String Message { get; set; }

		[DataMember(Name = "yes_text")]
		public String YesText { get; set; }

		[DataMember(Name = "no_text")]
		public String NoText { get; set; }
	}

	[DataContract]
	internal sealed class StoryMetadataResponse
	{
		[DataMember(Name = "story")]
		public StoryResponse Story { get; set; }

		[DataMember(Name = "viewed")]
		public Boolean Viewed { get; set; }
	}

	[DataContract]
	internal sealed class MyStoryResponse
	{
		[DataMember(Name = "story")]
		public StoryResponse Story { get; set; }

		[DataMember(Name = "story_notes")]
		public StoryNoteResponse[] StoryNotes { get; set; }

		[DataMember(Name = "story_extras")]
		public StoryExtrasResponse StoryExtras { get; set; }
	}

	[DataContract]
	internal sealed class StoryNoteResponse
	{
		[DataMember(Name = "screenshotted")]
		public Boolean ScreenShotted { get; set; }

		[DataMember(Name = "storypointer")]
		public Dictionary<string, string> StoryPointer { get; set; }

		[DataMember(Name = "timestamp")]
		[JsonConverter(typeof(UnixDateTimeConverter))]
		public DateTime Timestamp { get; set; }

		[DataMember(Name = "viewer")]
		public String Viewer { get; set; }
	}

	[DataContract]
	internal sealed class FriendStoryResponse
	{
		[DataMember(Name = "mature_content")]
		public Boolean MatureContent { get; set; }

		[DataMember(Name = "stories")]
		public StoryMetadataResponse[] Stories { get; set; }

		[DataMember(Name = "username")]
		public String Username { get; set; }
	}

	[DataContract]
	internal sealed class StoryExtrasResponse
	{
		[DataMember(Name = "screenshot_count")]
		public Int32 ScreenshotCount { get; set; }

		[DataMember(Name = "view_count")]
		public Int32 ViewCount { get; set; }
	}

	[DataContract]
	internal sealed class StoryResponse
		: IComparable, IComparable<StoryResponse>
	{
		[DataMember(Name = "caption_text_display")]
		public string CaptionTextDisplay { get; set; }

		[DataMember(Name = "client_id")]
		public string ClientId { get; set; }

		[DataMember(Name = "id")]
		public string Id { get; set; }

		[DataMember(Name = "mature_content")]
		public bool ContainsMatureContent { get; set; }

		[DataMember(Name = "media_id")]
		public string MediaId { get; set; }

		[DataMember(Name = "media_iv")]
		public string MediaIv { get; set; }

		[DataMember(Name = "media_key")]
		public string MediaKey { get; set; }

		[DataMember(Name = "media_type")]
		public MediaType MediaType { get; set; }

		[DataMember(Name = "media_url")]
		public string MediaUrl { get; set; }

		[DataMember(Name = "thumbnail_iv")]
		public string ThumbnailIv { get; set; }

		[DataMember(Name = "thumbnail_url")]
		public string ThumbnailUrl { get; set; }

		[DataMember(Name = "time")]
		public float Time { get; set; }

		[DataMember(Name = "time_left")]
		[JsonConverter(typeof(MillisecondTimeSpanConverter))]
		public TimeSpan TimeLeft { get; set; }

		[DataMember(Name = "timestamp")]
		[JsonConverter(typeof(UnixDateTimeConverter))]
		public DateTime PostedAt { get; set; }

		[DataMember(Name = "username")]
		public string Username { get; set; }

		[DataMember(Name = "zipped")]
		public bool Zipped { get; set; }

		#region IComparable Members

		public int CompareTo(object obj)
		{
			var story = obj as StoryResponse;
			return story != null ? Convert.ToInt32((TimeLeft - story.TimeLeft).TotalSeconds) : 0;
		}

		#endregion

		#region IComparable<Story> Members

		public int CompareTo(StoryResponse other)
		{
			return Convert.ToInt32((TimeLeft - other.TimeLeft).TotalSeconds);
		}

		#endregion
	}
}