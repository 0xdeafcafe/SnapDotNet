using System;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using SnapDotNet.Core.Snapchat.Converters.Json;

namespace SnapDotNet.Core.Snapchat.Models
{
	/// <summary>
	/// Indicates an account's privacy mode.
	/// </summary>
	public enum AccountPrivacy
	{
		/// <summary>
		/// Everyone can send snaps to the account.
		/// </summary>
		Everyone,

		/// <summary>
		/// Only friends can send snaps to the account.
		/// </summary>
		Friends
	}

	/// <summary>
	/// Represents an account.
	/// </summary>
	[DataContract]
	public class Account
		: Response
	{
		[DataMember(Name = "bests")]
		public ObservableCollection<string> BestFriends
		{
			get { return _bestFriends; }
			set { SetField(ref _bestFriends, value); }
		}
		private ObservableCollection<string> _bestFriends;

		[DataMember(Name = "is_vip")]
		public bool IsVip
		{
			get { return _isVip; }
			set { SetField(ref _isVip, value); }
		}
		private bool _isVip;

		[DataMember(Name = "score")]
		public int Score
		{
			get { return _score; }
			set { SetField(ref _score, value); }
		}
		private int _score;

		[DataMember(Name = "received")]
		public int Received
		{
			get { return _received; }
			set { SetField(ref _received, value); }
		}
		private int _received;

		[DataMember(Name = "added_friends")]
		public ObservableCollection<AddedFriend> AddedFriends
		{
			get { return _addedFriends; }
			set { SetField(ref _addedFriends, value); }
		}
		private ObservableCollection<AddedFriend> _addedFriends;

		[DataMember(Name = "requests")]
		public ObservableCollection<string> Requests
		{
			get { return _requests; }
			set { SetField(ref _requests, value); }
		}
		private ObservableCollection<string> _requests;

		[DataMember(Name = "sent")]
		public int Sent
		{
			get { return _sent; }
			set { SetField(ref _sent, value); }
		}
		private int _sent;

		[DataMember(Name = "story_privacy")]
		public string StoryPrivacy
		{
			get { return _storyPrivacy; }
			set { SetField(ref _storyPrivacy, value); }
		}
		private string _storyPrivacy;

		[DataMember(Name = "username")]
		public string Username
		{
			get { return _username; }
			set { SetField(ref _username, value); }
		}
		private string _username;

		[DataMember(Name = "friends")]
		public ObservableCollection<Friend> Friends
		{
			get { return _friends; }
			set { SetField(ref _friends, value); }
		}
		private ObservableCollection<Friend> _friends;

		[DataMember(Name = "device_token")]
		public string DeviceToken
		{
			get { return _deviceToken; }
			set { SetField(ref _deviceToken, value); }
		}
		private string _deviceToken;

		[DataMember(Name = "snap_p")]
		public AccountPrivacy AccountPrivacy
		{
			get { return _accountPrivacy; }
			set { SetField(ref _accountPrivacy, value); }
		}
		private AccountPrivacy _accountPrivacy;

		[DataMember(Name = "mobile_verification_key")]
		public string MobileVerificationKey
		{
			get { return _mobileVerificationKey; }
			set { SetField(ref _mobileVerificationKey, value); }
		}
		private string _mobileVerificationKey;

		[DataMember(Name = "recents")]
		public ObservableCollection<string> Recents
		{
			get { return _recents; }
			set { SetField(ref _recents, value); }
		}
		private ObservableCollection<string> _recents;

		[DataMember(Name = "added_friends_timestamp")]
		[JsonConverter(typeof(UnixDateTimeConverter))]
		public DateTime AddedFriendsTimestamp
		{
			get { return _addedFriendsTimestamp; }
			set { SetField(ref _addedFriendsTimestamp, value); }
		}
		private DateTime _addedFriendsTimestamp;

		[DataMember(Name = "notification_sound_setting")]
		public string NotificationSoundSettings
		{
			get { return _notificationSoundSettings; }
			set { SetField(ref _notificationSoundSettings, value); }
		}
		private string _notificationSoundSettings;

		[DataMember(Name = "snapchat_phone_number")]
		public string PhoneNumber
		{
			get { return _phoneNumber; }
			set { SetField(ref _phoneNumber, value); }
		}
		private string _phoneNumber;

		[DataMember(Name = "auth_token")]
		public string AuthToken
		{
			get { return _authToken; }
			set { SetField(ref _authToken, value); }
		}
		private string _authToken;
		
		[DataMember(Name = "image_caption")]
		public bool ImageCaption
		{
			get { return _imageCaption; }
			set { SetField(ref _imageCaption, value); }
		}
		private bool _imageCaption;

		[DataMember(Name = "country_code")]
		public string CountryCode
		{
			get { return _countryCode; }
			set { SetField(ref _countryCode, value); }
		}
		private string _countryCode;

		[DataMember(Name = "can_view_mature_content")]
		public bool CanViewMatureContent
		{
			get { return _canViewMatureContent; }
			set { SetField(ref _canViewMatureContent, value); }
		}
		private bool _canViewMatureContent;

		[DataMember(Name = "email")]
		public string Email
		{
			get { return _email; }
			set { SetField(ref _email, value); }
		}
		private string _email;

		[DataMember(Name = "should_send_text_to_verify_number")]
		public bool ShouldSendTextToVerifyNumber
		{
			get { return _shouldSendTextToVerifyNumber; }
			set { SetField(ref _shouldSendTextToVerifyNumber, value); }
		}
		private bool _shouldSendTextToVerifyNumber;

		[DataMember(Name = "mobile")]
		public string Mobile
		{
			get { return _mobile; }
			set { SetField(ref _mobile, value); }
		}
		private string _mobile;

		[DataMember(Name = "snaps")]
		public ObservableCollection<Snap> Snaps
		{
			get { return _snaps; }
			set { SetField(ref _snaps, value); }
		}
		private ObservableCollection<Snap> _snaps;
	}
}
