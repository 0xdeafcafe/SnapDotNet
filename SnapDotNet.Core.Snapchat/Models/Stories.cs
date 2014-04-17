using System;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using SnapDotNet.Core.Miscellaneous.Converters.Json;
using SnapDotNet.Core.Miscellaneous.Models;
using UnixDateTimeConverter = SnapDotNet.Core.Snapchat.Converters.Json.UnixDateTimeConverter;
using System.Collections.Generic;

namespace SnapDotNet.Core.Snapchat.Models
{
	/// <summary>
	/// Represents a collection of stories.
	/// </summary>
	[DataContract]
	public class Stories : Response
	{
		public Stories()
		{
			PropertyChanged += Stories_PropertyChanged;
		}

		[DataMember(Name = "mature_content_text")]
		public MatureContentWarning MatureContentText
		{
			get { return _matureContentText; }
			set { SetField(ref _matureContentText, value); }
		}
		private MatureContentWarning _matureContentText;

		[DataMember(Name = "my_stories")]
		public ObservableCollection<MyStory> MyStories
		{
			get { return _myStories; }
			set { SetField(ref _myStories, value); }
		}
		private ObservableCollection<MyStory> _myStories;

		[DataMember(Name = "friend_stories")]
		public ObservableCollection<FriendStory> FriendStories
		{
			get { return _friendStories; }
			set { SetField(ref _friendStories, value); }
		}
		private ObservableCollection<FriendStory> _friendStories;

		private ObservableCollection<Story> FriendStoryFeed
		{
			get { return _friendStoryFeed; }
			set { SetField(ref _friendStoryFeed, value); }
		}
		private ObservableCollection<Story> _friendStoryFeed;

		[DataContract]
		public class MatureContentWarning : NotifyPropertyChangedBase
		{
			[DataMember(Name = "title")]
			public string Title
			{
				get { return _title; }
				set { SetField(ref _title, value); }
			}
			private string _title;

			[DataMember(Name = "message")]
			public string Message
			{
				get { return _message; }
				set { SetField(ref _message, value); }
			}
			private string _message;

			[DataMember(Name = "yes_text")]
			public string YesText
			{
				get { return _yesText; }
				set { SetField(ref _yesText, value); }
			}
			private string _yesText;

			[DataMember(Name = "no_text")]
			public string NoText
			{
				get { return _noText; }
				set { SetField(ref _noText, value); }
			}
			private string _noText;
		}

		void Stories_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "FriendStories" && FriendStories != null)
			{
				FriendStories.CollectionChanged += delegate { GenerateStoryFeed(); };
				GenerateStoryFeed();
			}
		}

		void GenerateStoryFeed()
		{
			SortedSet<Story> stories = new SortedSet<Story>();
			foreach (var friendFeed in FriendStories)
			{
				foreach (var story in friendFeed.Stories)
				{
					stories.Add(story.Story);
				}
			}
			FriendStoryFeed = new ObservableCollection<Story>(stories);
		}
	}

	[DataContract]
	public class MyStory : NotifyPropertyChangedBase
	{
		[DataMember(Name = "story")]
		public Story Story
		{
			get { return _story; }
			set { SetField(ref _story, value); }
		}
		private Story _story;

		[DataMember(Name = "story_notes")]
		public ObservableCollection<StoryNote> StoryNotes
		{
			get { return _storyNotes; }
			set { SetField(ref _storyNotes, value); }
		}
		private ObservableCollection<StoryNote> _storyNotes;

		[DataMember(Name = "story_extras")]
		public StoryExtraInfo StoryExtras
		{
			get { return _storyExtras; }
			set { SetField(ref _storyExtras, value); }
		}
		private StoryExtraInfo _storyExtras;
	}

	[DataContract]
	public class StoryNote : NotifyPropertyChangedBase
	{
		[DataMember(Name = "viewer")]
		public string ViewerUsername
		{
			get { return _viewerUsername; }
			set { SetField(ref _viewerUsername, value); }
		}
		private string _viewerUsername;

		[DataMember(Name = "screenshotted")]
		public bool Screenshotted
		{
			get { return _screenshotted; }
			set { SetField(ref _screenshotted, value); }
		}
		private bool _screenshotted;

		[DataMember(Name = "timestamp")]
		[JsonConverter(typeof(UnixDateTimeConverter))]
		public DateTime Timestamp
		{
			get { return _timestamp; }
			set { SetField(ref _timestamp, value); }
		}
		private DateTime _timestamp;

		[DataMember(Name = "storypointer")]
		public StoryMediaPointer StoryPointer
		{
			get { return _storyPointer; }
			set { SetField(ref _storyPointer, value); }
		}
		private StoryMediaPointer _storyPointer;
	}

	[DataContract]
	public class StoryMediaPointer : NotifyPropertyChangedBase
	{
		[DataMember(Name = "mKey")]
		public string MediaKey
		{
			get { return _mediaKey; }
			set { SetField(ref _mediaKey, value); }
		}
		private string _mediaKey;

		[DataMember(Name = "mField")]
		public string MediaField
		{
			get { return _mediaField; }
			set { SetField(ref _mediaField, value); }
		}
		private string _mediaField;
	}

	[DataContract]
	public class StoryExtraInfo : NotifyPropertyChangedBase
	{
		[DataMember(Name = "view_count")]
		public int ViewCount
		{
			get { return _viewCount; }
			set { SetField(ref _viewCount, value); }
		}
		private int _viewCount;

		[DataMember(Name = "screenshot_count")]
		public int ScreenshotCount
		{
			get { return _screenshotCount; }
			set { SetField(ref _screenshotCount, value); }
		}
		private int _screenshotCount;
	}

	[DataContract]
	public class FriendStory : NotifyPropertyChangedBase
	{
		[DataMember(Name = "username")]
		public string Username
		{
			get { return _username; }
			set { SetField(ref _username, value); }
		}
		private string _username;

		[DataMember(Name = "stories")]
		public ObservableCollection<ViewableStory> Stories
		{
			get { return _stories; }
			set { SetField(ref _stories, value); }
		}
		private ObservableCollection<ViewableStory> _stories;

		[DataMember(Name = "mature_content")]
		public bool ContainsMatureContent
		{
			get { return _containsMatureContent; }
			set { SetField(ref _containsMatureContent, value); }
		}
		private bool _containsMatureContent;
	}

	[DataContract]
	public class ViewableStory : NotifyPropertyChangedBase
	{
		[DataMember(Name = "story")]
		public Story Story
		{
			get { return _story; }
			set { SetField(ref _story, value); }
		}
		private Story _story;

		[DataMember(Name = "viewed")]
		public bool Viewed
		{
			get { return _viewed; }
			set { SetField(ref _viewed, value); }
		}
		private bool _viewed;
	}

	[DataContract]
	public class Story : NotifyPropertyChangedBase, IComparable, IComparable<Story>
	{
		[DataMember(Name = "id")]
		public string Id
		{
			get { return _id; }
			set { SetField(ref _id, value); }
		}
		private string _id;

		[DataMember(Name = "username")]
		public string Username
		{
			get { return _username; }
			set { SetField(ref _username, value); }
		}
		private string _username;

		[DataMember(Name = "mature_content")]
		public bool ContainsMatureContent
		{
			get { return _containsMatureContent; }
			set { SetField(ref _containsMatureContent, value); }
		}
		private bool _containsMatureContent;

		[IgnoreDataMember]
		public bool IsDownloading
		{
			get { return _isDownloading; }
			set { SetField(ref _isDownloading, value); }
		}
		private bool _isDownloading;

		[DataMember(Name = "client_id")]
		public string ClientId
		{
			get { return _clientId; }
			set { SetField(ref _clientId, value); }
		}
		private string _clientId;

		[DataMember(Name = "timestamp")]
		[JsonConverter(typeof(UnixDateTimeConverter))]
		public DateTime Timestamp
		{
			get { return _timestamp; }
			set { SetField(ref _timestamp, value); }
		}
		private DateTime _timestamp;

		[DataMember(Name = "media_id")]
		public string MediaId
		{
			get { return _mediaId; }
			set { SetField(ref _mediaId, value); }
		}
		private string _mediaId;

		[DataMember(Name = "media_key")]
		public string MediaKey
		{
			get { return _mediaKey; }
			set { SetField(ref _mediaKey, value); }
		}
		private string _mediaKey;

		[DataMember(Name = "media_iv")]
		public string MediaIv
		{
			get { return _mediaIv; }
			set { SetField(ref _mediaIv, value); }
		}
		private string _mediaIv;

		[DataMember(Name = "thumbnail_iv")]
		public string ThumbnailIv
		{
			get { return _thumbnailIv; }
			set { SetField(ref _thumbnailIv, value); }
		}
		private string _thumbnailIv;

		[DataMember(Name = "media_type")]
		public MediaType MediaType
		{
			get { return _mediaType; }
			set { SetField(ref _mediaType, value); }
		}
		private MediaType _mediaType;

		[DataMember(Name = "time")]
		public float Time
		{
			get { return _time; }
			set { SetField(ref _time, value); }
		}
		private float _time;

		[DataMember(Name = "zipped")]
		public bool Zipped
		{
			get { return _zipped; }
			set { SetField(ref _zipped, value); }
		}
		private bool _zipped;

		[DataMember(Name = "time_left")]
		[JsonConverter(typeof(MillisecondTimeSpanConverter))]
		public TimeSpan TimeLeft
		{
			get { return _timeLeft; }
			set { SetField(ref _timeLeft, value); }
		}
		private TimeSpan _timeLeft;

		[DataMember(Name = "media_url")]
		public string MediaUrl
		{
			get { return _mediaUrl; }
			set { SetField(ref _mediaUrl, value); }
		}
		private string _mediaUrl;

		[DataMember(Name = "thumbnail_url")]
		public string ThumbnailUrl
		{
			get { return _thumbnailUrl; }
			set { SetField(ref _thumbnailUrl, value); }
		}
		private string _thumbnailUrl;

		#region IComparable Members

		public int CompareTo(object obj)
		{
			return Convert.ToInt32((TimeLeft - (obj as Story).TimeLeft).TotalSeconds);
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
