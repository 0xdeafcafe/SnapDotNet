using System;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Snapchat.SnapLogic.Api;
using Snapchat.SnapLogic.Api.Exceptions;
using Snapchat.SnapLogic.Helpers;
using SnapDotNet.Core.Miscellaneous.CustomTypes;
using SnapDotNet.Core.Miscellaneous.Helpers;
using SnapDotNet.Core.Miscellaneous.Models;
using Windows.UI.Xaml;

namespace Snapchat.Models
{
	public enum MessageBodyType
	{
		Text,
		Screenshot,
		Media
	}

	public class ChatMessage
		: NotifyPropertyChangedBase, IConversationItem
	{
		public ChatMessage()
		{
			_savedStates.MapChanged += (sender, args) => NotifyPropertyChanged("SavedStates");
		}

		public Body Body
		{
			get { return _body; }
			set { SetField(ref _body, value); }
		}
		private Body _body;

		public String ChatMessageId
		{
			get { return _chatMessageId; }
			set { SetField(ref _chatMessageId, value); }
		}
		private String _chatMessageId;

		public Header Header
		{
			get { return _header; }
			set { SetField(ref _header, value); }
		}
		private Header _header;

		public String Id
		{
			get { return _id; }
			set { SetField(ref _id, value); }
		}
		private String _id;

		public ObservableDictionary<string, SavedState> SavedStates
		{
			get { return _savedStates; }
			set { SetField(ref _savedStates, value); }
		}
		private ObservableDictionary<string, SavedState> _savedStates = new ObservableDictionary<string, SavedState>();

		public Boolean Saved
		{
			get { return _saved; }
			set { SetField(ref _saved, value); }
		}
		private Boolean _saved;

		public String SequenceNumber
		{
			get { return _sequenceNumber; }
			set { SetField(ref _sequenceNumber, value); }
		}
		private String _sequenceNumber;

		public DateTime PostedAt
		{
			get { return _postedAt; }
			set { SetField(ref _postedAt, value); }
		}
		private DateTime _postedAt;

		public String Type
		{
			get { return _type; }
			set { SetField(ref _type, value); }
		}
		private String _type;

		public void CreateFromServer(SnapLogic.Models.New.ChatMessage serverChatMessage)
		{
			Body = new Body
			{
				Type = serverChatMessage.Body.Type,
				Media = new BodyMedia
				{
					Iv = (serverChatMessage.Body.Media == null) ? null : serverChatMessage.Body.Media.Iv,
					Key = (serverChatMessage.Body.Media == null) ? null : serverChatMessage.Body.Media.Key,
					MediaId = (serverChatMessage.Body.Media == null) ? null : serverChatMessage.Body.Media.MediaId
				},
				Text = serverChatMessage.Body.Text
			};
			ChatMessageId = serverChatMessage.ChatMessageId;
			Id = serverChatMessage.Id;
			Type = serverChatMessage.Type;
			SequenceNumber = serverChatMessage.SequenceNumber;
			Header = new Header
			{
				ConversationId = serverChatMessage.Header.ConversationId,
				From = serverChatMessage.Header.From,
				To = serverChatMessage.Header.To
			};
			PostedAt = serverChatMessage.PostedAt;
			Saved = serverChatMessage.SavedStates != null && serverChatMessage.SavedStates[App.SnapchatManager.Username].Saved;
			SavedStates = new ObservableDictionary<string, SavedState>();
			foreach (var savedState in serverChatMessage.SavedStates ?? new ObservableDictionary<string, SnapLogic.Models.New.SavedState>())
				SavedStates.Add(savedState.Key, new SavedState { Saved = savedState.Value.Saved, Version = savedState.Value.Version });
		}

		#region Helpers

		[IgnoreDataMember]
		public ControlTemplate IconResource
		{
			get
			{
				var resourceName = "StatusIcon{0}{1}";
				var status = SavedStates[App.SnapchatManager.Account.Username].Version == 0 ? "Delivered" : "Opened";

				resourceName = Body.Type == MessageBodyType.Screenshot
					? String.Format(resourceName, "ChatScreenshot", "")
					: String.Format(resourceName, IsIncoming ? "Incoming" : "Outgoing", status);

				return (ControlTemplate) Application.Current.Resources[resourceName];
			}
		}

		[IgnoreDataMember]
		public SolidColorBrush IconColourBrush
		{
			get { return new SolidColorBrush(Color.FromArgb(0xFF, 0x3C, 0xB2, 0xE2)); }
		}

		[IgnoreDataMember]
		public Boolean IsIncoming
		{
			get { return App.SnapchatManager.Account.Username != Sender; }
		}

		[IgnoreDataMember]
		public String Sender
		{
			get { return Header.From; }
		}

		#endregion
	}

	public class Body
		: NotifyPropertyChangedBase
	{
		public String Text
		{
			get { return _text; }
			set { SetField(ref _text, value); }
		}
		private String _text;

		public BodyMedia Media
		{
			get { return _media; }
			set { SetField(ref _media, value); }
		}
		private BodyMedia _media;

		public MessageBodyType Type
		{
			get { return _type; }
			set { SetField(ref _type, value); }
		}
		private MessageBodyType _type;
	}

	public class BodyMedia
		: NotifyPropertyChangedBase
	{
		public String Iv
		{
			get { return _iv; }
			set { SetField(ref _iv, value); }
		}
		private String _iv;

		public String Key
		{
			get { return _key; }
			set { SetField(ref _key, value); }
		}
		private String _key;

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

	public class Header
		: NotifyPropertyChangedBase
	{
		public Header()
		{
			_to.CollectionChanged += (sender, args) => NotifyPropertyChanged("To");
		}

		public String ConversationId
		{
			get { return _conversationId; }
			set { SetField(ref _conversationId, value); }
		}
		private String _conversationId;

		public String From
		{
			get { return _from; }
			set { SetField(ref _from, value); }
		}
		private String _from;

		public ObservableCollection<String> To
		{
			get { return _to; }
			set { SetField(ref _to, value); }
		}
		private ObservableCollection<String> _to = new ObservableCollection<string>();
	}

	public class SavedState
		: NotifyPropertyChangedBase
	{
		public Boolean Saved
		{
			get { return _saved; }
			set { SetField(ref _saved, value); }
		}
		private Boolean _saved;

		public Int16 Version
		{
			get { return _version; }
			set { SetField(ref _version, value); }
		}
		private Int16 _version;
	}
}
