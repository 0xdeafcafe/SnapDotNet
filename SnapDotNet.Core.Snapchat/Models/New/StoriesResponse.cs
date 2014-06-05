using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using SnapDotNet.Core.Miscellaneous.Converters.Json;
using SnapDotNet.Core.Miscellaneous.Models;
using UnixDateTimeConverter = SnapDotNet.Core.Snapchat.Converters.Json.UnixDateTimeConverter;

namespace SnapDotNet.Core.Snapchat.Models.New
{
	[DataContract]
	public class StoriesResponse
		: NotifyPropertyChangedBase
	{
		[DataMember(Name = "friend_stories")]
		public FriendStory[] FriendStories { get; set; }

		[DataMember(Name = "mature_content_text")]
		public MatureContentText MatureContentText { get; set; }
		
		[DataMember(Name = "story_extras")]
		public MyStory[] MyStories { get; set; }

		// TODO: map out 'my_group_stories'
	}

	[DataContract]
	public class MatureContentText
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
	public class StoryMetadata
		: NotifyPropertyChangedBase
	{
		[DataMember(Name = "story")]
		public Story Story { get; set; }

		[DataMember(Name = "viewed")]
		public Boolean Viewed { get; set; }
	}

	[DataContract]
	public class MyStory
		: NotifyPropertyChangedBase
	{
		[DataMember(Name = "story")]
		public Story Story { get; set; }

		// TODO: map out 'story_notes'

		[DataMember(Name = "story_extras")]
		public StoryExtras StoryExtras { get; set; }
	}

	[DataContract]
	public class FriendStory
		: NotifyPropertyChangedBase
	{
		[DataMember(Name = "mature_content")]
		public Boolean MatureContent { get; set; }

		[DataMember(Name = "stories")]
		public Story[] Stories { get; set; }

		[DataMember(Name = "username")]
		public String Username { get; set; }
	}

	[DataContract]
	public class StoryExtras
		: NotifyPropertyChangedBase
	{
		[DataMember(Name = "screenshot_count")]
		public Int32 ScreenshotCount { get; set; }

		[DataMember(Name = "view_count")]
		public Int32 ViewCount { get; set; }
	}

	[DataContract]
	public class Story
		: NotifyPropertyChangedBase, IComparable, IComparable<Story>
	{
		[DataMember(Name = "caption_text_display")]
		public string CaptionTextDisplay
		{
			get { return _captionTextDisplay; }
			set { SetField(ref _captionTextDisplay, value); }
		}
		private string _captionTextDisplay;

		[DataMember(Name = "client_id")]
		public string ClientId
		{
			get { return _clientId; }
			set { SetField(ref _clientId, value); }
		}
		private string _clientId;

		[DataMember(Name = "id")]
		public string Id
		{
			get { return _id; }
			set { SetField(ref _id, value); }
		}
		private string _id;

		[DataMember(Name = "mature_content")]
		public bool ContainsMatureContent
		{
			get { return _containsMatureContent; }
			set { SetField(ref _containsMatureContent, value); }
		}
		private bool _containsMatureContent;

		[DataMember(Name = "media_id")]
		public string MediaId
		{
			get { return _mediaId; }
			set { SetField(ref _mediaId, value); }
		}
		private string _mediaId;

		[DataMember(Name = "media_iv")]
		public string MediaIv
		{
			get { return _mediaIv; }
			set { SetField(ref _mediaIv, value); }
		}
		private string _mediaIv;

		[DataMember(Name = "media_key")]
		public string MediaKey
		{
			get { return _mediaKey; }
			set { SetField(ref _mediaKey, value); }
		}
		private string _mediaKey;

		[DataMember(Name = "media_type")]
		public MediaType MediaType
		{
			get { return _mediaType; }
			set { SetField(ref _mediaType, value); }
		}
		private MediaType _mediaType;

		[DataMember(Name = "media_url")]
		public string MediaUrl
		{
			get { return _mediaUrl; }
			set { SetField(ref _mediaUrl, value); }
		}
		private string _mediaUrl;

		[DataMember(Name = "thumbnail_iv")]
		public string ThumbnailIv
		{
			get { return _thumbnailIv; }
			set { SetField(ref _thumbnailIv, value); }
		}
		private string _thumbnailIv;

		[DataMember(Name = "thumbnail_url")]
		public string ThumbnailUrl
		{
			get { return _thumbnailUrl; }
			set { SetField(ref _thumbnailUrl, value); }
		}
		private string _thumbnailUrl;

		[DataMember(Name = "time")]
		public float Time
		{
			get { return _time; }
			set { SetField(ref _time, value); }
		}
		private float _time;

		[DataMember(Name = "time_left")]
		[JsonConverter(typeof(MillisecondTimeSpanConverter))]
		public TimeSpan TimeLeft
		{
			get { return _timeLeft; }
			set { SetField(ref _timeLeft, value); }
		}
		private TimeSpan _timeLeft;

		[DataMember(Name = "timestamp")]
		[JsonConverter(typeof(UnixDateTimeConverter))]
		public DateTime PostedAt
		{
			get { return _postedAt; }
			set { SetField(ref _postedAt, value); }
		}
		private DateTime _postedAt;

		[DataMember(Name = "username")]
		public string Username
		{
			get { return _username; }
			set { SetField(ref _username, value); }
		}
		private string _username;

		[DataMember(Name = "zipped")]
		public bool Zipped
		{
			get { return _zipped; }
			set { SetField(ref _zipped, value); }
		}
		private bool _zipped;

		#region IComparable Members

		public int CompareTo(object obj)
		{
			var story = obj as Story;
			return story != null ? Convert.ToInt32((TimeLeft - story.TimeLeft).TotalSeconds) : 0;
		}

		#endregion

		#region IComparable<Story> Members

		public int CompareTo(Story other)
		{
			return Convert.ToInt32((TimeLeft - other.TimeLeft).TotalSeconds);
		}

		#endregion
	}
}
