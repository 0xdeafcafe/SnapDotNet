using System;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Snapchat.Models;
using SnapDotNet.Core.Miscellaneous.Converters.Json;
using SnapDotNet.Core.Miscellaneous.CustomTypes;
using SnapDotNet.Core.Miscellaneous.Models;
using UnixDateTimeConverter = Snapchat.SnapLogic.Converters.Json.UnixDateTimeConverter;

namespace Snapchat.SnapLogic.Models.New
{
	[DataContract]
	public class StoriesResponse
		: NotifyPropertyChangedBase
	{
		public StoriesResponse()
		{
			_friendStories.CollectionChanged += (sender, args) => NotifyPropertyChanged("FriendStories");
			_myStories.CollectionChanged += (sender, args) => NotifyPropertyChanged("MyStories");
		}

		[DataMember(Name = "friend_stories")]
		public ObservableCollection<FriendStory> FriendStories
		{
			get { return _friendStories; }
			set { SetField(ref _friendStories, value); }
		}
		private ObservableCollection<FriendStory> _friendStories = new ObservableCollection<FriendStory>();

		[DataMember(Name = "mature_content_text")]
		public MatureContentText MatureContentText { get; set; }

		[DataMember(Name = "story_extras")]
		public ObservableCollection<MyStory> MyStories
		{
			get { return _myStories; }
			set { SetField(ref _myStories, value); }
		}
		private ObservableCollection<MyStory> _myStories = new ObservableCollection<MyStory>();

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
		public MyStory()
		{
			_storyNotes.CollectionChanged += (sender, args) => NotifyPropertyChanged("StoryNotes");
		}

		[DataMember(Name = "story")]
		public Story Story { get; set; }

		[DataMember(Name = "story_notes")]
		public ObservableCollection<StoryNote> StoryNotes
		{
			get { return _storyNotes; }
			set { SetField(ref _storyNotes, value); }
		}
		private ObservableCollection<StoryNote> _storyNotes = new ObservableCollection<StoryNote>();
			
		[DataMember(Name = "story_extras")]
		public StoryExtras StoryExtras { get; set; }
	}

	[DataContract]
	public class StoryNote
		: NotifyPropertyChangedBase
	{
		public StoryNote()
		{
			_storyPointer.MapChanged += (sender, args) => NotifyPropertyChanged("StoryPointer");
		}

		[DataMember(Name = "screenshotted")]
		public Boolean ScreenShotted { get; set; }

		[DataMember(Name = "storypointer")]
		public ObservableDictionary<string, string> StoryPointer
		{
			get { return _storyPointer; }
			set { SetField(ref _storyPointer, value); }
		}
		private ObservableDictionary<string, string> _storyPointer = new ObservableDictionary<string, string>();
		
		[DataMember(Name = "timestamp")]
		[JsonConverter(typeof(UnixDateTimeConverter))]
		public DateTime Timestamp { get; set; }

		[DataMember(Name = "viewer")]
		public String Viewer { get; set; }
	}

	[DataContract]
	public class FriendStory
		: NotifyPropertyChangedBase
	{
		public FriendStory()
		{
			_stories.CollectionChanged += (sender, args) => NotifyPropertyChanged("Stories");
		}

		[DataMember(Name = "mature_content")]
		public Boolean MatureContent { get; set; }

		[DataMember(Name = "stories")]
		public ObservableCollection<Story> Stories
		{
			get { return _stories; }
			set { SetField(ref _stories, value); }
		}
		private ObservableCollection<Story> _stories = new ObservableCollection<Story>();

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
