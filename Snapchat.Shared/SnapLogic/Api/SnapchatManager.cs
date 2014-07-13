using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Snapchat.Models;
using Snapchat.SnapLogic.Models.New;
using SnapDotNet.Core.Miscellaneous.CustomTypes;
using SnapDotNet.Core.Miscellaneous.Extensions;
using SnapDotNet.Core.Miscellaneous.Helpers;
using SnapDotNet.Core.Miscellaneous.Helpers.Storage;
using SnapDotNet.Core.Miscellaneous.Models;
using ChatMessage = Snapchat.Models.ChatMessage;
using LastChatActions = Snapchat.Models.LastChatActions;
using Snap = Snapchat.Models.Snap;

namespace Snapchat.SnapLogic.Api
{
	public sealed class SnapchatManager
		: NotifyPropertyChangedBase
	{
		private const string AccountDataFileName = "user_data.json";
		private const string RoamingSnapchatDataContainer = "SnapchatData";

		public SnapchatData SnapchatData
		{
			get { return _snapchatData; }
			set { SetField(ref _snapchatData, value); }
		}
		private SnapchatData _snapchatData;

		public String AuthToken
		{
			get
			{
				if (SnapchatData != null && SnapchatData.UserAccount != null)
					return SnapchatData.UserAccount.AuthToken;
				
				return null;
			}
		}

		public String Username
		{
			get
			{
				if (SnapchatData != null && SnapchatData.UserAccount != null)
					return SnapchatData.UserAccount.Username;

				return null;
			}
		}

		public Account Account
		{
			get { return SnapchatData.UserAccount; }
		}

		public StoriesResponse Stories
		{
			get { return SnapchatData.StoriesResponse; }
		}

		public Endpoints Endpoints
		{
			get { return _endpoints; }
			set { SetField(ref _endpoints, value); }
		}
		private Endpoints _endpoints;

		public bool Loaded { get; private set; }

		#region Constructors

		/// <summary>
		/// 
		/// </summary>
		public SnapchatManager()
		{
			Endpoints = new Endpoints(this);

			//Load();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="username"></param>
		/// <param name="authToken"></param>
		/// <param name="getUpdates"></param>
		public SnapchatManager(string username, string authToken, bool getUpdates = false)
		{
			Endpoints = new Endpoints(this);

			UpdateUsername(username);
			UpdateAuthToken(authToken);

			if (!getUpdates) return;

			Endpoints.GetAllUpdatesAsync().Wait();
		}

		#endregion
		
		#region Setters

		public async Task<bool> Logout()
		{
			try
			{
				await Endpoints.LogoutAsync();
			}
			catch (Exception ex)
			{
				SnazzyDebug.WriteLine(ex);
			}
			finally
			{
				SnapchatData = null;
				DeleteAsync();
			}
			return true;
		}

		public void UpdateAuthToken(string authToken)
		{
			if (SnapchatData == null)
				SnapchatData = new SnapchatData();
			if (SnapchatData.UserAccount == null)
				SnapchatData.UserAccount = new Account();

			SnapchatData.UserAccount.AuthToken = authToken;
		}

		public void UpdateUsername(string username)
		{
			if (SnapchatData == null)
				SnapchatData = new SnapchatData();
			if (SnapchatData.UserAccount == null)
				SnapchatData.UserAccount = new Account();
			SnapchatData.UserAccount.Username = username;
		}

		public void UpdatePublicActivities(ObservableDictionary<string, Models.New.Responses.PublicActivity> publicActivities)
		{
			foreach (var friend in SnapchatData.UserAccount.Friends)
			{
				if (friend.PublicActivity == null)
					friend.PublicActivity = new PublicActivity();

				if (!publicActivities.ContainsKey(friend.Name)) continue;

				var publicActivity = publicActivities[friend.Name];
				friend.PublicActivity.Score = publicActivity.Score;
				friend.PublicActivity.BestFriends.Clear();
				foreach (var bestFriend in publicActivity.BestFriends.Take(3))
					friend.PublicActivity.BestFriends.Add(bestFriend);
			}
		}

		public void Update(AllUpdatesResponse allUpdates)
		{
			// lets get messy ya'll
			if (SnapchatData == null)
				SnapchatData = new SnapchatData();

			SnapchatData.BackgroundFetchSecretKey = allUpdates.BackgroundFetchSecretKey;

			#region Update MessagingGatewayInfo

			if (SnapchatData.MessagingGatewayInfo == null)
				SnapchatData.MessagingGatewayInfo = new Snapchat.Models.MessagingGatewayInfo();

			var messagingGatewayInfo = SnapchatData.MessagingGatewayInfo;
			messagingGatewayInfo.GatewayAuthenticationToken = new Snapchat.Models.GatewayAuthenticationToken
			{
				Mac = allUpdates.MessagingGatewayInfo.GatewayAuthenticationToken.Mac,
				Payload = allUpdates.MessagingGatewayInfo.GatewayAuthenticationToken.Payload,
			};
			messagingGatewayInfo.GatewayServer = allUpdates.MessagingGatewayInfo.GatewayServer;

			#endregion

			#region Update Updates

			if (SnapchatData.UserAccount == null)
				SnapchatData.UserAccount = new Account();
			var updates = SnapchatData.UserAccount;
			var newUpdates = allUpdates.UpdatesResponse;

			updates.AccountPrivacy = newUpdates.AccountPrivacy;
			updates.AddedFriendsTimestamp = newUpdates.AddedFriendsTimestamp;
			updates.AuthToken = newUpdates.AuthToken;
			updates.CanViewMatureContent = newUpdates.CanViewMatureContent;
			updates.CountryCode = newUpdates.CountryCode;
			updates.CurrentTimestamp = newUpdates.CurrentTimestamp;
			updates.DeviceToken = newUpdates.DeviceToken;
			updates.Email = newUpdates.Email;
			updates.LastReplayedSnap = newUpdates.LastReplayedSnap;
			updates.Logged = newUpdates.Logged;
			updates.Mobile = newUpdates.Mobile;
			updates.MobileVerificationKey = newUpdates.MobileVerificationKey;
			updates.NotificationSoundSetting = newUpdates.NotificationSoundSetting;
			updates.NumberOfBestFriends = newUpdates.NumberOfBestFriends;
			updates.RecievedSnaps = newUpdates.RecievedSnaps;
			updates.Score = newUpdates.Score;
			updates.SearchableByPhoneNumber = newUpdates.SearchableByPhoneNumber;
			updates.SentSnaps = newUpdates.SentSnaps;
			updates.ShouldCallToVerifyNumber = newUpdates.ShouldCallToVerifyNumber;
			updates.ShouldSendTextToVerifyNumber = newUpdates.ShouldSendTextToVerifyNumber;
			updates.SnapchatPhoneNumber = newUpdates.SnapchatPhoneNumber;
			updates.StoryPrivacy = newUpdates.StoryPrivacy;
			updates.Username = newUpdates.Username;

			// Update Added Friends
			if (updates.AddedFriends == null) updates.AddedFriends = new ObservableCollection<AddedFriend>();
			updates.AddedFriends.Clear();
			foreach (var addedFriend in newUpdates.AddedFriends)
				updates.AddedFriends.Add(addedFriend);

			// Update Best Friends
			if (updates.BestFriends == null) updates.BestFriends = new ObservableCollection<String>();
			updates.BestFriends.Clear();
			foreach (var bestFriend in newUpdates.BestFriends)
				updates.BestFriends.Add(bestFriend);

			#region Update Friends

			// This is messy, so I'm wrapping it in a region
			if (updates.Friends == null) updates.Friends = new ObservableCollection<Friend>();
			var currentFriends = new ObservableCollection<String>();
			foreach (var newFriend in newUpdates.Friends)
			{
				currentFriends.Add(newFriend.Name);
				var oldFriend = updates.Friends.FirstOrDefault(f => f.Name == newFriend.Name);
				if (oldFriend == null)
					// this guy is a new guy!
					updates.Friends.Add(newFriend);
				else
				{
					oldFriend.CanSeeCustomStories = newFriend.CanSeeCustomStories;
					oldFriend.Direction = newFriend.Direction;
					oldFriend.Display = newFriend.Display;
					oldFriend.Name = newFriend.Name;
				}

			}

			// TODO: If someone can think of a better way to do this, pls do it
			var duplicateFriends = updates.Friends.ToList();
			foreach (var currentFriend in currentFriends)
			{
				var currentFriendSafe = currentFriend;
				var current = duplicateFriends.FirstOrDefault(f => f.Name == currentFriendSafe);
				if (current == null) updates.Friends.Remove(f => f.Name == currentFriendSafe);
				else duplicateFriends.Remove(current);
			}

			#endregion

			// Recent Friends
			if (updates.RecentFriends == null) updates.RecentFriends = new ObservableCollection<String>();
			updates.RecentFriends.Clear();
			foreach (var recentfriend in newUpdates.RecentFriends)
				updates.RecentFriends.Add(recentfriend);

			// Requests
			if (updates.Requests == null) updates.Requests = new ObservableCollection<String>();
			updates.Requests.Clear();
			foreach (var requestedFriend in newUpdates.Requests)
				updates.Requests.Add(requestedFriend);

			#endregion

			#region Update Conversations

			if (SnapchatData.Conversations == null)
				SnapchatData.Conversations = new ObservableCollection<IConversation>();

			// Add conversations that are not currently there
			foreach (var newConversation in allUpdates.ConversationResponse.Reverse())
			{
				var currentConversation = SnapchatData.Conversations.FirstOrDefault(c => c != null && c.Id == newConversation.Id) as Conversation;

				#region Add Missing Conversation

				if (currentConversation == null)
				{
					// Add Conversation
					currentConversation = new Conversation
					{
						ConversationState = new Snapchat.Models.ConversationState
						{
							UserChatReleases = newConversation.ConversationState.UserChatReleases,
							UserSnapReleases = newConversation.ConversationState.UserSnapReleases,
							UserSequences = newConversation.ConversationState.UserSequences
						},
						ConversationType = ConversationType.Person2Person,
						Id = newConversation.Id,
						IterToken = newConversation.Id,
						LastChatActions = new LastChatActions
						{
							LastRead = newConversation.LastChatActions != null ? newConversation.LastChatActions.LastRead : DateTime.Now,
							LastReader = newConversation.LastChatActions != null ? newConversation.LastChatActions.LastReader : null,
							LastWrite = newConversation.LastChatActions != null ? newConversation.LastChatActions.LastWrite : DateTime.Now,
							LastWriteType = newConversation.LastChatActions != null ? newConversation.LastChatActions.LastWriteType : null,
							LastWriter = newConversation.LastChatActions != null ? newConversation.LastChatActions.LastWriter : null,
						},
						LastInteraction = newConversation.LastInteraction,
						Participants = newConversation.Participants,
						PendingChatMessages = newConversation.PendingChatsFor,
						ConversationMessages = new Snapchat.Models.ConversationMessages
						{
							MessagingAuthentication = new Snapchat.Models.MessagingAuthentication
							{
								Mac = newConversation.ConversationMessages.MessagingAuthentication.Mac,
								Payload = newConversation.ConversationMessages.MessagingAuthentication.Payload
							},
							Chats = new ObservableCollection<ChatMessage>(),
							Snaps = new ObservableCollection<Snap>()
						}
					};

					foreach (var message in newConversation.ConversationMessages.Messages)
					{
						if (message.ChatMessage != null)
						{
							// Is a chat message
							var chatMessage = new ChatMessage();
							chatMessage.CreateFromServer(message.ChatMessage);
							currentConversation.ConversationMessages.Chats.Add(chatMessage);
						}
						else if (message.Snap != null)
						{
							// Is a snap
							var snap = new Snap();
							snap.CreateFromServer(message.Snap);
							if (snap.IsIncoming && snap.Status == SnapStatus.Delivered)
								currentConversation.ConversationMessages.NewSnaps.Add(snap);
							else
								currentConversation.ConversationMessages.Snaps.Add(snap);
						}
					}

					currentConversation.ConversationMessages.Snaps.SortDescending(s => s.PostedAt);
					currentConversation.ConversationMessages.Chats.SortDescending(s => s.PostedAt);
					currentConversation.ConversationMessages.NewSnaps.Sort(s => s.PostedAt);

					// No idea why, but apparently I need this check...
					if (currentConversation != null)
						SnapchatData.Conversations.Add(currentConversation);

					continue;
				}

				#endregion

				#region Update Existing Conversation

				// Update Conversation
				currentConversation.Id = newConversation.Id;
				currentConversation.IterToken = newConversation.IterToken;
				currentConversation.LastChatActions = new LastChatActions
				{
					LastRead = newConversation.LastChatActions != null ? newConversation.LastChatActions.LastRead : DateTime.Now,
					LastReader = newConversation.LastChatActions != null ? newConversation.LastChatActions.LastReader : null,
					LastWrite = newConversation.LastChatActions != null ? newConversation.LastChatActions.LastWrite : DateTime.Now,
					LastWriteType = newConversation.LastChatActions != null ? newConversation.LastChatActions.LastWriteType : null,
					LastWriter = newConversation.LastChatActions != null ? newConversation.LastChatActions.LastWriter : null,
				};
				currentConversation.LastInteraction = newConversation.LastInteraction;
				currentConversation.Participants = newConversation.Participants;
				currentConversation.ConversationState = new Snapchat.Models.ConversationState
				{
					UserChatReleases = newConversation.ConversationState.UserChatReleases,
					UserSnapReleases = newConversation.ConversationState.UserSnapReleases,
					UserSequences = newConversation.ConversationState.UserSequences
				};
				currentConversation.PendingChatMessages = newConversation.PendingChatsFor;
				currentConversation.ConversationType = ConversationType.Person2Person;
				if (currentConversation.ConversationMessages == null)
					currentConversation.ConversationMessages = new Snapchat.Models.ConversationMessages();
				if (currentConversation.ConversationMessages.Chats == null)
					currentConversation.ConversationMessages.Chats = new ObservableCollection<ChatMessage>();
				if (currentConversation.ConversationMessages.Snaps == null)
					currentConversation.ConversationMessages.Snaps = new ObservableCollection<Snap>();

				// Messages
				foreach (var message in newConversation.ConversationMessages.Messages)
				{
					if (message.ChatMessage != null)
					{
						// We a chat message
						var existingChatMessage = currentConversation.ConversationMessages.Chats.FirstOrDefault(s => s.Id == message.ChatMessage.Id);
						if (existingChatMessage != null) continue; // TODO: Does anything here need updating? Don't think so xox (inside and else)

						// Insert chat
						var chat = new ChatMessage();
						chat.CreateFromServer(message.ChatMessage);
						currentConversation.ConversationMessages.Chats.Add(chat);
					}
					else if (message.Snap != null)
					{
						// We a snap
						var existingSnap = currentConversation.ConversationMessages.Snaps.FirstOrDefault(s => s.Id == message.Snap.Id);
						if (existingSnap == null)
						{
							// Insert snap
							var snap = new Snap();
							snap.CreateFromServer(message.Snap);
							currentConversation.ConversationMessages.Snaps.Add(snap);
						}
						else if (!message.Snap.IsIncoming)
						{
							if (existingSnap.Status == message.Snap.Status) continue;

							// Update status, and timestamps
							existingSnap.Status = message.Snap.Status;
							existingSnap.Timestamp = message.Snap.Timestamp;
						}
					}
				}

				currentConversation.ConversationMessages.Chats.SortDescending(c => c.PostedAt);
				currentConversation.ConversationMessages.Snaps.SortDescending(s => s.PostedAt);

				// No idea why, but apparently I need this check...
				if (currentConversation != null)
					SnapchatData.Conversations.Add(currentConversation);

				#endregion
			}

			SnapchatData.Conversations.SortDescending(c => c.LastInteraction);

			#endregion

			// TODO: finish this
			SnapchatData.StoriesResponse = allUpdates.StoriesResponse;
		}

		#endregion

		#region Cool Functions

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public bool IsAuthenticated()
		{
			return (SnapchatData != null && SnapchatData.UserAccount != null && SnapchatData.UserAccount.Logged);
		}

		/// <summary>
		/// Creates a MediaId used for Snaps, Stories and Conversations
		/// </summary>
		public string GenerateMediaId()
		{
			return !IsAuthenticated() ? null : String.Format("{0}~{1}", Username.ToUpperInvariant(), Guid.NewGuid());
		}

		#endregion
		
		#region Actions

		#region Actions:Save

		public async Task SaveAsync()
		{
			await SaveSnapchatDataAsync();
		}

		public async Task SaveSnapchatDataAsync()
		{
			// Seralize the Account model and save as json string in Isolated Storage
			await IsolatedStorage.WriteFileAsync(AccountDataFileName, JsonConvert.SerializeObject(SnapchatData));
		}

		#endregion

		public async void DeleteAsync()
		{
			await IsolatedStorage.DeleteFileAsync(AccountDataFileName, true);
			IsolatedStorage.DeleteRoamingSetting(RoamingSnapchatDataContainer, "Username");
			IsolatedStorage.DeleteRoamingSetting(RoamingSnapchatDataContainer, "AuthToken");
		}

		public async void LoadAsync()
		{
			// Deseralize the Account model from IsolatedStorage
			var accountData = await IsolatedStorage.ReadFileAsync(AccountDataFileName);
			if (accountData != null && !String.IsNullOrEmpty(accountData) && accountData != "null")
			{
				try
				{
					SnapchatData = JsonConvert.DeserializeObject<SnapchatData>(accountData);
				}
				catch (Exception exception)
				{
					Debug.WriteLine(exception.ToString());
				}
			}

			Loaded = true;
		}

		public async Task UpdateAllAsync(Action hidependingUiAction)
		{
			// Get all the updates fo stuff
			await Endpoints.GetAllUpdatesAsync();

			// Public Activity API only takes in 30 at a time, so we gotta get chunky
			foreach (var chunk in SnapchatData.UserAccount.Friends.Chunk(30))
				await Endpoints.GetPublicActivityAsync(chunk.Select(f => f.Name).ToArray());

			// Save data to IsolatedStorage
			await SaveAsync();

			hidependingUiAction();
		}

		public async Task DownloadSnapsAsync()
		{
			// Note: Auto-download settings is moved to the app

			//foreach (var conversation in AllUpdates.ConversationResponse)
			//	foreach (var snap in conversation.ConversationMessages.Messages.Where(m => m.Snap != null))
			//		await snap.DownloadSnapBlobAsync(this);

			// yea son
			await SaveSnapchatDataAsync();
		}

		#endregion
	}
}
