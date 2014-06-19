using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SnapDotNet.Core.Miscellaneous.CustomTypes;
using SnapDotNet.Core.Miscellaneous.Extensions;
using SnapDotNet.Core.Miscellaneous.Helpers.Storage;
using SnapDotNet.Core.Miscellaneous.Models;
using SnapDotNet.Core.Snapchat.Models.New;
using SnapDotNet.Core.Snapchat.Models.New.Responses;

namespace SnapDotNet.Core.Snapchat.Api
{
	public sealed class SnapchatManager
		: NotifyPropertyChangedBase
	{
		private const string AccountDataFileName = "update.json";
		private const string RoamingSnapchatDataContainer = "SnapchatData";

		public AllUpdatesResponse AllUpdates
		{
			get { return _allUpdates; }
			set { SetField(ref _allUpdates, value); }
		}
		private AllUpdatesResponse _allUpdates;

		public String AuthToken
		{
			get
			{
				if (AllUpdates != null && AllUpdates.UpdatesResponse != null)
					return AllUpdates.UpdatesResponse.AuthToken;
				
				return null;
			}
		}

		public String Username
		{
			get
			{
				if (AllUpdates != null && AllUpdates.UpdatesResponse != null)
					return AllUpdates.UpdatesResponse.Username;

				return null;
			}
		}

		public StoriesResponse Stories
		{
			get { return AllUpdates.StoriesResponse; }
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
				AllUpdates = null;
				await DeleteAsync();
				await Endpoints.LogoutAsync();
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}

		public void UpdateAuthToken(string authToken)
		{
			if (AllUpdates == null)
				AllUpdates = new AllUpdatesResponse
				{
					ConversationResponse = new ObservableCollection<ConversationResponse>(),
					MessagingGatewayInfo = new MessagingGatewayInfo(),
					StoriesResponse = new StoriesResponse(),
					UpdatesResponse = new UpdatesResponse()
				};

			AllUpdates.UpdatesResponse.AuthToken = authToken;
		}

		public void Update(AllUpdatesResponse allUpdates)
		{
			// lets get messy ya'll
			if (AllUpdates == null)
				AllUpdates = new AllUpdatesResponse();

			#region Update Updates

			if (AllUpdates.UpdatesResponse == null)
				AllUpdates.UpdatesResponse = new UpdatesResponse();
			var updates = AllUpdates.UpdatesResponse;
			var newUpdates = allUpdates.UpdatesResponse;

			updates.AccountPrivacy = newUpdates.AccountPrivacy;
			updates.AddedFriendsTimestamp = newUpdates.AddedFriendsTimestamp;
			updates.AuthToken = newUpdates.AuthToken;
			updates.CanViewMatureContent = newUpdates.CanViewMatureContent;
			updates.CountryCode = newUpdates.CountryCode;
			updates.CurrentTimestamp = newUpdates.CurrentTimestamp;
			updates.DeviceToken = newUpdates.DeviceToken;
			updates.Email = newUpdates.Email;
			updates.FeatureSettings = newUpdates.FeatureSettings;
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

			#region Update Background Fetch Secret Key

			AllUpdates.BackgroundFetchSecretKey = allUpdates.BackgroundFetchSecretKey;

			#endregion

			#region Update Messaging Gateway Infomation

			if (AllUpdates.MessagingGatewayInfo == null)
				AllUpdates.MessagingGatewayInfo = new MessagingGatewayInfo();

			var messagingGatewayInfo = AllUpdates.MessagingGatewayInfo;

			messagingGatewayInfo.GatewayAuthenticationToken = allUpdates.MessagingGatewayInfo.GatewayAuthenticationToken;
			messagingGatewayInfo.GatewayServer = allUpdates.MessagingGatewayInfo.GatewayServer;

			#endregion

			// TODO: finish this
			AllUpdates.ConversationResponse = allUpdates.ConversationResponse;
			AllUpdates.StoriesResponse = allUpdates.StoriesResponse;

			//AllUpdates = allUpdates;

			//// we got some house keeping to do
			//foreach (var newSnap in account.Snaps)
			//{
			//	var found = false;
			//	foreach (var oldSnap in Account.Snaps.Where(oldSnap => newSnap.Id == oldSnap.Id))
			//	{
			//		found = true;
			//		if (newSnap.RecipientName == Account.Username || newSnap.RecipientName == null)
			//			break;

			//		// replace, instead
			//		var position = Account.Snaps.IndexOf(oldSnap);
			//		if (position == -1) break;
			//		Account.Snaps.RemoveAt(position);
			//		Account.Snaps.Insert(position, newSnap);
			//		break;
			//	}

			//	if (!found)
			//		Account.Snaps.Insert(0, newSnap);
			//}

			//Account.Snaps = new ObservableCollection<Snap>(Account.Snaps.OrderByDescending(s => s.SentTimestamp).Take(50));
			//account.Snaps = Account.Snaps;
			//Account = account;
		}

		public void UpdateUsername(string username)
		{
			if (AllUpdates == null)
				AllUpdates = new AllUpdatesResponse
				{
					ConversationResponse = new ObservableCollection<ConversationResponse>(),
					MessagingGatewayInfo = new MessagingGatewayInfo(),
					StoriesResponse = new StoriesResponse(),
					UpdatesResponse = new UpdatesResponse()
				};

			AllUpdates.UpdatesResponse.Username = username;
		}

		public void UpdatePublicActivities(ObservableDictionary<string, PublicActivity> publicActivities)
		{
			foreach (var friend in AllUpdates.UpdatesResponse.Friends)
			{
				if (friend.PublicActivity == null)
					friend.PublicActivity = new PublicActivity();

				if (!publicActivities.ContainsKey(friend.Name)) continue;

				var publicActivity = publicActivities[friend.Name];
				friend.PublicActivity.Score = publicActivity.Score;
				friend.PublicActivity.BestFriends.Clear();
				foreach(var bestFriend in publicActivity.BestFriends.Take(3))
					friend.PublicActivity.BestFriends.Add(bestFriend);
			}
		}

		#endregion

		#region Cool Functions

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public bool IsAuthenticated()
		{
			return (AllUpdates != null && AllUpdates.UpdatesResponse != null && AllUpdates.UpdatesResponse.Logged);
		}

		#endregion
		
		#region Actions

		#region Actions:Save

		public async void Save()
		{
			await SaveAccountDataAsync();
			
			// All done b
		}

		public async Task SaveAccountDataAsync()
		{
			// Seralize the Account model and save as json string in Isolated Storage
			await IsolatedStorage.WriteFileAsync(AccountDataFileName, await Task.Factory.StartNew(() => JsonConvert.SerializeObject(AllUpdates)));
		}

		#endregion

		public async Task DeleteAsync()
		{
			await IsolatedStorage.DeleteFileAsync(AccountDataFileName);
			IsolatedStorage.DeleteRoamingSetting(RoamingSnapchatDataContainer, "Username");
			IsolatedStorage.DeleteRoamingSetting(RoamingSnapchatDataContainer, "AuthToken");
		}

		public async Task LoadAsync()
		{
			// Deseralize the Account model from IsolatedStorage
			var accountData = await IsolatedStorage.ReadFileAsync(AccountDataFileName);
			if (accountData != null && !String.IsNullOrEmpty(accountData) && accountData != "null")
			{
				try
				{
					var accountDataParsed = JsonConvert.DeserializeObject<AllUpdatesResponse>(accountData);
					Update(accountDataParsed);
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
			foreach (var chunk in AllUpdates.UpdatesResponse.Friends.Chunk(30))
				await Endpoints.GetPublicActivityAsync(chunk.Select(f => f.Name).ToArray());

			// Save data to IsolatedStorage
			Save();

			hidependingUiAction();
		}

		public async Task DownloadSnapsAsync()
		{
			// Note: Auto-download settings is moved to the app

			//foreach (var conversation in AllUpdates.ConversationResponse)
			//	foreach (var snap in conversation.ConversationMessages.Messages.Where(m => m.Snap != null))
			//		await snap.DownloadSnapBlobAsync(this);

			// yea son
			await SaveAccountDataAsync();
		}

		#endregion
	}
}
