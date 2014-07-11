using System;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Snapchat.Models;
using Snapchat.SnapLogic.Converters.Json;
using SnapDotNet.Core.Miscellaneous.CustomTypes;
using SnapDotNet.Core.Miscellaneous.Helpers;
using SnapDotNet.Core.Miscellaneous.Models;
using Snapchat.SnapLogic.Api;
using Snapchat.SnapLogic.Api.Exceptions;
using Snapchat.SnapLogic.Helpers;

namespace Snapchat.SnapLogic.Models.New
{
	[DataContract]
	public class ChatMessage
		: NotifyPropertyChangedBase, IConversationItem
	{
		[DataMember(Name = "body")]
		public Body Body { get; set; }

		[DataMember(Name = "chat_message_id")]
		public String ChatMessageId { get; set; }

		[DataMember(Name = "header")]
		public Header Header { get; set; }

		[DataMember(Name = "id")]
		public String Id { get; set; }

		[DataMember(Name = "saved_state")]
		public ObservableDictionary<string, SavedState> SavedStates { get; set; }
		
		[DataMember(Name = "seq_num")]
		public String SequenceNumber { get; set; }

		[DataMember(Name = "timestamp")]
		[JsonConverter(typeof(UnixDateTimeConverter))]
		public DateTime PostedAt { get; set; }

		[DataMember(Name = "type")]
		public String Type { get; set; }

		#region Helpers

		[IgnoreDataMember]
		public String Sender
		{
			get { return Header.From; }
		}

		#endregion
	}

	[DataContract]
	public class Body
		: NotifyPropertyChangedBase
	{
		[DataMember(Name = "text")]
		public String Text { get; set; }

		[DataMember(Name = "media")]
		public BodyMedia Media { get; set; }

		[DataMember(Name = "type")]
		public MessageBodyType Type { get; set; }
	}

	[DataContract]
	public class BodyMedia
		: NotifyPropertyChangedBase
	{
		[DataMember(Name = "iv")]
		public String Iv { get; set; }

		[DataMember(Name = "key")]
		public String Key { get; set; }

		[DataMember(Name = "media_id")]
		public String MediaId { get; set; }

		#region Helpers

		[IgnoreDataMember]
		public bool HasMedia
		{
			get
			{
				return AsyncHelpers.RunSync(() => Blob.StorageContainsBlobAsync(MediaId, BlobType.ChatMessageMedia));
			}
		}

		public async Task DownloadSnapBlobAsync(SnapchatManager manager)
		{
			if (HasMedia) return;

			// Start the download
			try
			{
				await Blob.DeleteBlobFromStorageAsync(MediaId, BlobType.ChatMessageMedia);
				var mediaBlob = await manager.Endpoints.GetChatMediaAsync(MediaId, Iv, Key);
				await Blob.SaveBlobToStorageAsync(mediaBlob, MediaId, BlobType.ChatMessageMedia);
			}
			catch (InvalidHttpResponseException exception)
			{
				if (exception.Message == "Gone")
				{
					return;
				}

				SnazzyDebug.WriteLine(exception);
			}
			catch (Exception exception)
			{
				SnazzyDebug.WriteLine(exception);
			}
		}

		public async Task<byte[]> OpenSnapBlobAsync()
		{
			return await Blob.ReadBlobFromStorageAsync(MediaId, BlobType.ChatMessageMedia);
		}

		#endregion
	}

	[DataContract]
	public class Header
		: NotifyPropertyChangedBase
	{
		[DataMember(Name = "conv_id")]
		public String ConversationId { get; set; }

		[DataMember(Name = "from")]
		public String From { get; set; }

		[DataMember(Name = "to")]
		public ObservableCollection<String> To { get; set; }
	}

	[DataContract]
	public class SavedState
		: NotifyPropertyChangedBase
	{
		[DataMember(Name = "saved")]
		public Boolean Saved { get; set; }

		[DataMember(Name = "version")]
		public Int16 Version { get; set; }
	}
}
