using Newtonsoft.Json;
using SnapDotNet.Converters.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Runtime.Serialization;

namespace SnapDotNet.Data.ApiResponses
{
	[DataContract]
	internal sealed class StoriesResponse
	{
		[DataMember(Name = "mature_content_text")]
		public MatureContentTextResponse MatureContentText { get; set; }

		[DataMember(Name = "my_stories")]
		public MyStoryResponse[] MyStories { get; set; }

		[DataMember(Name = "friend_stories")]
		public FriendStoryResponse[] FriendStories { get; set; }

		// TODO: map out 'my_group_stories'
	}

	[DataContract]
	internal sealed class MatureContentTextResponse
	{
		[DataMember(Name = "title")]
		public string Title { get; set; }

		[DataMember(Name = "message")]
		public string Message { get; set; }

		[DataMember(Name = "yes_text")]
		public string YesText { get; set; }

		[DataMember(Name = "no_text")]
		public string NoText { get; set; }
	}

	[DataContract]
	internal sealed class StoryMetadataResponse
	{
		[DataMember(Name = "story")]
		public StoryResponse Story { get; set; }

		[DataMember(Name = "viewed")]
		public bool Viewed { get; set; }
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
		public bool Screenshotted { get; set; }

		[DataMember(Name = "storypointer")]
		public Dictionary<string, string> StoryPointer { get; set; }

		[DataMember(Name = "timestamp")]
		[JsonConverter(typeof(UnixDateTimeConverter))]
		public DateTime Timestamp { get; set; }

		[DataMember(Name = "viewer")]
		public string Viewer { get; set; }
	}

	[DataContract]
	internal sealed class FriendStoryResponse
	{
		[DataMember(Name = "mature_content")]
		public bool MatureContent { get; set; }

		[DataMember(Name = "stories")]
		public StoryMetadataResponse[] Stories { get; set; }

		[DataMember(Name = "username")]
		public string Username { get; set; }
	}

	[DataContract]
	internal sealed class StoryExtrasResponse
	{
		[DataMember(Name = "screenshot_count")]
		public int ScreenshotCount { get; set; }

		[DataMember(Name = "view_count")]
		public int ViewCount { get; set; }
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
		public bool HasMatureContent { get; set; }

		[DataMember(Name = "media_id")]
		public string MediaId { get; set; }

		[DataMember(Name = "media_iv")]
		public string MediaIv { get; set; }

		[DataMember(Name = "media_key")]
		public string MediaKey { get; set; }

		[DataMember(Name = "media_type")]
		public int MediaType { get; set; }

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

		[DataMember(Name = "is_shared")]
		public bool IsShared { get; set; }

		#region IComparable<StoryResponse> Members

		public int CompareTo(StoryResponse other)
		{
			return Convert.ToInt32((TimeLeft - other.TimeLeft).TotalSeconds);
		}

		#endregion

		#region IComparable Members

		public int CompareTo(object obj)
		{
			return Convert.ToInt32((TimeLeft - (obj as StoryResponse).TimeLeft).TotalSeconds);
		}

		#endregion
	}
}
