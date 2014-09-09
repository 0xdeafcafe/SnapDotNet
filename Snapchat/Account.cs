using Newtonsoft.Json;
using SnapDotNet.Extentions;
using SnapDotNet.Responses;
using SnapDotNet.Utilities;
using SnapDotNet.Utilities.CustomTypes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;
using Windows.Web.Http;

namespace SnapDotNet
{
	/// <summary>
	/// Indicates the media type.
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

	public sealed class Account
		: ObservableObject
	{
		/// <summary>
		/// Authenticates a user using the given <paramref name="username"/> and <paramref name="password"/>,
		/// and retrieves an <see cref="Account"/> object containing the account data.
		/// </summary>
		/// <param name="username">The username.</param>
		/// <param name="password">The password.</param>
		/// <returns>
		/// If authenticated, an <see cref="Account"/> object containing the account data.
		/// </returns>
		/// <exception cref="InvalidCredentialsException">
		/// Given set of credentials is incorrect.
		/// </exception>
		[Pure]
		public static async Task<Account> AuthenticateAsync(string username, string password)
		{
			Contract.Requires<ArgumentNullException>(username != null && password != null);
			Debug.WriteLine("[Snapchat API] Authenticating account [username: {0} | password: {1}]", username, password);

			var data = new Dictionary<string, string>
			{
				{ "username", username },
				{ "password", password }
			};

			try
			{
				var response = await EndpointManager.Managers["bq"].PostAsync<AccountResponse>("login", data);
				if (response == null || !response.IsLogged)
					throw new InvalidCredentialsException();

				// Only get the relevant data and do some processing to make properties like
				// friends more accessible to the view/view models.
				var account = new Account
				{
					AuthToken = response.AuthToken,
					Username = response.Username,
					Email = response.Email,
					PhoneNumber = response.PhoneNumber,
					LastReplayed = response.LastReplayedSnapTimestamp,
					PhoneNumberVerificationKey = response.PhoneNumberVerificationKey,
					VerificationPhoneNumber = response.SnapchatPhoneNumber,
					ShouldTextToVerifyPhoneNumber = response.ShouldSendTextToVerifyNumber,
					ShouldCallToVerifyPhoneNumber = response.ShouldCallToVerifyNumber,
					SearchableByPhoneNumber = response.SearchableByPhoneNumber,
					Score = response.Score,
					SnapsReceived = response.SnapsReceived,
					SnapsSent = response.SnapsSent,
					Birthday = response.Birthday
				};
				//account.CreateSortedFriends();
				return account;
			}
			catch (InvalidHttpResponseException ex)
			{
				if (ex.HttpResponseMessage.StatusCode == HttpStatusCode.Unauthorized)
					throw new InvalidCredentialsException();

				throw;
			}
		}

		/// <summary>
		/// Authenticates a user using the given <paramref name="username"/> and <paramref name="authToken"/>,
		/// and creates a dummy <see cref="Account"/> object to be used in conjunction with <see cref="UpdateAccountAsync"/>.
		/// </summary>
		/// <param name="username">The username.</param>
		/// <param name="authToken">The snapchat authentication token.</param>
		/// <returns>
		/// If authenticated, an <see cref="Account"/> object containing the account data.
		/// </returns>
		[Pure]
		public static Account AuthenticateFromAuth(string username, string authToken)
		{
			return new Account
			{
				AuthToken = authToken, 
				Username = username
			};
		}

		/// <summary>
		/// 
		/// </summary>
		private Account()
		{
			_friends.CollectionChanged +=
				(sender, args) => OnObservableCollectionChanged(args, "Friends", () => OnPropertyChanged("SortedFriends"));
		}

		#region Properties

		/// <summary>
		/// Gets the username associated with this account.
		/// </summary>
		[JsonProperty]
		public string Username
		{
			get { return _username; }
			private set { SetValue(ref _username, value); }
		}
		private string _username;

		/// <summary>
		/// Gets the email associated with this account.
		/// </summary>
		[JsonProperty]
		public string Email
		{
			get { return _email; }
			private set { SetValue(ref _email, value); }
		}
		private string _email;

		/// <summary>
		/// Gets this user's birthday.
		/// </summary>
		[JsonProperty]
		public DateTime Birthday
		{
			get { return _birthday; }
			private set { SetValue(ref _birthday, value); }
		}
		private DateTime _birthday;

		/// <summary>
		/// Gets if the user can legally view mature content.
		/// </summary>
		[JsonIgnore]
		public bool CanLegallySeeMatureContent
		{
			get
			{
				var now = DateTime.Now;
				var age = now.Year - Birthday.Year;
				if (now < Birthday.AddYears(age)) age--;

				return (age >= 18);
			}
		}

		/// <summary>
		/// Gets the users friends.
		/// </summary>
		[JsonProperty]
		public ObservableCollection<Friend> Friends
		{
			get { return _friends; }
			private set
			{
				SetValue(ref _friends, value);
				OnPropertyChanged("SortedFriends");
			}
		}
		private ObservableCollection<Friend> _friends = new ObservableCollection<Friend>();

		/// <summary>
		/// 
		/// </summary>
		[JsonIgnore]
		public Friend Me
		{
			get { return Friends.FirstOrDefault(f => f.Name == Username); }
		}

		/// <summary>
		/// 
		/// </summary>
		[JsonProperty]
		public ObservableCollection<PersonalStory> PersonalStories
		{
			get { return _personalStories; }
			set { SetValue(ref _personalStories, value); }
		} 
		private ObservableCollection<PersonalStory> _personalStories = new ObservableCollection<PersonalStory>();

		/// <summary>
		/// 
		/// </summary>
		[JsonIgnore]
		public ObservableCollection<FriendsKeyGroup> SortedFriends
		{
			get { return _sortedFriends; }
			set { SetValue(ref _sortedFriends, value); }
		}
		private ObservableCollection<FriendsKeyGroup> _sortedFriends;

		/// <summary>
		/// Gets the phone number associated with this account.
		/// </summary>
		/// <seealso cref="PhoneNumberVerificationKey"/>
		/// <seealso cref="ShouldCallToVerifyPhoneNumber"/>
		/// <seealso cref="ShouldTextToVerifyPhoneNumber"/>
		/// <seealso cref="VerificationPhoneNumber"/>
		[JsonProperty]
		public string PhoneNumber
		{
			get { return _phone; }
			private set { SetValue(ref _phone, value); }
		}
		private string _phone;

		/// <summary>
		/// Gets the key that should be sent to <see cref="VerificationPhoneNumber"/> in order to
		/// verify this user's phone number.
		/// </summary>
		/// <seealso cref="PhoneNumber"/>
		/// <seealso cref="ShouldCallToVerifyPhoneNumber"/>
		/// <seealso cref="ShouldTextToVerifyPhoneNumber"/>
		/// <seealso cref="VerificationPhoneNumber"/>
		[JsonProperty]
		public string PhoneNumberVerificationKey
		{
			get { return _phoneNumberVerificationKey; }
			private set { SetValue(ref _phoneNumberVerificationKey, value); }
		}
		private string _phoneNumberVerificationKey;

		/// <summary>
		/// Gets the phone number where the <see cref="PhoneNumberVerificationKey"/> should be sent
		/// in order to verify this user's phone number.
		/// </summary>
		/// <seealso cref="PhoneNumber"/>
		/// <seealso cref="ShouldCallToVerifyPhoneNumber"/>
		/// <seealso cref="ShouldTextToVerifyPhoneNumber"/>
		/// <seealso cref="PhoneNumberVerificationKey"/>
		[JsonProperty]
		public string VerificationPhoneNumber
		{
			get { return _verificationPhoneNumber; }
			private set { SetValue(ref _verificationPhoneNumber, value); }
		}
		private string _verificationPhoneNumber;

		/// <summary>
		/// Gets a boolean value indicating whether this user's phone number should be verified by
		/// calling <see cref="VerificationPhoneNumber"/>.
		/// </summary>
		/// <seealso cref="PhoneNumber"/>
		/// <seealso cref="VerificationPhoneNumber"/>
		/// <seealso cref="ShouldTextToVerifyPhoneNumber"/>
		/// <seealso cref="PhoneNumberVerificationKey"/>
		[JsonProperty]
		public bool ShouldCallToVerifyPhoneNumber
		{
			get { return _shouldCallToVerifyPhoneNumber; }
			private set { SetValue(ref _shouldCallToVerifyPhoneNumber, value); }
		}
		private bool _shouldCallToVerifyPhoneNumber;

		/// <summary>
		/// Gets a boolean value indicating whether this user's phone number should be verified by
		/// sending a SMS message to <see cref="VerificationPhoneNumber"/>.
		/// </summary>
		/// <seealso cref="PhoneNumber"/>
		/// <seealso cref="VerificationPhoneNumber"/>
		/// <seealso cref="ShouldCallToVerifyPhoneNumber"/>
		/// <seealso cref="PhoneNumberVerificationKey"/>
		[JsonProperty]
		public bool ShouldTextToVerifyPhoneNumber
		{
			get { return _shouldTextToVerifyPhoneNumber; }
			private set { SetValue(ref _shouldTextToVerifyPhoneNumber, value); }
		}
		private bool _shouldTextToVerifyPhoneNumber;

		/// <summary>
		/// Gets or sets a boolean value indicating whether this user can be found by searching for
		/// this user's phone number.
		/// </summary>
		[JsonProperty]
		public bool SearchableByPhoneNumber
		{
			get { return _searchableByPhoneNumber; }
			private set { SetValue(ref _searchableByPhoneNumber, value); }
		}
		private bool _searchableByPhoneNumber;

		/// <summary>
		/// Gets the timestamp of the last replay.
		/// </summary>
		[JsonProperty]
		public DateTime LastReplayed
		{
			get { return _lastReplayed; }
			private set { SetValue(ref _lastReplayed, value); }
		}
		private DateTime _lastReplayed;

		/// <summary>
		/// Gets a boolean value indicating whether this user can replay a Snap.
		/// </summary>
		[JsonIgnore]
		public bool CanReplaySnap
		{
			get { return (DateTime.UtcNow - LastReplayed).TotalHours > 24; }
		}

		/// <summary>
		/// Gets the number of Snaps this user has received.
		/// </summary>
		[JsonProperty]
		public int SnapsReceived
		{
			get { return _snapsReceived; }
			private set
			{
				SetValue(ref _snapsReceived, value);
				OnPropertyChanged("SnapBandwidthSplit");
			}
		}
		private int _snapsReceived;

		/// <summary>
		/// Gets the number of Snaps send and received, in the format; {Sent} | {Recieved}
		/// </summary>
		[JsonIgnore]
		public string SnapBandwidthSplit
		{
			get { return String.Format("{0} | {1}", SnapsSent, SnapsReceived); }
		}

		/// <summary>
		/// Gets the number of Snaps this user has sent.
		/// </summary>
		[JsonProperty]
		public int SnapsSent
		{
			get { return _snapsSent; }
			private set
			{
				SetValue(ref _snapsSent, value);
				OnPropertyChanged("SnapBandwidthSplit");
			}
		}
		private int _snapsSent;

		/// <summary>
		/// Gets this user's score.
		/// </summary>
		[JsonProperty]
		public int Score
		{
			get { return _score; }
			internal set { SetValue(ref _score, value); }
		}
		private int _score;

		/// <summary>
		/// Gets a boolean value indicating whether this user is blocked from participating in the
		/// Our Story feature.
		/// </summary>
		[JsonProperty]
		public bool BlockedFromOurStory
		{
			get { return _blockedFromOurStory; }
			internal set { SetValue(ref _blockedFromOurStory, value); }
		}
		private bool _blockedFromOurStory;

		/// <summary>
		/// Gets or sets the auth token for the current session.
		/// </summary>
		[JsonProperty]
		public string AuthToken
		{
			get { return _authToken; }
			private set { SetValue(ref _authToken, value); }
		}
		private string _authToken;

		/*
			[DataMember(Name = "added_friends")]
			public ObservableCollection<AddedFriend> AddedFriends { get; set; }

			[DataMember(Name = "notification_sound_setting")]
			public string NotificationSoundSetting { get; set; }

			[DataMember(Name = "recents")]
			public ObservableCollection<string> RecentFriends { get; set; }

			[DataMember(Name = "requests")]
			public ObservableCollection<string> FriendRequests { get; set; }

			[DataMember(Name = "snap_p")]
			public string AccountPrivacySetting { get; set; }

			[DataMember(Name = "story_privacy")]
			public string StoryPrivacySetting { get; set; }
		*/

		#endregion

		#region Sorted Friends

		/// <summary>
		/// 
		/// </summary>
		public Task CreateSortedFriendsAsync()
		{
			return Task.Run(() => { SortedFriends = FriendsKeyGroup.CreateGroups(Friends, Username); });
		}

		public void UpdateSortedFriends()
		{
			// LINQ game ridiculous
			var list = (from @group in SortedFriends
				from friend in @group
				let res1 = @group.Key == "!" && friend.FriendRequestState != FriendRequestState.Blocked
				let res2 = @group.Key != "!" && friend.FriendRequestState == FriendRequestState.Blocked
				let res3 = @group.Key == "#" && !Char.IsNumber(friend.FriendlyName[0])
				let res4 =
					@group.Key != friend.FriendlyName[0].ToString().ToLowerInvariant() &&
					friend.FriendRequestState != FriendRequestState.Blocked
				where res1 || res2 || res3 || res4
				select friend).ToList();
			var modifiedFriends = new ObservableCollection<Friend>(list);

			FriendsKeyGroup.RemoveEntries(SortedFriends, modifiedFriends);
			FriendsKeyGroup.InsertEntries(SortedFriends, modifiedFriends, Username);
		}

		#endregion

		#region Update

		/// <summary>
		/// Gets all updates for the authenticated user, given <see cref="Username"/> and <see cref="AuthToken"/>
		/// </summary>
		/// <exception cref="InvalidCredentialsException">
		/// Given set of credentials is incorrect.
		/// </exception>
		public async Task UpdateAccountAsync()
		{
			Contract.Requires<NullReferenceException>(Username != null && AuthToken != null);
			Debug.WriteLine("[Snapchat API] Updating account [username: {0} | auth-token: {1}]", Username, AuthToken);

			var data = new Dictionary<string, string>
			{
				{ "username", Username }
			};

			try
			{
				var response = await EndpointManager.Managers["loq"].PostAsync<AllUpdatesResponse>("all_updates", data, AuthToken);
				if (response == null)
					throw new InvalidCredentialsException();

				// Deal with Account data
				UpdateAccount(response.AccountResponse);

				// Deal with Stories data
				await UpdateStories(response.StoriesResponse);

				// TODO: Parse the rest of the data

				// Update Public Activities
				await UpdateFriendsPublicActivityAsync();
			}
			catch (InvalidHttpResponseException ex)
			{
				if (ex.HttpResponseMessage.StatusCode == HttpStatusCode.Unauthorized)
					throw new InvalidCredentialsException();

				throw;
			}
		}

		/// <summary>
		/// Update the data in this model from <paramref name="accountResponse"/>
		/// </summary>
		/// <param name="accountResponse">The Account Response to update from</param>
		private async void UpdateAccount(AccountResponse accountResponse)
		{
			Contract.Requires<ArgumentNullException>(accountResponse != null);

			AuthToken = accountResponse.AuthToken;
			Username = accountResponse.Username;
			Email = accountResponse.Email;
			PhoneNumber = accountResponse.PhoneNumber;
			LastReplayed = accountResponse.LastReplayedSnapTimestamp;
			PhoneNumberVerificationKey = accountResponse.PhoneNumberVerificationKey;
			VerificationPhoneNumber = accountResponse.SnapchatPhoneNumber;
			ShouldTextToVerifyPhoneNumber = accountResponse.ShouldSendTextToVerifyNumber;
			ShouldCallToVerifyPhoneNumber = accountResponse.ShouldCallToVerifyNumber;
			SearchableByPhoneNumber = accountResponse.SearchableByPhoneNumber;
			Score = accountResponse.Score;
			SnapsReceived = accountResponse.SnapsReceived;
			SnapsSent = accountResponse.SnapsSent;
			Birthday = accountResponse.Birthday;

			// TODO: Save the fields that are missing to this model

			if (SortedFriends == null)
				await CreateSortedFriendsAsync();

			// Remove removed friends.
			var removedFriends = Friends.Where(f => accountResponse.Friends.FirstOrDefault(f1 => f1.Name == f.Name) == null);
			var removeFriendsModel = new List<Friend>();
			foreach (var removedFriend in removedFriends)
			{
				Friends.Remove(removedFriend);
				removeFriendsModel.Add(removedFriend);
			}
			FriendsKeyGroup.RemoveEntries(SortedFriends, removeFriendsModel);

			// Add new friends.
			var newFriends = accountResponse.Friends.Where(f1 => Friends.FirstOrDefault(f => f.Name == f1.Name) == null);
			var newFriendsModel = new List<Friend>();
			foreach (var friend in newFriends.Select(Friend.Create))
			{
				Friends.Add(friend);
				newFriendsModel.Add(friend);
			}
			FriendsKeyGroup.InsertEntries(SortedFriends, newFriendsModel, Username);

			// Update existing friends
			var existingFriends = accountResponse.Friends.Where(f1 => Friends.FirstOrDefault(f => f.Name == f1.Name) != null);
			foreach (var existingFriend in existingFriends)
			{
				var friend = Friends.FirstOrDefault(f => existingFriend.Name == f.Name);
				if (friend == null) continue;

				var updateSortedFriends = (friend.FriendRequestState != (FriendRequestState) existingFriend.FriendRequestState ||
				                           friend.DisplayName != existingFriend.DisplayName);
				friend.Update(existingFriend);
				if (!updateSortedFriends) continue;

				// Update Sorted Friends
				FriendsKeyGroup.RemoveEntries(SortedFriends, new List<Friend> { friend });
				FriendsKeyGroup.InsertEntries(SortedFriends, new List<Friend> { friend }, Username);
			}
		}

		/// <summary>
		/// Update the data in this model from <paramref name="storiesResponse"/>
		/// </summary>
		/// <param name="storiesResponse">The Stories Reponse to update from</param>
		private async Task UpdateStories(StoriesResponse storiesResponse)
		{
			Contract.Requires<ArgumentNullException>(storiesResponse != null);

			// Check for new personal stories
			foreach (var personalStory in storiesResponse.MyStories.OrderByDescending(s => s.Story.TimeLeft).Where(friendStory => PersonalStories.FirstOrDefault(s => s.Story.Id == friendStory.Story.Id) == null))
				PersonalStories.Add(PersonalStory.Create(personalStory));

			// Check for active personal stories, and update them
			foreach (var personalStory in PersonalStories)
			{
				var personalStoryMetadata = storiesResponse.MyStories.FirstOrDefault(s => s.Story.Id == personalStory.Story.Id);
				if (personalStoryMetadata == null) continue;
				personalStory.Update(personalStoryMetadata);
			}

			// Check for expired personal stories, and delete them
			foreach (var redundantStory in PersonalStories.Where(s => storiesResponse.MyStories.FirstOrDefault(s1 => s1.Story.Id == s.Story.Id) == null).ToList())
				PersonalStories.Remove(redundantStory);

			// Update Friends Stories xox
			foreach (var friendStory in storiesResponse.FriendStories)
			{
				var friend = Friends.FirstOrDefault(f => f.Name == friendStory.Username && f.Name != Username);
				if (friend == null) continue;
				friend.UpdateStories(friendStory);
			}

			// Download Snaps, depending on user settings
			const bool alwaysDownload = true; // hehe
			foreach (var friend in Friends)
			{
				foreach (var story in friend.Stories)
				{
					if (!alwaysDownload) continue;
					await story.GetThumbnailAsync();
					await story.GetMediaAsync();
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public async Task UpdateFriendsPublicActivityAsync()
		{
			var publicActivities = new Dictionary<string, PublicActivityResponse>();
			foreach (var chunk in Friends.Chunk(30))
			{
				var data = new Dictionary<string, string>
				{
					{"username", Username},
					{"friend_usernames", JsonConvert.SerializeObject(chunk.Select(u => u.Name))}
				};

				var response = await EndpointManager.Managers["bq"].PostAsync<Dictionary<string, PublicActivityResponse>>("bests", data, AuthToken);
				if (response == null)
					throw new InvalidCredentialsException();
				foreach (var publicActivity in response)
					publicActivities.Add(publicActivity.Key, publicActivity.Value);
			}

			foreach (var publicActivity in publicActivities)
			{
				var selectedFriend = Friends.FirstOrDefault(f => f.Name == publicActivity.Key);
				selectedFriend.UpdatePublicActivies(publicActivity.Value, Friends.ToList());
			}
		}

		#endregion
	}
}
