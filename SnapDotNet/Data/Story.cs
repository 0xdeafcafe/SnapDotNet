using System.Diagnostics;
using System.Threading.Tasks;
using SnapDotNet.Data.ApiResponses;
using SnapDotNet.Utilities;
using System;
using System.Diagnostics.Contracts;

namespace SnapDotNet.Data
{
	/// <summary>
	/// Represents a story entry.
	/// </summary>
	public class Story
		: ObservableObject
	{
		internal Story() { }

		/// <summary>
		/// Gets a boolean value indicating whether this story contains mature content.
		/// </summary>
		public bool HasMatureContent
		{
			get { return _matureContent; }
			private set { SetValue(ref _matureContent, value); }
		}
		private bool _matureContent;

		/// <summary>
		/// Gets the expiration date and time of this story.
		/// </summary>
		public DateTime ExpiresAt
		{
			get { return _expiresAt; }
			private set
			{
				if (SetValue(ref _expiresAt, value))
					OnPropertyChanged(() => IsExpired);
			}
		}
		private DateTime _expiresAt;

		/// <summary>
		/// Gets a boolean value indicating whether this story has expired.
		/// </summary>
		public bool IsExpired
		{
			get { return DateTime.UtcNow > ExpiresAt; }
		}

		/// <summary>
		/// Gets the id of this story.
		/// </summary>
		public string Id
		{
			get { return _id; }
			private set { SetValue(ref _id, value); }
		}
		private string _id;

		/// <summary>
		/// Gets a buffer containing the thumbnail of this story.
		/// </summary>
		public byte[] ThumbnailData
		{
			get { return _thumbnailData; }
			private set { SetValue(ref _thumbnailData, value); }
		}
		private byte[] _thumbnailData;

		/// <summary>
		/// Gets a buffer containing this story's media.
		/// </summary>
		public byte[] MediaData
		{
			get { return _mediaData; }
			private set { SetValue(ref _mediaData, value); }
		}
		private byte[] _mediaData;

		/// <summary>
		/// Gets a buffer containing this story's media overlay.
		/// </summary>
		public byte[] MediaOverlayData
		{
			get { return _mediaOverlayData; }
			private set { SetValue(ref _mediaOverlayData, value); }
		}
		private byte[] _mediaOverlayData;

		/// <summary>
		/// Gets a boolean value indicating whether the media data is cached.
		/// </summary>
		public bool IsCached
		{
			get { return _isCached; }
			private set { SetValue(ref _isCached, value); }
		}
		private bool _isCached;

		/// <summary>
		/// Gets a boolean value indicating whether the media data is being downloaded.
		/// </summary>
		public bool IsDownloading
		{
			get { return _isDownloading; }
			private set { SetValue(ref _isDownloading, value); }
		}
		private bool _isDownloading;

		/// <summary>
		/// Gets the duration of this story.
		/// </summary>
		public int Duration
		{
			get { return _duration; }
			private set { SetValue(ref _duration, value); }
		}
		private int _duration;

		private string MediaId { get; set; }
		private string MediaKey { get; set; }
		private string ThumbnailIv { get; set; }
		private bool IsCompressed { get; set; }

		public async Task DownloadThumbnailAsync()
		{
			Debug.WriteLine("[Story] Processing thumbnail (Is Compressed: {0})", IsCompressed);
			byte[] data = await EndpointManager.Managers["bq"].GetAsync(string.Format("story_thumbnail?story_id={0}", MediaId));
			ThumbnailData = Aes.DecryptDataWithIv(data, Convert.FromBase64String(MediaKey), Convert.FromBase64String(ThumbnailIv));

			// TODO: Cache thumbnail data
		}

		public async Task DownloadMediaAsync()
		{
			IsDownloading = true;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="response"></param>
		/// <returns></returns>
		internal static Story CreateFromResponse(StoryResponse response)
		{
			Contract.Requires<ArgumentNullException>(response != null);

			var story = new Story();
			story.UpdateFromResponse(response);
			return story;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="response"></param>
		internal void UpdateFromResponse(StoryResponse response)
		{
			Contract.Requires<ArgumentNullException>(response != null);

			Id = response.Id;
			HasMatureContent = response.HasMatureContent;
			ExpiresAt = DateTime.UtcNow + response.TimeLeft;
			MediaId = response.MediaId;
			IsCompressed = response.Zipped;
			MediaKey = response.MediaKey;
			ThumbnailIv = response.ThumbnailIv;
			Duration = (int) response.Time;
			/*
			Owner = storyResponse.Username;
			ClientId = storyResponse.ClientId;
			PostedAt = storyResponse.PostedAt;
			MediaIv = storyResponse.MediaIv;
			MediaType = (MediaType)storyResponse.MediaType;
			Caption = storyResponse.CaptionTextDisplay;
			IsShared = storyResponse.IsShared;
			MediaUrl = storyResponse.MediaUrl;
			ThumbnailUrl = storyResponse.ThumbnailUrl;

			MediaOverlayData = await Task.Run<byte[]>(() =>
			{
				if (!LocalMedia)
					return null;

				var storageObject = StorageManager.Local.RetrieveStorageObject(Id, StorageType.StoryOverlay);
				if (storageObject != null)
				{
					var data = AsyncHelpers.RunSync(storageObject.ReadDataAsync);
					return data;
				}
				return null;
			});*/
		}
	}
}
