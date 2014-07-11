using System;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Snapchat.SnapLogic.Api;
using Snapchat.SnapLogic.Api.Exceptions;
using Snapchat.SnapLogic.Helpers;
using SnapDotNet.Core.Miscellaneous.Helpers;
using SnapDotNet.Core.Miscellaneous.Models;

namespace Snapchat.Models
{
	/// <summary>
	/// Indicates the status of a snap.
	/// </summary>
	public enum SnapStatus
	{
		None = -1,
		Sent,
		Delivered,
		Opened,
		Screenshotted,
		Downloading = 0xDEAD,
	}

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

	public class Snap
		: NotifyPropertyChangedBase, IConversationItem, IComparable, IComparable<Snap>
	{
		public Snap()
		{
			Status = SnapStatus.None;
		}

		public string ContentId
		{
			get { return _contentId; }
			set { SetField(ref _contentId, value); }
		}
		private string _contentId;

		public string Id
		{
			get { return _id; }
			set { SetField(ref _id, value); }
		}
		private string _id;

		public MediaType MediaType
		{
			get { return _mediaType; }
			set { SetField(ref _mediaType, value); }
		}
		private MediaType _mediaType;

		public string RecipientName
		{
			get { return _recipientName; }
			set { SetField(ref _recipientName, value); }
		}
		private string _recipientName;

		public string SenderName
		{
			get { return _senderName; }
			set { SetField(ref _senderName, value); }
		}
		private string _senderName;

		public SnapStatus Status
		{
			get { return _status; }
			set
			{
				SetField(ref _status, value);
				NotifyPropertyChanged("IsDownloading");
				NotifyPropertyChanged("HasMedia");
				NotifyPropertyChanged("Id");
			}
		}
		private SnapStatus _status;

		public DateTime PostedAt
		{
			get { return _postedAt; }
			set { SetField(ref _postedAt, value); }
		}
		private DateTime _postedAt;

		public DateTime Timestamp
		{
			get { return _timestamp; }
			set { SetField(ref _timestamp, value); }
		}
		private DateTime _timestamp;

		public void CreateFromServer(SnapLogic.Models.New.Snap serverSnap)
		{
			ContentId = serverSnap.ContentId;
			Id = serverSnap.Id;
			MediaType = serverSnap.MediaType;
			PostedAt = serverSnap.PostedAt;
			RecipientName = serverSnap.RecipientName;
			SenderName = serverSnap.SenderName;
			Status = serverSnap.Status;
			Timestamp = serverSnap.Timestamp;
		}

		#region Helpers

		[IgnoreDataMember]
		public Boolean IsIncoming
		{
			get { return RecipientName == null; }
		}

		[IgnoreDataMember]
		public Boolean IsImage
		{
			get { return (MediaType == MediaType.Image || MediaType == MediaType.FriendRequestImage); }
		}

		[IgnoreDataMember]
		public String Sender
		{
			get { return SenderName ?? ContentId.Split('~')[0]; }
		}

		[IgnoreDataMember]
		public bool HasMedia
		{
			get
			{
				return AsyncHelpers.RunSync(() => Blob.StorageContainsBlobAsync(Id, BlobType.Snap));
			}
		}

		public async Task DownloadSnapBlobAsync(SnapchatManager manager)
		{
			if (Status != SnapStatus.Delivered || SenderName == manager.Username) return; // || HasMedia

			// Set snap to IsDownloading
			Status = SnapStatus.Downloading;

			// Start the download
			try
			{
				await Blob.DeleteBlobFromStorageAsync(Id, BlobType.Snap);
				var mediaBlob = await manager.Endpoints.GetSnapBlobAsync(Id);
				await Blob.SaveBlobToStorageAsync(mediaBlob, Id, BlobType.Snap);
			}
			catch (InvalidHttpResponseException exception)
			{
				if (exception.Message == "Gone")
				{
					Status = SnapStatus.Opened;
					return;
				}

				SnazzyDebug.WriteLine(exception);
			}
			catch (Exception exception)
			{
				SnazzyDebug.WriteLine(exception);
			}

			// Set snap to delivered again, but this time with media
			Status = SnapStatus.Delivered;
		}

		public async Task<byte[]> OpenSnapBlobAsync()
		{
			return await Blob.ReadBlobFromStorageAsync(Id, BlobType.Snap);
		}

		#endregion

		#region IComparable<Snap> Members

		public int CompareTo(Snap other)
		{
			return Convert.ToInt32((PostedAt - other.PostedAt).TotalSeconds);
		}

		#endregion

		#region IComparable Members

		public int CompareTo(object obj)
		{
			return Convert.ToInt32((PostedAt - ((Snap)obj).PostedAt).TotalSeconds);
		}

		#endregion

		public void Update(Snap newSnap)
		{
			ContentId = newSnap.ContentId;
			Id = newSnap.Id;
			MediaType = newSnap.MediaType;
			RecipientName = newSnap.RecipientName;
			SenderName = newSnap.SenderName;
			Status = newSnap.Status;
			PostedAt = newSnap.PostedAt;
			Timestamp = newSnap.Timestamp;
		}
	}
}
