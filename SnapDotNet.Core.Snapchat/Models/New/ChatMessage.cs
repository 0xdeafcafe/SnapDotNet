using System;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SnapDotNet.Core.Miscellaneous.CustomTypes;
using SnapDotNet.Core.Miscellaneous.Helpers;
using SnapDotNet.Core.Miscellaneous.Helpers.Async;
using SnapDotNet.Core.Miscellaneous.Models;
using SnapDotNet.Core.Snapchat.Api;
using SnapDotNet.Core.Snapchat.Api.Exceptions;
using SnapDotNet.Core.Snapchat.Converters.Json;
using SnapDotNet.Core.Snapchat.Helpers;

namespace SnapDotNet.Core.Snapchat.Models.New
{
	[DataContract]
	public class ChatMessage
		: NotifyPropertyChangedBase, IConversationItem
	{
		public ChatMessage()
		{
			_savedStates.MapChanged += (sender, args) => NotifyPropertyChanged("SavedStates");
		}

		[DataMember(Name = "body")]
		public Body Body
		{
			get { return _body; }
			set { SetField(ref _body, value); }
		}
		private Body _body;

		[DataMember(Name = "chat_message_id")]
		public String ChatMessageId
		{
			get { return _chatMessageId; }
			set { SetField(ref _chatMessageId, value); }
		}
		private String _chatMessageId;

		[DataMember(Name = "header")]
		public Header Header
		{
			get { return _header; }
			set { SetField(ref _header, value); }
		}
		private Header _header;

		[DataMember(Name = "id")]
		public String Id
		{
			get { return _id; }
			set { SetField(ref _id, value); }
		}
		private String _id;

		[DataMember(Name = "saved_state")]
		public ObservableDictionary<string, SavedState> SavedStates
		{
			get { return _savedStates; }
			set { SetField(ref _savedStates, value); }
		}
		private ObservableDictionary<string, SavedState> _savedStates = new ObservableDictionary<string, SavedState>();
		
		[DataMember(Name = "seq_num")]
		public String SequenceNumber
		{
			get { return _sequenceNumber; }
			set { SetField(ref _sequenceNumber, value); }
		}
		private String _sequenceNumber;

		[DataMember(Name = "timestamp")]
		[JsonConverter(typeof(UnixDateTimeConverter))]
		public DateTime PostedAt
		{
			get { return _postedAt; }
			set { SetField(ref _postedAt, value); }
		}
		private DateTime _postedAt;

		[DataMember(Name = "type")]
		public String Type
		{
			get { return _type; }
			set { SetField(ref _type, value); }
		}
		private String _type;

		#region Helpers

		[IgnoreDataMember]
		public String Sender
		{
			get { return Header.From; }
		}

		#endregion
	}

	public enum MessageBodyType
	{
		Text,
		Screenshot,
		Media
	}

	[DataContract]
	public class Body
		: NotifyPropertyChangedBase
	{
		[DataMember(Name = "text")]
		public String Text
		{
			get { return _text; }
			set { SetField(ref _text, value); }
		}
		private String _text;

		[DataMember(Name = "media")]
		public BodyMedia Media
		{
			get { return _media; }
			set { SetField(ref _media, value); }
		}
		private BodyMedia _media;

		[DataMember(Name = "type")]
		public MessageBodyType Type
		{
			get { return _type; }
			set { SetField(ref _type, value); }
		}
		private MessageBodyType _type;
	}

	[DataContract]
	public class BodyMedia
		: NotifyPropertyChangedBase
	{
		[DataMember(Name = "iv")]
		public String Iv
		{
			get { return _iv; }
			set { SetField(ref _iv, value); }
		}
		private String _iv;

		[DataMember(Name = "key")]
		public String Key
		{
			get { return _key; }
			set { SetField(ref _key, value); }
		}
		private String _key;

		[DataMember(Name = "media_id")]
		public String MediaId
		{
			get { return _mediaId; }
			set { SetField(ref _mediaId, value); }
		}
		private String _mediaId;

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
		public Header()
		{
			_to.CollectionChanged += (sender, args) => NotifyPropertyChanged("To");
		}

		[DataMember(Name = "conv_id")]
		public String ConversationId
		{
			get { return _conversationId; }
			set { SetField(ref _conversationId, value); }
		}
		private String _conversationId;

		[DataMember(Name = "from")]
		public String From
		{
			get { return _from; }
			set { SetField(ref _from, value); }
		}
		private String _from;

		[DataMember(Name = "to")]
		public ObservableCollection<String> To
		{
			get { return _to; }
			set { SetField(ref _to, value); }
		}
		private ObservableCollection<String> _to = new ObservableCollection<string>();
	}

	[DataContract]
	public class SavedState
		: NotifyPropertyChangedBase
	{
		[DataMember(Name = "saved")]
		public Boolean Saved
		{
			get { return _saved; }
			set { SetField(ref _saved, value); }
		}
		private Boolean _saved;

		[DataMember(Name = "version")]
		public Int16 Version
		{
			get { return _version; }
			set { SetField(ref _version, value); }
		}
		private Int16 _version;
	}
}
