using System;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using SnapDotNet.Core.Miscellaneous.Converters.Json;
using UnixDateTimeConverter = SnapDotNet.Core.Snapchat.Converters.Json.UnixDateTimeConverter;

namespace SnapDotNet.Core.Snapchat.Models
{
	/// <summary>
	/// Represents a collection of stories.
	/// </summary>
	[DataContract]
	public class Stories : Response
	{
		[DataMember(Name = "mature_content_text")]
		public MatureContentWarning MatureContentText { get; set; }

		[DataMember(Name = "my_stories")]
		public ObservableCollection<MyStory> MyStories { get; set; }

		[DataMember(Name = "friend_stories")]
		public ObservableCollection<FriendStory> FriendStories { get; set; }

		[DataContract]
		public class MatureContentWarning
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
	}

	[DataContract]
	public class MyStory
	{
		[DataMember(Name = "story")]
		public Story Story { get; set; }

		[DataMember(Name = "story_notes")]
		public ObservableCollection<StoryNote> StoryNotes { get; set; }

		[DataMember(Name = "story_extras")]
		public StoryExtraInfo StoryExtras { get; set; }
	}

	[DataContract]
	public class StoryNote
	{
		[DataMember(Name = "viewer")]
		public string ViewerUsername { get; set; }

		[DataMember(Name = "screenshotted")]
		public bool Screenshotted { get; set; }

		[DataMember(Name = "timestamp")]
		[JsonConverter(typeof(UnixDateTimeConverter))]
		public DateTime Timestamp { get; set; }

		[DataMember(Name = "storypointer")]
		public StoryMediaPointer StoryPointer { get; set; }
	}

	[DataContract]
	public class StoryMediaPointer
	{
		[DataMember(Name = "mKey")]
		public string MediaKey { get; set; }

		[DataMember(Name = "mField")]
		public string MediaField { get; set; }
	}

	[DataContract]
	public class StoryExtraInfo
	{
		[DataMember(Name = "view_count")]
		public int ViewCount { get; set; }

		[DataMember(Name = "screenshot_count")]
		public int ScreenshotCount { get; set; }
	}

	[DataContract]
	public class FriendStory
	{
		[DataMember(Name = "username")]
		public string Username { get; set; }

		[DataMember(Name = "stories")]
		public ObservableCollection<ViewableStory> Stories { get; set; }

		[DataMember(Name = "mature_content")]
		public bool ContainsMatureContent { get; set; }
	}

	[DataContract]
	public class ViewableStory
	{
		[DataMember(Name = "story")]
		public Story Story { get; set; }

		[DataMember(Name = "viewed")]
		public bool Viewed { get; set; }
	}

	[DataContract]
	public class Story
	{
		[DataMember(Name = "id")]
		public string Id { get; set; }

		[DataMember(Name = "username")]
		public string Username { get; set; }

		[DataMember(Name = "mature_content")]
		public bool ContainsMatureContent { get; set; }

		[DataMember(Name = "client_id")]
		public string ClientId { get; set; }

		[DataMember(Name = "timestamp")]
		[JsonConverter(typeof(UnixDateTimeConverter))]
		public DateTime Timestamp { get; set; }

		[DataMember(Name = "media_id")]
		public string MediaId { get; set; }

		[DataMember(Name = "media_key")]
		public string MediaKey { get; set; }

		[DataMember(Name = "media_iv")]
		public string MediaIv { get; set; }

		[DataMember(Name = "thumbnail_iv")]
		public string ThumbnailIv { get; set; }

		[DataMember(Name = "media_type")]
		public MediaType MediaType { get; set; }

		[DataMember(Name = "time")]
		public float Time { get; set; }

		[DataMember(Name = "zipped")]
		public bool Zipped { get; set; }

		// Needs to be some sort of timespan
		[DataMember(Name = "time_left")]
		[JsonConverter(typeof(MillisecondTimeSpanConverter))]
		public TimeSpan TimeLeft { get; set; }

		[DataMember(Name = "media_url")]
		public string MediaUrl { get; set; }

		[DataMember(Name = "thumbnail_url")]
		public string ThumbnailUrl { get; set; }
	}
}
