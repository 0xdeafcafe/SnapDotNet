using System;
using System.Diagnostics.Contracts;
using Newtonsoft.Json;
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
}
