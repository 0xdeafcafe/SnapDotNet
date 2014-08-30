﻿using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Newtonsoft.Json;
using SnapDotNet.Extentions;
using SnapDotNet.Responses;
using SnapDotNet.Utilities;

namespace SnapDotNet
{
	public class Story
		: ObservableObject
	{
		/// <summary>
		/// Create a Story from the <seealso cref="StoryResponse"/>.
		/// </summary>
		/// <param name="storyResponse">The story response to create the model from.</param>
		[Pure]
		internal static Story Create(StoryResponse storyResponse)
		{
			Contract.Requires<ArgumentNullException>(storyResponse != null);

			return new Story
			{
				Id = storyResponse.Id,
				Owner = storyResponse.Username,
				MatureContent = storyResponse.ContainsMatureContent,
				ClientId = storyResponse.ClientId,
				PostedAt = storyResponse.PostedAt,
				MediaId = storyResponse.MediaId,
				MediaKey = storyResponse.MediaKey,
				MediaIv = storyResponse.MediaIv,
				ThumbnailIv = storyResponse.ThumbnailIv,
				MediaType = (MediaType) storyResponse.MediaType,
				SecondLength = storyResponse.Time,
				Caption = storyResponse.CaptionTextDisplay,
				Compressed = storyResponse.Zipped,
				ExpiresAt = DateTime.UtcNow + storyResponse.TimeLeft,
				MediaUrl = storyResponse.MediaUrl,
				ThumbnailUrl = storyResponse.ThumbnailUrl
			};
		}

		#region Properties

		/// <summary>
		/// 
		/// </summary>
		[JsonProperty]
		public string Id
		{
			get { return _id; }
			set { SetValue(ref _id, value); }
		}
		private string _id;

		/// <summary>
		/// 
		/// </summary>
		[JsonProperty]
		public string Owner
		{
			get { return _owner; }
			set { SetValue(ref _owner, value); }
		}
		private string _owner;

		/// <summary>
		/// 
		/// </summary>
		[JsonProperty]
		public bool MatureContent
		{
			get { return _matureContent; }
			set { SetValue(ref _matureContent, value); }
		}
		private bool _matureContent;

		/// <summary>
		/// 
		/// </summary>
		[JsonProperty]
		public string ClientId
		{
			get { return _clientId; }
			set { SetValue(ref _clientId, value); }
		}
		private string _clientId;

		/// <summary>
		/// 
		/// </summary>
		[JsonProperty]
		public DateTime PostedAt
		{
			get { return _postedAt; }
			set { SetValue(ref _postedAt, value); }
		}
		private DateTime _postedAt;

		/// <summary>
		/// 
		/// </summary>
		[JsonProperty]
		public string MediaId
		{
			get { return _mediaId; }
			set { SetValue(ref _mediaId, value); }
		}
		private string _mediaId;

		/// <summary>
		/// 
		/// </summary>
		[JsonProperty]
		public string MediaKey
		{
			get { return _mediaKey; }
			set { SetValue(ref _mediaKey, value); }
		}
		private string _mediaKey;

		/// <summary>
		/// 
		/// </summary>
		[JsonProperty]
		public string MediaIv
		{
			get { return _mediaIv; }
			set { SetValue(ref _mediaIv, value); }
		}
		private string _mediaIv;

		/// <summary>
		/// 
		/// </summary>
		[JsonProperty]
		public string ThumbnailIv
		{
			get { return _thumbnailIv; }
			set { SetValue(ref _thumbnailIv, value); }
		}
		private string _thumbnailIv;

		/// <summary>
		/// 
		/// </summary>
		[JsonProperty]
		public MediaType MediaType
		{
			get { return _mediaType; }
			set { SetValue(ref _mediaType, value); }
		}
		private MediaType _mediaType;

		/// <summary>
		/// 
		/// </summary>
		[JsonProperty]
		public double SecondLength
		{
			get { return _secondLength; }
			set { SetValue(ref _secondLength, value); }
		}
		private double _secondLength;

		/// <summary>
		/// 
		/// </summary>
		[JsonProperty]
		public string Caption
		{
			get { return _caption; }
			set { SetValue(ref _caption, value); }
		}
		private string _caption;

		/// <summary>
		/// 
		/// </summary>
		[JsonProperty]
		public bool Compressed
		{
			get { return _compressed; }
			set { SetValue(ref _compressed, value); }
		}
		private bool _compressed;

		/// <summary>
		/// 
		/// </summary>
		[JsonProperty]
		public DateTime ExpiresAt
		{
			get { return _expiresAt; }
			set { SetValue(ref _expiresAt, value); }
		}
		private DateTime _expiresAt;

		/// <summary>
		/// 
		/// </summary>
		[JsonProperty]
		public string MediaUrl
		{
			get { return _mediaUrl; }
			set { SetValue(ref _mediaUrl, value); }
		}
		private string _mediaUrl;

		/// <summary>
		/// 
		/// </summary>
		[JsonProperty]
		public string ThumbnailUrl
		{
			get { return _thumbnailUrl; }
			set { SetValue(ref _thumbnailUrl, value); }
		}
		private string _thumbnailUrl;

		#endregion

		#region Helpers

		/// <summary>
		/// Gets if the story has expired.
		/// </summary>
		[JsonIgnore]
		public bool Expired
		{
			get { return DateTime.UtcNow > ExpiresAt; }
		}

		/// <summary>
		/// 
		/// </summary>
		[JsonIgnore]
		public bool IsImage
		{
			get
			{
				switch (MediaType)
				{
					case MediaType.FriendRequestVideo:
					case MediaType.FriendRequestVideoNoAudio:
					case MediaType.Video:
					case MediaType.VideoNoAudio:
						return false;

					default:
						return true;
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		[JsonIgnore]
		public int SecondsRemaining
		{
			get { return _secondsRemaining; }
			set { SetValue(ref _secondsRemaining, value); }
		}
		private int _secondsRemaining;

		/// <summary>
		/// 
		/// </summary>
		[JsonIgnore]
		public double PercentageLeft
		{
			get { return _percentageLeft; }
			set { SetValue(ref _percentageLeft, value); }
		}
		private double _percentageLeft;

		#endregion

		#region Data Helpers

		/// <summary>
		/// Gets if the story media data is saved locally.
		/// </summary>
		[JsonIgnore]
		public bool LocalMedia
		{
			get { return StorageManager.Local.RetrieveStorageObject(Id, StorageType.Story) != null; }
		}

		/// <summary>
		/// Gets if the story thumbnail data is saved locally.
		/// </summary>
		[JsonIgnore]
		public bool LocalThumbnail
		{
			get { return StorageManager.Local.RetrieveStorageObject(Id, StorageType.StoryThumbnail) != null; }
		}

		/// <summary>
		/// Gets this stories media data.
		/// </summary>
		public async Task<byte[]> GetMediaAsync()
		{
			if (LocalMedia)
				return await StorageManager.Local.RetrieveStorageObject(Id, StorageType.Story).ReadDataAsync();

			try
			{
				var mediaData = await EndpointManager.Managers["bq"].GetAsync(String.Format("story_blob?story_id={0}", MediaId));
				mediaData = Aes.DecryptDataWithIv(mediaData, Convert.FromBase64String(MediaKey), Convert.FromBase64String(MediaIv));
				Debug.WriteLine("[Story] Processing Media Compressed: {0}", Compressed);

				if (Compressed)
				{
					// well, this is going to be fun.
					Debug.WriteLine("[Story] Decompressing the story archive");
					var zipArchive = new ZipArchive(mediaData.ToMemoryStream());
					foreach (var entry in zipArchive.Entries)
					{
						if (entry.Name.StartsWith("overlay"))
						{
							Debug.WriteLine("[Story] Detected png overlay in the archive");

							var storageObject = new StorageObject
							{
								ExpiresAt = DateTime.UtcNow.AddDays(2),
								SnapchatId = Id,
								StorageType = StorageType.StoryOverlay
							};
							using (var stream = entry.Open())
								storageObject.WriteDataAsync(stream.ToByteArray());
							StorageManager.Local.AddStorageObject(storageObject);
						}
						else if (entry.Name.StartsWith("media"))
						{
							Debug.WriteLine("[Story] Detected video media in the archive");

							var storageObject = new StorageObject
							{
								ExpiresAt = DateTime.UtcNow.AddDays(2),
								SnapchatId = Id,
								StorageType = StorageType.Story
							};
							using (var stream = entry.Open())
								storageObject.WriteDataAsync(stream.ToByteArray());
							StorageManager.Local.AddStorageObject(storageObject);
						}
					}
				}
				else
				{
					// create storage object, downlaod thumbnail, insert object, write media
					var storageObject = new StorageObject
					{
						ExpiresAt = DateTime.UtcNow.AddDays(2),
						SnapchatId = Id,
						StorageType = StorageType.Story
					};
					storageObject.WriteDataAsync(mediaData);
					StorageManager.Local.AddStorageObject(storageObject);
				}

				OnPropertyChanged("LocalMedia");

				return mediaData;
			}
			catch
			{
				return null;
			}
		}

		/// <summary>
		/// Gets this stories thumbnail data.
		/// </summary>
		public async Task<byte[]> GetThumbnailAsync()
		{
			if (LocalMedia)
			{
				return await StorageManager.Local.RetrieveStorageObject(Id, StorageType.StoryThumbnail).ReadDataAsync();
			}

			try
			{
				var thumbnailData =
					await EndpointManager.Managers["bq"].GetAsync(String.Format("story_thumbnail?story_id={0}", MediaId));
				Debug.WriteLine("[Story] Processing Thumbnail Compressed: {0}", Compressed);
				thumbnailData = Aes.DecryptDataWithIv(thumbnailData, Convert.FromBase64String(MediaKey), Convert.FromBase64String(ThumbnailIv));

				// create storage object, downlaod thumbnail, insert object, write media
				var storageObject = new StorageObject
				{
					ExpiresAt = DateTime.UtcNow.AddDays(2),
					SnapchatId = Id,
					StorageType = StorageType.StoryThumbnail
				};
				storageObject.WriteDataAsync(thumbnailData);
				StorageManager.Local.AddStorageObject(storageObject);

				OnPropertyChanged("LocalThumbnail");
				OnPropertyChanged("Thumbnail");

				return thumbnailData;
			}
			catch
			{
				OnPropertyChanged("LocalThumbnail");
				OnPropertyChanged("Thumbnail");
				return null;
			}
		}
		
		/// <summary>
		/// Gets this stories thumbnail data.
		/// </summary>
		[JsonIgnore]
		public byte[] ThumbnailData
		{
			get
			{
				return AsyncHelpers.RunSync(GetThumbnailAsync);
			}
		}

		/// <summary>
		/// Gets this stories media data.
		/// </summary>
		[JsonIgnore]
		public byte[] MediaData
		{
			get
			{
				return AsyncHelpers.RunSync(GetMediaAsync);
			}
		}

		/// <summary>
		/// Gets this stories media data.
		/// </summary>
		[JsonIgnore]
		public byte[] MediaOverlayData
		{
			get
			{
				if (!LocalMedia)
					return null;

				var storageObject = StorageManager.Local.RetrieveStorageObject(Id, StorageType.StoryOverlay);
				var data = AsyncHelpers.RunSync(storageObject.ReadDataAsync);
				return data;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		[JsonIgnore]
		public string PathToMediaData
		{
			get
			{
				var baseFilePath = StorageManager.Local.StorageFolder.Path;
				var fileName = StorageManager.Local.RetrieveStorageObject(Id, StorageType.Story).GenerateFileName();
				return Path.Combine(baseFilePath, fileName);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		[JsonIgnore]
		public MediaElement MediaElement
		{
			get { return _mediaElement; }
			set { SetValue(ref _mediaElement, value); }
		}
		private MediaElement _mediaElement;

		#endregion

		#region UI Helpers

		public delegate void MediaElapsedEventHandler(object sender);
		private event MediaElapsedEventHandler MediaElapsed;
		private DispatcherTimer _mediaElapsedTimer;
		private DispatcherTimer _mediaIntervalTimer;

		public async void InitalizeStory(MediaElapsedEventHandler mediaElapsed)
		{
			// Attach Elpased Event
			MediaElapsed += mediaElapsed;

			if (_mediaIntervalTimer != null)
				_mediaIntervalTimer.Stop();
			if (_mediaElapsedTimer != null)
				_mediaElapsedTimer.Stop();

			// Set Seconds Remaining
			SecondsRemaining = (int) SecondLength;
			PercentageLeft = 100;

			_mediaIntervalTimer = new DispatcherTimer{ Interval = new TimeSpan(0, 0, 1)};
			_mediaIntervalTimer.Tick += (sender, o) =>
			{
				if (_mediaIntervalTimer == null) return;

				if (SecondsRemaining > 1)
					SecondsRemaining--;
				else
					SecondsRemaining = 1;

				PercentageLeft = (SecondsRemaining / SecondLength) * 100;
			};
			_mediaElapsedTimer = new DispatcherTimer {Interval = new TimeSpan(0, 0, (int) SecondLength)};
			_mediaElapsedTimer.Tick += (sender, o) =>
			{
				if (_mediaElapsedTimer == null) return;

				_mediaElapsedTimer.Stop();
				_mediaIntervalTimer.Stop();

				// fire event
				if (IsImage)
					MediaElapsed(this); // If it isn't an image, we fire this inside the MediaElement
			};
			if (!IsImage)
			{
				// Create Media Element
				var storageObject = StorageManager.Local.RetrieveStorageObject(Id, StorageType.Story);
				var storageFile = await StorageManager.Local.StorageFolder.GetFileAsync(storageObject.GenerateFileName());
				var stream = await storageFile.OpenAsync(FileAccessMode.Read);
				MediaElement = new MediaElement
				{
					AutoPlay = true
				};
				MediaElement.Loaded += delegate
				{
					MediaElement.SetSource(stream, "video/mp4");
					MediaElement.Play();

					// Start timers when the media element is initalized
					_mediaIntervalTimer.Start();
					_mediaElapsedTimer.Start();
				};
				MediaElement.MediaEnded += delegate
				{
					MediaElapsed(this);
				};
			}
			else
			{
				_mediaIntervalTimer.Start();
				_mediaElapsedTimer.Start();
			}
		}

		public void DisposeStory()
		{
			if (_mediaIntervalTimer != null)
				_mediaIntervalTimer.Stop();

			if (_mediaElapsedTimer != null)
				_mediaElapsedTimer.Stop();

			if (MediaElement != null)
				MediaElement.Stop();

			_mediaIntervalTimer = null;
			_mediaElapsedTimer = null;
			MediaElement = null;

			SecondsRemaining = (int) SecondLength;
			PercentageLeft = 100;
			MediaElapsed = null;
		}

		#endregion

		/// <summary>
		/// Update a Story from the <seealso cref="StoryResponse"/>.
		/// </summary>
		/// <param name="storyResponse">The story response to update the model from.</param>
		internal void Update(StoryResponse storyResponse)
		{
			Id = storyResponse.Id;
			Owner = storyResponse.Username;
			MatureContent = storyResponse.ContainsMatureContent;
			ClientId = storyResponse.ClientId;
			PostedAt = storyResponse.PostedAt;
			MediaId = storyResponse.MediaId;
			MediaKey = storyResponse.MediaKey;
			MediaIv = storyResponse.MediaIv;
			ThumbnailIv = storyResponse.ThumbnailIv;
			MediaType = (MediaType)storyResponse.MediaType;
			SecondLength = storyResponse.Time;
			Caption = storyResponse.CaptionTextDisplay;
			Compressed = storyResponse.Zipped;
			ExpiresAt = DateTime.UtcNow + storyResponse.TimeLeft;
			MediaUrl = storyResponse.MediaUrl;
			ThumbnailUrl = storyResponse.ThumbnailUrl;
		}
	}

	public class FriendStory
		: Story
	{
		/// <summary>
		/// Create a Story from the <seealso cref="StoryMetadataResponse"/>.
		/// </summary>
		/// <param name="storyResponse">The story response to create the model from.</param>
		[Pure]
		internal static FriendStory Create(StoryMetadataResponse storyResponse)
		{
			Contract.Requires<ArgumentNullException>(storyResponse != null);

			var story = Create(storyResponse.Story);
			var friendStory = new FriendStory
			{
				Id = story.Id,
				Owner = story.Owner,
				MatureContent = story.MatureContent,
				ClientId = story.ClientId,
				PostedAt = story.PostedAt,
				MediaId = story.MediaId,
				MediaKey = story.MediaKey,
				MediaIv = story.MediaIv,
				ThumbnailIv = story.ThumbnailIv,
				MediaType = story.MediaType,
				SecondLength = story.SecondLength,
				Caption = story.Caption,
				Compressed = story.Compressed,
				ExpiresAt = story.ExpiresAt,
				MediaUrl = story.MediaUrl,
				ThumbnailUrl = story.ThumbnailUrl,
				Viewed = storyResponse.Viewed
			};
			return friendStory;
		}

		/// <summary>
		/// Gets or sets if the story has been viewed.
		/// </summary>
		[JsonProperty]
		public bool Viewed
		{
			get { return _viewed; }
			set { SetValue(ref _viewed, value); }
		}
		private bool _viewed;

		/// <summary>
		/// Update a Story from the <seealso cref="StoryMetadataResponse"/>.
		/// </summary>
		/// <param name="storyResponse">The story response to update the model from.</param>
		internal void Update(StoryMetadataResponse storyResponse)
		{
			// Don't set the Viewed field, we want to keep that from our own records
			Update(storyResponse.Story);
		}
	}

	public class PersonalStory
		: ObservableObject
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="storyResponse"></param>
		/// <returns></returns>
		[Pure]
		internal static PersonalStory Create(MyStoryResponse storyResponse)
		{
			Contract.Requires<ArgumentNullException>(storyResponse != null);

			var personalStory = new PersonalStory
			{
				Story = Story.Create(storyResponse.Story),
				ScreenshotCount = storyResponse.StoryExtras.ScreenshotCount,
				ViewCount = storyResponse.StoryExtras.ViewCount,
				StoryNotes = new ObservableCollection<StoryNote>(storyResponse.StoryNotes.Select(SnapDotNet.StoryNote.Create))
			};
			return personalStory;
		}

		#region Properties

		/// <summary>
		/// 
		/// </summary>
		[JsonProperty]
		public Story Story
		{
			get { return _story; }
			set { SetValue(ref _story, value); }
		}
		private Story _story;

		/// <summary>
		/// 
		/// </summary>
		[JsonProperty]
		public int ViewCount
		{
			get { return _viewCount; }
			set { SetValue(ref _viewCount, value); }
		}
		private int _viewCount;

		/// <summary>
		/// 
		/// </summary>
		[JsonProperty]
		public ObservableCollection<StoryNote> StoryNotes
		{
			get { return _storyNotes; }
			set { SetValue(ref _storyNotes, value); }
		}
		private ObservableCollection<StoryNote> _storyNotes = new ObservableCollection<StoryNote>();

		/// <summary>
		/// 
		/// </summary>
		[JsonProperty]
		public int ScreenshotCount
		{
			get { return _screenshot; }
			set { SetValue(ref _screenshot, value); }
		}
		private int _screenshot;

		#endregion

		/// <summary>
		/// 
		/// </summary>
		/// <param name="storyResponse"></param>
		internal void Update(MyStoryResponse storyResponse)
		{
			Contract.Requires<ArgumentNullException>(storyResponse != null);

			// Update Story
			Story.Update(storyResponse.Story);

			// Update Properties
			ViewCount = storyResponse.StoryExtras.ViewCount;
			ScreenshotCount = storyResponse.StoryExtras.ViewCount;

			// Add new Notes
			foreach(var note in storyResponse.StoryNotes.Where(n1 => StoryNotes.FirstOrDefault(n => n.Viewer == n1.Viewer) == null))
				StoryNotes.Add(StoryNote.Create(note));
			
			// Update exisiting notes
			foreach (var note in StoryNotes)
			{
				var updatedNote = storyResponse.StoryNotes.FirstOrDefault(n => n.Viewer == note.Viewer);
				if (updatedNote == null) continue;
				note.Update(updatedNote);
			}

			// Remove non-existant notes
			foreach (
				var redudantStory in
					StoryNotes.Where(n => storyResponse.StoryNotes.FirstOrDefault(n1 => n1.Viewer == n.Viewer) == null).ToList())
				StoryNotes.Remove(redudantStory);
		}
	}

	public class StoryNote
		: ObservableObject
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="storyNoteResponse"></param>
		/// <returns></returns>
		[Pure]
		internal static StoryNote Create(StoryNoteResponse storyNoteResponse)
		{
			Contract.Requires<ArgumentNullException>(storyNoteResponse != null);

			return new StoryNote
			{
				ScreenShotted = storyNoteResponse.ScreenShotted,
				ViewedAt = storyNoteResponse.Timestamp,
				Viewer = storyNoteResponse.Viewer
			};
		}

		#region Properties

		/// <summary>
		/// 
		/// </summary>
		[JsonProperty]
		public string Viewer
		{
			get { return _viewer; }
			set { SetValue(ref _viewer, value); }
		}
		private string _viewer;

		/// <summary>
		/// 
		/// </summary>
		[JsonProperty]
		public DateTime ViewedAt
		{
			get { return _viewedAt; }
			set { SetValue(ref _viewedAt, value); }
		}
		private DateTime _viewedAt;

		/// <summary>
		/// 
		/// </summary>
		[JsonProperty]
		public bool ScreenShotted
		{
			get { return _screenShotted; }
			set { SetValue(ref _screenShotted, value); }
		}
		private bool _screenShotted;

		#endregion

		/// <summary>
		/// 
		/// </summary>
		/// <param name="storyNoteResponse"></param>
		internal void Update(StoryNoteResponse storyNoteResponse)
		{
			Viewer = storyNoteResponse.Viewer;
			ViewedAt = storyNoteResponse.Timestamp;
			ScreenShotted = storyNoteResponse.ScreenShotted;
		}
	}
}
