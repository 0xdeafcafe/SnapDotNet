using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using SnapDotNet.Core.Miscellaneous.CustomTypes;
using SnapDotNet.Core.Miscellaneous.Models;
using SnapDotNet.Core.Snapchat.Converters.Json;
using SnapDotNet.Core.Snapchat.Models.AppSpecific;

namespace SnapDotNet.Core.Snapchat.Models.New
{
	[DataContract]
	public class ConversationResponse
		: NotifyPropertyChangedBase
	{
		public ConversationResponse()
		{
			_participants.CollectionChanged += (sender, args) => NotifyPropertyChanged("Participants");
			_pendingChatsFor.CollectionChanged += (sender, args) => NotifyPropertyChanged("PendingChatsFor");
			_pendingReceivedSnaps.CollectionChanged += (sender, args) =>
			{
				NotifyPropertyChanged("PendingReceivedSnaps"); 
				NotifyPropertyChanged("LastPendingSnap"); 
				NotifyPropertyChanged("PendingSnapCount");
			};
		}

		[DataMember(Name = "conversation_messages")]
		public ConversationMessages ConversationMessages
		{
			get { return _conversationMessages; }
			set { SetField(ref _conversationMessages, value); }
		}
		private ConversationMessages _conversationMessages;

		[DataMember(Name = "conversation_state")]
		public ConversationState ConversationState
		{
			get { return _conversationState; }
			set { SetField(ref _conversationState, value); }
		}
		private ConversationState _conversationState;

		[DataMember(Name = "last_chat_actions")]
		public LastSnapActions LastChatActions
		{
			get { return _lastChatActions; }
			set { SetField(ref _lastChatActions, value); }
		}
		private LastSnapActions _lastChatActions;

		[DataMember(Name = "id")]
		public String Id
		{
			get { return _id; }
			set { SetField(ref _id, value); }
		}
		private String _id;

		[DataMember(Name = "iter_token")]
		public String IterToken
		{
			get { return _iterToken; }
			set { SetField(ref _iterToken, value); }
		}
		private String _iterToken;

		[DataMember(Name = "last_interaction_ts")]
		[JsonConverter(typeof(UnixDateTimeConverter))]
		public DateTime LastInteraction
		{
			get { return _lastInteraction; }
			set { SetField(ref _lastInteraction, value); }
		}
		private DateTime _lastInteraction;

		[DataMember(Name = "last_snap")]
		public Snap LastSnap
		{
			get { return _lastSnap; }
			set { SetField(ref _lastSnap, value); }
		}
		private Snap _lastSnap;

		[DataMember(Name = "participants")]
		public ObservableCollection<String> Participants
		{
			get { return _participants; }
			set { SetField(ref _participants, value); }
		}
		private ObservableCollection<String> _participants = new ObservableCollection<String>();

		[DataMember(Name = "pending_chats_for")]
		public ObservableCollection<String> PendingChatsFor
		{
			get { return _pendingChatsFor; }
			set { SetField(ref _pendingChatsFor, value); }
		}
		private ObservableCollection<String> _pendingChatsFor = new ObservableCollection<String>();

		[DataMember(Name = "pending_received_snaps")]
		public ObservableCollection<Snap> PendingReceivedSnaps
		{
			get { return _pendingReceivedSnaps; }
			set
			{
				SetField(ref _pendingReceivedSnaps, value);
				NotifyPropertyChanged("LastPendingSnap");
				NotifyPropertyChanged("PendingSnapCount");
			}
		}
		private ObservableCollection<Snap> _pendingReceivedSnaps = new ObservableCollection<Snap>();

		#region Helpers

		public Snap LastPendingSnap { get { return PendingReceivedSnaps.Reverse().First(); } }

		public Int32 PendingSnapCount { get { return PendingReceivedSnaps.Count; } }

		public Boolean HasPendingSnaps { get { return (PendingReceivedSnaps != null && PendingReceivedSnaps.Any()); } }

		#endregion
	}

	[DataContract]
	public class ConversationMessages
		: NotifyPropertyChangedBase
	{
		public ConversationMessages()
		{
			_messages.CollectionChanged += (sender, args) => { NotifyPropertyChanged("Messages"); NotifyPropertyChanged("SortedMessages"); };
		}

		[DataMember(Name = "messages")]
		public ObservableCollection<MessageContainer> Messages
		{
			get { return _messages; }
			set
			{
				SetField(ref _messages, value);
				NotifyPropertyChanged("SortedMessages");
			}
		}
		private ObservableCollection<MessageContainer> _messages = new ObservableCollection<MessageContainer>();
		
		[DataMember(Name = "messaging_auth")]
		public MessagingAuthentication MessagingAuthentication
		{
			get { return _messagingAuthentication; }
			set { SetField(ref _messagingAuthentication, value); }
		}
		private MessagingAuthentication _messagingAuthentication;

		#region Helpers

		[IgnoreDataMember]
		public ObservableCollection<IConversationThreadItem> SortedMessages
		{
			get
			{
				string lastUsername = null;
				var lastItemDateTime = new DateTime(1994, 08, 18);
				var conversationThread = new ObservableCollection<IConversationThreadItem>();
				UserHeader currentUserData = null;

				foreach (var message in Messages.Reverse())
				{
					var messageMeta = message.MessageMetaData;
					var senderName = messageMeta.Sender.ToLowerInvariant();

					// Check if time has changed
					if (lastItemDateTime.Year != messageMeta.PostedAt.Year ||
					    lastItemDateTime.Month != messageMeta.PostedAt.Month ||
					    lastItemDateTime.Day != messageMeta.PostedAt.Day)
						// New Time!
						conversationThread.Add(new TimeSeperator(lastItemDateTime = messageMeta.PostedAt));

					if (lastUsername == null ||
						lastUsername != senderName)
					{
						// Add User
						if (currentUserData != null)
							conversationThread.Add(currentUserData);

						currentUserData = new UserHeader
						{
							User = lastUsername = senderName,
							Messages = new ObservableCollection<MessageContainer>()
						};
					}

					currentUserData.Messages.Add(message);
				}

				if (currentUserData != null && currentUserData.Messages.Any())
					conversationThread.Add(currentUserData);

				return conversationThread;
			}
		}

		#endregion
	}

	[DataContract]
	public class MessageContainer
		: NotifyPropertyChangedBase
	{
		[DataMember(Name = "snap")]
		public Snap Snap
		{
			get { return _snap; }
			set { SetField(ref _snap, value); }
		}
		private Snap _snap;

		[DataMember(Name = "chat_message")]
		public ChatMessage ChatMessage
		{
			get { return _chatMessage; }
			set { SetField(ref _chatMessage, value); }
		}
		private ChatMessage _chatMessage;

		[DataMember(Name = "iter_token")]
		public String IterToken
		{
			get { return _iterToken; }
			set { SetField(ref _iterToken, value); }
		}
		private String _iterToken;

		#region Helper
		
		public IConversationItem MessageMetaData
		{
			get { return (IConversationItem)Snap ?? (IConversationItem)ChatMessage; }
		}

		#endregion
	}

	[DataContract]
	public class MessagingAuthentication
		: NotifyPropertyChangedBase
	{
		[DataMember(Name = "mac")]
		public String Mac
		{
			get { return _mac; }
			set { SetField(ref _mac, value); }
		}
		private String _mac;

		[DataMember(Name = "payload")]
		public String Payload
		{
			get { return _payload; }
			set { SetField(ref _payload, value); }
		}
		private String _payload;
	}

	[DataContract]
	public class ConversationState
		: NotifyPropertyChangedBase
	{
		public ConversationState()
		{
			// User Chat Releases Inpc
			_userChatReleases.CollectionChanged += (sender, args) => NotifyPropertyChanged("UserChatReleases");
			foreach (var userChatRelease in _userChatReleases.Values) userChatRelease.CollectionChanged += (sender, args) => NotifyPropertyChanged("UserChatReleases");

			// User Sequences Inpc
			_userSequences.CollectionChanged += (sender, args) => NotifyPropertyChanged("UserSequences");

			// User Snap Releases Inpc
			_userSnapReleases.CollectionChanged += (sender, args) => NotifyPropertyChanged("UserSnapReleases");
			foreach (var userSnapRelease in _userSnapReleases.Values) userSnapRelease.CollectionChanged += (sender, args) => NotifyPropertyChanged("UserSnapReleases");
		}

		[DataMember(Name = "user_chat_releases")]
		public ObservableDictionary<string, ObservableDictionary<string, Int64>> UserChatReleases
		{
			get { return _userChatReleases; }
			set { SetField(ref _userChatReleases, value); }
		}
		private ObservableDictionary<string, ObservableDictionary<string, Int64>> _userChatReleases = new ObservableDictionary<string, ObservableDictionary<string, Int64>>();

		[DataMember(Name = "user_sequences")]
		public ObservableDictionary<string, int> UserSequences
		{
			get { return _userSequences; }
			set { SetField(ref _userSequences, value); }
		}
		private ObservableDictionary<string, int> _userSequences = new ObservableDictionary<string, int>();

		[DataMember(Name = "user_snap_releases")]
		public ObservableDictionary<string, ObservableDictionary<string, Int64>> UserSnapReleases
		{
			get { return _userSnapReleases; }
			set { SetField(ref _userSnapReleases, value); }
		}
		private ObservableDictionary<string, ObservableDictionary<string, Int64>> _userSnapReleases = new ObservableDictionary<string, ObservableDictionary<string, Int64>>();
	}

	[DataContract]
	public class LastSnapActions
		: NotifyPropertyChangedBase
	{
		[DataMember(Name = "last_read_timestamp")]
		[JsonConverter(typeof(UnixDateTimeConverter))]
		public DateTime LastRead
		{
			get { return _lastRead; }
			set { SetField(ref _lastRead, value); }
		}
		private DateTime _lastRead;

		[DataMember(Name = "last_reader")]
		public String LastReader
		{
			get { return _lastReader; }
			set { SetField(ref _lastReader, value); }
		}
		private String _lastReader;

		[DataMember(Name = "last_write")]
		[JsonConverter(typeof (UnixDateTimeConverter))]
		public DateTime LastWrite
		{
			get { return _lastWrite; }
			set { SetField(ref _lastWrite, value); }
		}
		private DateTime _lastWrite;

		[DataMember(Name = "last_write_type")]
		public String LastWriteType
		{
			get { return _lastWriteType; }
			set { SetField(ref _lastWriteType, value); }
		}
		private String _lastWriteType;

		[DataMember(Name = "last_writer")]
		public String LastWriter
		{
			get { return _lastWriter; }
			set { SetField(ref _lastWriter, value); }
		}
		private String _lastWriter;
	}
}
