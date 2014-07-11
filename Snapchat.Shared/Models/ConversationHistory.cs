using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using SnapDotNet.Core.Miscellaneous.CustomTypes;
using SnapDotNet.Core.Miscellaneous.Models;

namespace Snapchat.Models
{
	public enum ConversationType
	{
		Person2Person,
		PendingUpload
	}

	public interface IConversation
	{
		ConversationType ConversationType { get; set; }

		String Id { get; set; }
	}

	public class Conversation
		: NotifyPropertyChangedBase, IConversation
	{
		public Conversation()
		{
			_participants.CollectionChanged += (sender, args) => NotifyPropertyChanged("Participants");
		}

		public ConversationType ConversationType
		{
			get { return _conversationType; }
			set { SetField(ref _conversationType, value); }
		}
		private ConversationType _conversationType;

		public ConversationMessages ConversationMessages
		{
			get { return _conversationMessages; }
			set { SetField(ref _conversationMessages, value); }
		}
		private ConversationMessages _conversationMessages;

		public ConversationState ConversationState
		{
			get { return _conversationState; }
			set { SetField(ref _conversationState, value); }
		}
		private ConversationState _conversationState;

		public LastChatActions LastChatActions
		{
			get { return _lastChatActions; }
			set { SetField(ref _lastChatActions, value); }
		}
		private LastChatActions _lastChatActions;

		public String Id
		{
			get { return _id; }
			set { SetField(ref _id, value); }
		}
		private String _id;

		public String IterToken
		{
			get { return _iterToken; }
			set { SetField(ref _iterToken, value); }
		}
		private String _iterToken;

		public DateTime LastInteraction
		{
			get { return _lastInteraction; }
			set { SetField(ref _lastInteraction, value); }
		}
		private DateTime _lastInteraction;

		public ObservableCollection<String> PendingChatMessages
		{
			get { return _pendingChatMessages; }
			set { SetField(ref _pendingChatMessages, value); }
		}
		private ObservableCollection<String> _pendingChatMessages = new ObservableCollection<String>();

		public ObservableCollection<String> Participants
		{
			get { return _participants; }
			set { SetField(ref _participants, value); }
		}
		private ObservableCollection<String> _participants = new ObservableCollection<String>();
}

	public class PendingUpload
		: NotifyPropertyChangedBase, IConversation
	{
		public ConversationType ConversationType
		{
			get { return _conversationType; }
			set { SetField(ref _conversationType, value); }
		}
		private ConversationType _conversationType;

		public String Id
		{
			get { return _id; }
			set { SetField(ref _id, value); }
		}
		private String _id;

		// List of Recipients
		// Bool saying if it's posting to the Story or not
		// Status of the upload (pending, failed)
		// Media Id (can be null)
		// Id of the media to access the local file
		// Timestamp of last action
	}

	public class ConversationMessages
		: NotifyPropertyChangedBase
	{
		public ConversationMessages()
		{
			_snaps.CollectionChanged += (sender, args) => { NotifyPropertyChanged("Chats"); NotifyPropertyChanged("Snaps"); NotifyPropertyChanged("SortedMessages"); };
			_chats.CollectionChanged += (sender, args) => { NotifyPropertyChanged("Snaps"); NotifyPropertyChanged("Chats"); NotifyPropertyChanged("SortedMessages"); };
		}

		public ObservableCollection<Snap> Snaps
		{
			get { return _snaps; }
			set
			{
				SetField(ref _snaps, value);
				NotifyPropertyChanged("SortedMessages");
				NotifyPropertyChanged("Chats");
			}
		}
		private ObservableCollection<Snap> _snaps = new ObservableCollection<Snap>();

		public ObservableCollection<ChatMessage> Chats
		{
			get { return _chats; }
			set
			{
				SetField(ref _chats, value);
				NotifyPropertyChanged("SortedMessages");
				NotifyPropertyChanged("Snaps");
			}
		}
		private ObservableCollection<ChatMessage> _chats = new ObservableCollection<ChatMessage>();

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

				var items = new List<IConversationItem>();
				items.AddRange(Chats);
				items.AddRange(Snaps);
				items = items.OrderByDescending(i => i.PostedAt).ToList();

				foreach (var message in items)
				{
					var postedAt = message.PostedAt;
					var sentBy = message.Sender;

					// Check if time has changed
					if (lastItemDateTime.Year != postedAt.Year ||
						lastItemDateTime.Month != postedAt.Month ||
						lastItemDateTime.Day != postedAt.Day)
						// New Time!
						conversationThread.Add(new TimeSeperator(lastItemDateTime = postedAt));

					if (lastUsername == null ||
						lastUsername != sentBy)
					{
						// Add User
						if (currentUserData != null)
							conversationThread.Add(currentUserData);

						currentUserData = new UserHeader
						{
							User = lastUsername = sentBy,
							Messages = new ObservableCollection<IConversationItem>()
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
	
	public class MessageContainer
		: NotifyPropertyChangedBase
	{
		public Snap Snap
		{
			get { return _snap; }
			set { SetField(ref _snap, value); }
		}
		private Snap _snap;

		public ChatMessage ChatMessage
		{
			get { return _chatMessage; }
			set { SetField(ref _chatMessage, value); }
		}
		private ChatMessage _chatMessage;

		#region Helper

		public IConversationItem MessageMetaData
		{
			get { return Snap ?? (IConversationItem) ChatMessage; }
		}

		#endregion
	}

	public class MessagingAuthentication
		: NotifyPropertyChangedBase
	{
		public String Mac
		{
			get { return _mac; }
			set { SetField(ref _mac, value); }
		}
		private String _mac;

		public String Payload
		{
			get { return _payload; }
			set { SetField(ref _payload, value); }
		}
		private String _payload;
	}

	public class ConversationState
		: NotifyPropertyChangedBase
	{
		public ConversationState()
		{
			// User Chat Releases Inpc
			_userChatReleases.MapChanged += (sender, args) => NotifyPropertyChanged("UserChatReleases");
			foreach (var userChatRelease in _userChatReleases.Values) userChatRelease.MapChanged += (sender, args) => NotifyPropertyChanged("UserChatReleases");

			// User Sequences Inpc
			_userSequences.MapChanged += (sender, args) => NotifyPropertyChanged("UserSequences");

			// User Snap Releases Inpc
			_userSnapReleases.MapChanged += (sender, args) => NotifyPropertyChanged("UserSnapReleases");
			foreach (var userSnapRelease in _userSnapReleases.Values) userSnapRelease.MapChanged += (sender, args) => NotifyPropertyChanged("UserSnapReleases");
		}

		public ObservableDictionary<string, ObservableDictionary<string, Int64>> UserChatReleases
		{
			get { return _userChatReleases; }
			set { SetField(ref _userChatReleases, value); }
		}
		private ObservableDictionary<string, ObservableDictionary<string, Int64>> _userChatReleases = new ObservableDictionary<string, ObservableDictionary<string, Int64>>();

		public ObservableDictionary<string, int> UserSequences
		{
			get { return _userSequences; }
			set { SetField(ref _userSequences, value); }
		}
		private ObservableDictionary<string, int> _userSequences = new ObservableDictionary<string, int>();

		public ObservableDictionary<string, ObservableDictionary<string, Int64>> UserSnapReleases
		{
			get { return _userSnapReleases; }
			set { SetField(ref _userSnapReleases, value); }
		}
		private ObservableDictionary<string, ObservableDictionary<string, Int64>> _userSnapReleases = new ObservableDictionary<string, ObservableDictionary<string, Int64>>();
	}

	public class LastChatActions
		: NotifyPropertyChangedBase
	{
		public DateTime LastRead
		{
			get { return _lastRead; }
			set { SetField(ref _lastRead, value); }
		}
		private DateTime _lastRead;

		public String LastReader
		{
			get { return _lastReader; }
			set { SetField(ref _lastReader, value); }
		}
		private String _lastReader;

		public DateTime LastWrite
		{
			get { return _lastWrite; }
			set { SetField(ref _lastWrite, value); }
		}
		private DateTime _lastWrite;

		public String LastWriteType
		{
			get { return _lastWriteType; }
			set { SetField(ref _lastWriteType, value); }
		}
		private String _lastWriteType;

		public String LastWriter
		{
			get { return _lastWriter; }
			set { SetField(ref _lastWriter, value); }
		}
		private String _lastWriter;
	}
}
