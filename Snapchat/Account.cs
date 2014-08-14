using Newtonsoft.Json;
using SnapDotNet.Responses;
using SnapDotNet.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using Windows.Web.Http;

namespace SnapDotNet
{
	public sealed class Account
		: ObservableObject
	{
		private Account() { }

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
				return new Account
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
					Birthday = response.Birthday,
				};
			}
			catch (InvalidHttpResponseException ex)
			{
				if (ex.HttpResponseMessage.StatusCode == HttpStatusCode.Unauthorized)
					throw new InvalidCredentialsException();

				throw;
			}
		}

		// NOTE: JsonProperty attribute is needed to force JsonConvert to assign values to
		// properties with non-public setters so it can be deserialized too.

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
			private set { SetValue(ref _snapsReceived, value); }
		}
		private int _snapsReceived;

		/// <summary>
		/// Gets the number of Snaps this user has sent.
		/// </summary>
		[JsonProperty]
		public int SnapsSent
		{
			get { return _snapsSent; }
			private set { SetValue(ref _snapsSent, value); }
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
		internal string AuthToken
		{
			get { return _authToken; }
			private set { SetValue(ref _authToken, value); }
		}
		private string _authToken;

		/*

		//[DataMember(Name = "added_friends")]
		//public ObservableCollection<AddedFriend> AddedFriends { get; set; }

		[DataMember(Name = "bests")]
		public ObservableCollection<string> BestFriends { get; set; }

		//[DataMember(Name = "friends")]
		//public ObservableCollection<Friend> Friends { get; set; }

		[DataMember(Name = "notification_sound_setting")]
		public string NotificationSoundSetting { get; set; }

		[DataMember(Name = "recents")]
		public ObservableCollection<string> RecentFriends { get; set; }

		[DataMember(Name = "requests")]
		public ObservableCollection<string> FriendRequests { get; set; }

		[DataMember(Name = "snap_p")]
		public string AccountPrivacySetting { get; set; }

		[DataMember(Name = "story_privacy")]
		public string StoryPrivacySetting { get; set; }*/
	}
}
