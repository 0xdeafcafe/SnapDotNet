using Windows.System.Threading;
using Newtonsoft.Json;
using SnapDotNet.Data.ApiResponses;
using SnapDotNet.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;
using Windows.Web.Http;

namespace SnapDotNet.Data
{
	/// <summary>
	/// Represents a Snapchat account.
	/// 
	/// This class cannot be inherited.
	/// </summary>
	public sealed class Account
		: ObservableObject
	{
		private Account()
		{
			_friends = new ObservableCollection<Friend>();
			_readOnlyFriends = new ReadOnlyObservableCollection<Friend>(_friends);
			_recentFriends = new ObservableCollection<Friend>();
			_readOnlyRecentFriends = new ReadOnlyObservableCollection<Friend>(_recentFriends);
		}

		#region Properties

		/// <summary>
		/// Gets the auth token for the current session.
		/// </summary>
		public string AuthToken
		{
			get { return _authToken; }
			private set { SetValue(ref _authToken, value); }
		}
		private string _authToken;

		/// <summary>
		/// Gets this user's birthday.
		/// 
		/// Use the <see cref="SetBirthdayAsync"/> method to modify this property.
		/// </summary>
		/// <seealso cref="SetBirthdayAsync"/>
		public DateTime Birthday
		{
			get { return _birthday; }
			private set { SetValue(ref _birthday, value); }
		}
		private DateTime _birthday;

		/// <summary>
		/// Gets a boolean value indicating whether this user can view mature content.
		/// 
		/// Use the <see cref="SetCanViewMatureContentAsync"/> method to modify this property.
		/// </summary>
		/// <seealso cref="SetCanViewMatureContentAsync"/>
		public bool CanViewMatureContent
		{
			get { return _canViewMatureContent; }
			private set { SetValue(ref _canViewMatureContent, value); }
		}
		private bool _canViewMatureContent;

		/// <summary>
		/// Gets the email associated with this account.
		/// 
		/// Use the <see cref="SetEmailAsync"/> method to modify this property.
		/// </summary>
		/// <seealso cref="SetEmailAsync"/>
		public string Email
		{
			get { return _email; }
			private set { SetValue(ref _email, value); } 
		}
		private string _email;

		/// <summary>
		/// Gets the username associated with this account.
		/// </summary>
		public string Username
		{
			get { return _username; }
			private set { SetValue(ref _username, value); }
		}
		private string _username;

		/// <summary>
		/// Gets this user's friends.
		/// </summary>
		public ReadOnlyObservableCollection<Friend> Friends
		{
			get { return _readOnlyFriends; }
			private set { SetValue(ref _readOnlyFriends, value); }
		}
		internal readonly ObservableCollection<Friend> _friends;
		private ReadOnlyObservableCollection<Friend> _readOnlyFriends;

		/// <summary>
		/// Gets a <see cref="Friend"/> object representing this user.
		/// </summary>
		public Friend Me
		{
			get { return Friends.FirstOrDefault(f => f.Username == Username); }
		}

		/// <summary>
		/// Gets this user's stories.
		/// </summary>
		public ObservableCollection<PersonalStory> PersonalStories
		{
			get { return _personalStories; }
			private set { SetValue(ref _personalStories, value); }
		}
		private ObservableCollection<PersonalStory> _personalStories = new ObservableCollection<PersonalStory>();

		/// <summary>
		/// Gets the phone number associated with this account.
		/// 
		/// Use the <see cref="SetPhoneNumberAsync"/> method to modify this property.
		/// </summary>
		/// <seealso cref="PhoneNumberVerificationKey"/>
		/// <seealso cref="ShouldCallToVerifyPhoneNumber"/>
		/// <seealso cref="ShouldTextToVerifyPhoneNumber"/>
		/// <seealso cref="VerificationPhoneNumber"/>
		/// <seealso cref="SetPhoneNumberAsync"/>
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
		public bool ShouldTextToVerifyPhoneNumber
		{
			get { return _shouldTextToVerifyPhoneNumber; }
			private set { SetValue(ref _shouldTextToVerifyPhoneNumber, value); }
		}
		private bool _shouldTextToVerifyPhoneNumber;

		/// <summary>
		/// Gets or sets a boolean value indicating whether this user can be found by searching for
		/// this user's phone number.
		/// 
		/// Use the <see cref="SetPhoneNumberPrivacyAsync"/> method to modify this property.
		/// </summary>
		/// <seealso cref="SetPhoneNumberPrivacyAsync"/>
		public bool IsSearchableByPhoneNumber
		{
			get { return _searchableByPhoneNumber; }
			private set { SetValue(ref _searchableByPhoneNumber, value); }
		}
		private bool _searchableByPhoneNumber;

		/// <summary>
		/// Gets the timestamp of the last replay.
		/// </summary>
		public DateTime LastReplayed
		{
			get { return _lastReplayed; }
			private set { SetValue(ref _lastReplayed, value); }
		}
		private DateTime _lastReplayed;

		/// <summary>
		/// Gets a boolean value indicating whether this user can replay a Snap.
		/// </summary>
		public bool CanReplaySnap
		{
			get { return (DateTime.UtcNow - LastReplayed).TotalHours > ApiSettings.ReplayCooldownDuration; }
		}

		/// <summary>
		/// Gets the number of Snaps this user has received.
		/// </summary>
		public int SnapsReceived
		{
			get { return _snapsReceived; }
			private set { SetValue(ref _snapsReceived, value); }
		}
		private int _snapsReceived;

		/// <summary>
		/// Gets the number of Snaps this user has sent.
		/// </summary>
		public int SnapsSent
		{
			get { return _snapsSent; }
			private set { SetValue(ref _snapsSent, value); }
		}
		private int _snapsSent;

		/// <summary>
		/// Gets this user's score.
		/// </summary>
		public int Score
		{
			get { return _score; }
			private set { SetValue(ref _score, value); }
		}
		private int _score;

		/// <summary>
		/// Gets a boolean value indicating whether this user is blocked from participating in the
		/// Our Story feature.
		/// </summary>
		public bool IsBlockedFromOurStory
		{
			get { return _isBlockedFromOurStory; }
			private set { SetValue(ref _isBlockedFromOurStory, value); }
		}
		private bool _isBlockedFromOurStory;

		/// <summary>
		/// Gets a boolean value indicating whether notifications should play a sound.
		/// </summary>
		/// <seealso cref="SetShouldPlayNotificationSoundAsync"/>
		public bool ShouldPlayNotificationSound
		{
			get { return _shouldPlayNotificationSound; }
			private set { SetValue(ref _shouldPlayNotificationSound, value); }
		}
		private bool _shouldPlayNotificationSound;

		/// <summary>
		/// Gets a collection of friends who this user recently interacted with.
		/// </summary>
		public ReadOnlyObservableCollection<Friend> RecentFriends
		{
			get { return _readOnlyRecentFriends; }
			private set { SetValue(ref _readOnlyRecentFriends, value); }
		}
		private ReadOnlyObservableCollection<Friend> _readOnlyRecentFriends;
		private readonly ObservableCollection<Friend> _recentFriends;

		/// <summary>
		/// Gets the number of best friends of this user.
		/// </summary>
		/// <seealso cref="SetBestFriendsCountAsync"/>
		public int NumberOfBestFriends
		{
			get { return _bestFriendCount; }
			private set { SetValue(ref _bestFriendCount, value); }
		}
		private int _bestFriendCount;

		/* TODO:
			[DataMember(Name = "added_friends")]
			public ObservableCollection<AddedFriend> AddedFriends { get; set; }

			[DataMember(Name = "notification_sound_setting")]
			public string NotificationSoundSetting { get; set; }

			[DataMember(Name = "requests")]
			public ObservableCollection<string> FriendRequests { get; set; }

			[DataMember(Name = "snap_p")]
			public string AccountPrivacySetting { get; set; }

			[DataMember(Name = "story_privacy")]
			public string StoryPrivacySetting { get; set; }
		*/

		#endregion

		#region Authentication

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
			Debug.WriteLine("[Account] Authenticating account [username: {0} | password: {1}]", username, password);

			var data = new Dictionary<string, string>
			{
				{ "username", username },
				{ "password", password }
			};

			var response = await EndpointManager.Managers["bq"].PostAsync<AccountResponse>("login", data);
			if (response.IsLogged)
			{
				var account = new Account();
				account.UpdateFromResponse(response);
				return account;
			}

			throw new InvalidCredentialsException(response.Message);
		}

		/// <summary>
		/// Authenticates a user using the given <paramref name="username"/> and <paramref name="authToken"/>,
		/// and retrieves an <see cref="Account"/> object containing the account data.
		/// </summary>
		/// <param name="username">The username.</param>
		/// <param name="authToken">The auth token.</param>
		/// <returns>
		/// If authenticated, an <see cref="Account"/> object containing the account data.
		/// </returns>
		/// <exception cref="InvalidCredentialsException">
		/// Given set of credentials is incorrect.
		/// </exception>
		[Pure]
		public static async Task<Account> AuthenticateFromTokenAsync(string username, string authToken)
		{
			Contract.Requires<ArgumentNullException>(username != null && authToken != null);

			var account = new Account
			{
				Username = username,
				AuthToken = authToken
			};
			await account.UpdateAccountAsync();
			return account;
		}

		/// <summary>
		/// Invalidates the current session.
		/// </summary>
		/// <returns><c>true</c> if logged out successfully; otherwise, <c>false</c>.</returns>
		public async Task<bool> LogoutAsync()
		{
			var data = new Dictionary<string, string>
			{
				{ "username", Username },
				{ "json", "{}" },
				{ "events", "[]" }
			};

			try
			{
				await EndpointManager.Managers["bq"].PostAsync<Response>("logout", data, AuthToken);
				Debug.WriteLine("[Account] Logged out (auth token is now invalid)");
				return true;
			}
			catch (InvalidHttpResponseException ex)
			{
				if (ex.HttpResponseMessage.StatusCode == HttpStatusCode.Unauthorized)
					throw new InvalidCredentialsException();

				throw;
			}
		}

		#endregion

		#region Settings
		
		/// <summary>
		/// Sets the number of best friends for this user.
		/// 
		/// Acceptable values are 3, 5 or 7.
		/// </summary>
		/// <param name="count">The number of friends.</param>
		/// <returns>A new collection of best friends.</returns>
		/// <seealso cref="NumberOfBestFriends"/>
		public async Task<IEnumerable<Friend>> SetBestFriendsCountAsync(int count)
		{
			Contract.Requires<ArgumentOutOfRangeException>(count == 3 || count == 5 || count == 7);
			Debug.WriteLine("[Account] Changing best friend count...", count);

			var data = new Dictionary<string, string>
			{
				{ "username", Username },
				{ "num_best_friends", count.ToString() }
			};

			var bestFriends = new List<Friend>();
			try
			{
				var response = await EndpointManager.Managers["bq"].PostAsync<BestFriendsResponse>("set_num_best_friends", data, AuthToken);
				Debug.WriteLine("[Account] Set number of best friends to {0}", count);

				foreach (var bestFriend in response.BestFriends)
				{
					var friend = Friends.FirstOrDefault(f => f.Username == bestFriend);
					if (friend != null) bestFriends.Add(friend);
				}

				NumberOfBestFriends = bestFriends.Count;
				return bestFriends;
			}
			catch (InvalidHttpResponseException ex)
			{
				if (ex.HttpResponseMessage.StatusCode == HttpStatusCode.Unauthorized)
					throw new InvalidCredentialsException();

				throw;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="email"></param>
		/// <returns><c>null</c> if succeeded; otherwise, the error message.</returns>
		/// <exception cref="InvalidCredentialsException">
		/// <see cref="AuthToken"/> is expired or invalid.
		/// </exception>
		public async Task<string> SetEmailAsync(string email)
		{
			Contract.Requires<ArgumentNullException>(email != null);
			var response = await UpdateSettingAsync("updateEmail", new Dictionary<string, string> {{"email", email}}, verify: email);
			if (response == null)
			{
				Debug.WriteLine("[Account] Success! Email = \"{0}\"", email);
				Email = email;
			}
			return response;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="playSound"></param>
		/// <returns></returns>
		/*public async Task<bool> SetShouldPlayNotificationSoundAsync(bool playSound)
		{
			string value = playSound ? "ON" : "OFF";
			if (await UpdateSettingAsync("updateNotificationSoundSetting", new Dictionary<string, string> { { "notification_sound_setting", value } }, verify: value))
			{
				Debug.WriteLine("[Account] Success! ShouldPlayNotificationSound = {0}", playSound);
				ShouldPlayNotificationSound = playSound;
				return true;
			}
			return false;
		}*/

		/// <summary>
		/// 
		/// </summary>
		/// <param name="canViewMatureContent"></param>
		/// <returns><c>null</c> if succeeded; otherwise, the error message.</returns>
		/// <exception cref="InvalidCredentialsException">
		/// <see cref="AuthToken"/> is expired or invalid.
		/// </exception>
		public async Task<string> SetCanViewMatureContentAsync(bool canViewMatureContent)
		{
			string canView = canViewMatureContent ? "true" : "false";
			var response = await UpdateSettingAsync("updateCanViewMatureContent", new Dictionary<string, string> {{"canViewMatureContent", canView}});
			if (response == null)
			{
				Debug.WriteLine("[Account] Success! CanViewMatureContent = {0}", canViewMatureContent);
				CanViewMatureContent = canViewMatureContent;
			}
			return response;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="isPhoneNumberSearchable"></param>
		/*public async Task<bool> SetPhoneNumberPrivacyAsync(bool isPhoneNumberSearchable)
		{
			string isSearchable = isPhoneNumberSearchable ? "true" : "false";
			if (await UpdateSettingAsync("updateSearchableByPhoneNumber", new Dictionary<string, string> { { "searchable_by_phone_number", isSearchable } }, verify: isSearchable))
			{
				Debug.WriteLine("[Account] Success! IsSearchableByPhoneNumber = {0}", isPhoneNumberSearchable);
				IsSearchableByPhoneNumber = isPhoneNumberSearchable;
				return true;
			}
			return false;
		}*/

		//updateStoryPrivacy - privacySetting, storyFriendsToBlock: "['teamsnapchat','another_username']" (if custom)
		//updatePrivacy - privacySetting

		/// <summary>
		/// 
		/// </summary>
		/// <param name="phoneNumber"></param>
		/*public async Task<bool> SetPhoneNumberAsync(string phoneNumber)
		{
			Contract.Requires<ArgumentNullException>(phoneNumber != null);
			if (await UpdateSettingAsync("updateMobile", new Dictionary<string, string> { { "mobile", phoneNumber } }))
			{
				Debug.WriteLine("[Account] Success! PhoneNumber = \"{0}\"", phoneNumber);
				PhoneNumber = phoneNumber;
				return true;
			}
			return false;
		}*/

		/// <summary>
		/// Change this user's birthday. The birth year cannot be modified.
		/// </summary>
		/// <param name="month">The month this user was born.</param>
		/// <param name="day">The day this user was born.</param>
		/// <returns><c>null</c> if succeeded; otherwise, the error message.</returns>
		/// <exception cref="InvalidCredentialsException">
		/// <see cref="AuthToken"/> is expired or invalid.
		/// </exception>
		public async Task<string> SetBirthdayAsync(int month, int day)
		{
			string birthday = string.Format("{0:D2}-{1:D2}", month, day);
			string verification = Birthday.Year + "-" + birthday;

			var response = await UpdateSettingAsync("updateBirthday", new Dictionary<string, string> {{"birthday", birthday}}, verify: verification);
			if (response == null)
			{
				Birthday = new DateTime(Birthday.Year, month, day);
				Debug.WriteLine("[Account] Success! Birthday = {0}", Birthday);
			}
			return response;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="actionName"></param>
		/// <param name="data"></param>
		/// <param name="verify"></param>
		/// <returns><c>null</c> if succeeded; otherwise, the error message.</returns>
		private async Task<string> UpdateSettingAsync(string actionName, Dictionary<string, string> data, string verify = null)
		{
			Contract.Requires<ArgumentNullException>(actionName != null);
			Debug.WriteLine("[Account] Updating setting [actionName: \"{0}\"]", actionName);

			data = data ?? new Dictionary<string, string>();
			data["username"] = Username;
			data["action"] = actionName;

			try
			{
				var response = await EndpointManager.Managers["bq"].PostAsync<Response>("settings", data, AuthToken);
				if (response.IsLogged)
				{
					if (verify != null)
						return verify == response.Parameter ? null : response.Message;

					return null;
				}
				else
				{
					Debug.WriteLine("[Account] Failed to update setting. Reason: {0}", response.Message);
					return response.Message;
				}
			}
			catch (InvalidHttpResponseException ex)
			{
				if (ex.HttpResponseMessage.StatusCode == HttpStatusCode.Unauthorized)
					throw new InvalidCredentialsException();

				throw;
			}
		}

		#endregion

		#region Updating

		/// <summary>
		/// Updates the properties in this <see cref="Account"/> to reflect the latest data obtained
		/// from Snapchat.
		/// </summary>
		/// <exception cref="InvalidCredentialsException">
		/// <see cref="AuthToken"/> is expired or invalid.
		/// </exception>
		public async Task UpdateAccountAsync()
		{
			Contract.Requires(!string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(AuthToken));
			Debug.WriteLine("[Account] Updating account [username: {0} | auth-token: {1}]", Username, AuthToken);

			var data = new Dictionary<string, string> { { "username", Username } };

			try
			{
				var response = await EndpointManager.Managers["loq"].PostAsync<AllUpdatesResponse>("all_updates", data, AuthToken);

				await UpdateFriendsPublicActivityAsync();
				UpdateFromResponse(response.AccountResponse);
				UpdateStoriesFromResponse(response.StoriesResponse);

				// TODO: Parse the rest of the data
			}
			catch (InvalidHttpResponseException ex)
			{
				if (ex.HttpResponseMessage.StatusCode == HttpStatusCode.Unauthorized)
					throw new InvalidCredentialsException();

				throw;
			}
		}

		/// <summary>
		/// Updates personal and friends' stories based on the given <paramref name="response"/>.
		/// </summary>
		/// <param name="response">A <see cref="StoriesResponse"/> instance.</param>
		private void UpdateStoriesFromResponse(StoriesResponse response)
		{
			Contract.Requires<ArgumentNullException>(response != null);

			// Check for new personal stories.
			foreach (var personalStory in response.MyStories.OrderByDescending(s => s.Story.TimeLeft).Where(friendStory => PersonalStories.FirstOrDefault(s => s.Id == friendStory.Story.Id) == null))
				PersonalStories.Add(PersonalStory.CreateFromResponse(personalStory));

			// Check for active personal stories, and update them.
			foreach (var personalStory in PersonalStories)
			{
				var personalStoryMetadata = response.MyStories.FirstOrDefault(s => s.Story.Id == personalStory.Id);
				if (personalStoryMetadata == null) continue;
				personalStory.UpdateFromResponse(personalStoryMetadata);
			}

			// Check for expired personal stories, and delete them.
			foreach (var redundantStory in PersonalStories.Where(s => response.MyStories.FirstOrDefault(s1 => s1.Story.Id == s.Id) == null).ToList())
				PersonalStories.Remove(redundantStory);

			// Update friends' stories.
			foreach (var friendStory in response.FriendStories)
			{
				var friend = Friends.FirstOrDefault(f => f.Username == friendStory.Username && f.Username != Username);
				if (friend == null) continue;
				friend.UpdateStoriesFromResponse(friendStory);
			}
		}

		/// <summary>
		/// Updates each friend's public activity.
		/// </summary>
		/// <exception cref="InvalidCredentialsException">
		/// <see cref="AuthToken"/> is expired or invalid.
		/// </exception>
		private async Task UpdateFriendsPublicActivityAsync()
		{
			var publicActivities = new Dictionary<string, PublicActivityResponse>();
			foreach (var chunk in Friends.Chunk(30))
			{
				var data = new Dictionary<string, string>
				{
					{ "username", Username },
					{ "friend_usernames", JsonConvert.SerializeObject(chunk.Select(u => u.Username)) }
				};

				try
				{
					var response = await EndpointManager.Managers["bq"].PostAsync<Dictionary<string, PublicActivityResponse>>("bests", data, AuthToken);
					foreach (var publicActivity in response)
						publicActivities.Add(publicActivity.Key, publicActivity.Value);
				}
				catch (InvalidHttpResponseException ex)
				{
					if (ex.HttpResponseMessage.StatusCode == HttpStatusCode.Unauthorized)
						throw new InvalidCredentialsException();

					throw;
				}
			}

			foreach (var publicActivity in publicActivities)
			{
				var selectedFriend = Friends.FirstOrDefault(f => f.Username == publicActivity.Key);
				selectedFriend.UpdatePublicActivity(publicActivity.Value, Friends);
			}
		}

		/// <summary>
		/// Updates the properties in this instance to reflect the values in the given
		/// <paramref name="response"/>.
		/// </summary>
		/// <param name="response">An <see cref="AccountResponse"/> instance.</param>
		private void UpdateFromResponse(AccountResponse response)
		{
			Contract.Requires<ArgumentNullException>(response != null);

			AuthToken = response.AuthToken;
			Username = response.Username;
			Email = response.Email;
			PhoneNumber = response.PhoneNumber;
			LastReplayed = response.LastReplayedSnapTimestamp;
			PhoneNumberVerificationKey = response.PhoneNumberVerificationKey;
			VerificationPhoneNumber = response.SnapchatPhoneNumber;
			ShouldTextToVerifyPhoneNumber = response.ShouldSendTextToVerifyNumber;
			ShouldCallToVerifyPhoneNumber = response.ShouldCallToVerifyNumber;
			IsSearchableByPhoneNumber = response.SearchableByPhoneNumber;
			Score = response.Score;
			SnapsReceived = response.SnapsReceived;
			SnapsSent = response.SnapsSent;
			Birthday = response.Birthday;
			CanViewMatureContent = response.CanViewMatureContent;
			IsBlockedFromOurStory = response.BlockedFromOurStory;
			ShouldPlayNotificationSound = (response.NotificationSoundSetting == "ON");
			NumberOfBestFriends = response.NumberOfBestFriends;

			// Update recent friends.
			_recentFriends.Clear();
			foreach (var friendName in response.RecentFriends)
			{
				var friend = Friends.FirstOrDefault(p => p.Username == friendName);
				if (friend != null) _recentFriends.Add(friend);
			}

			// Remove removed friends.
			var removedFriends = new List<Friend>();
			foreach (var friend in Friends)
			{
				if (response.Friends.FirstOrDefault(f => f.Name == friend.Username) == null)
					removedFriends.Add(friend);
			}
			foreach (var friend in removedFriends)
				_friends.Remove(friend);

			// Add new friends.
			var newFriends = new List<Friend>();
			foreach (var friend in response.Friends)
			{
				if (Friends.FirstOrDefault(f => f.Username == friend.Name) == null)
					newFriends.Add(Friend.CreateFromResponse(this, friend));
			}
			foreach (var friend in newFriends)
				_friends.Add(friend);

			// Update existing friends.
			var existingFriends = response.Friends.Where(f1 => Friends.FirstOrDefault(f => f.Username == f1.Name) != null);
			foreach (var existingFriend in existingFriends)
			{
				var friend = Friends.FirstOrDefault(f => existingFriend.Name == f.Username);
				if (friend == null) continue;
				friend.UpdateFromResponse(existingFriend);
			}
		}

		#endregion

		#region Media Objects

		/// <summary>
		/// Downloads the thumbnail for the most recent story of each friend.
		/// </summary>
		public async Task DownloadStoryThumbnailsAsync()
		{
			// TODO: Download in batches of 5.

			foreach (var friend in Friends)
			{
				var story = friend.Stories.FirstOrDefault();
				if (story != null)
				{
					await story.DownloadThumbnailAsync();
				}
			}
		}

		#endregion
	}
}
