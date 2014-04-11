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
		public ObservableCollection<string> BestFriends { get; set; }

		[DataMember(Name = "is_vip")]
		public bool IsVip
		{
			get { return _isVip; }
			set { SetField(ref _isVip, value); }
		}

		private bool _isVip;

		[DataMember(Name = "score")]
		public int Score { get; set; }

		[DataMember(Name = "received")]
		public int Received { get; set; }

		[DataMember(Name = "added_friends")]
		public ObservableCollection<AddedFriend> AddedFriends { get; set; }

		[DataMember(Name = "requests")]
		public ObservableCollection<string> Requests { get; set; }

		[DataMember(Name = "sent")]
		public int Sent { get; set; }

		[DataMember(Name = "story_privacy")]
		public string StoryPrivacy { get; set; }

		[DataMember(Name = "username")]
		public string Username { get; set; }

		[DataMember(Name = "friends")]
		public ObservableCollection<Friend> Friends
		{
			get { return _friends; }
			set { SetField(ref _friends, value); }
		}
		private ObservableCollection<Friend> _friends;

		[DataMember(Name = "device_token")]
		public string DeviceToken { get; set; }

		[DataMember(Name = "snap_p")]
		public AccountPrivacy AccountPrivacy { get; set; }

		[DataMember(Name = "mobile_verification_key")]
		public string MobileVerificationKey { get; set; }

		[DataMember(Name = "recents")]
		public ObservableCollection<string> Recents { get; set; }

		[DataMember(Name = "added_friends_timestamp")]
		[JsonConverter(typeof(UnixDateTimeConverter))]
		public DateTime AddedFriendsTimestamp { get; set; }

		[DataMember(Name = "notification_sound_setting")]
		public string NotificationSoundSettings { get; set; }

		[DataMember(Name = "snapchat_phone_number")]
		public string PhoneNumber { get; set; }

		[DataMember(Name = "auth_token")]
		public string AuthToken { get; set; }
		
		[DataMember(Name = "image_caption")]
		public bool ImageCaption { get; set; }

		[DataMember(Name = "country_code")]
		public string CountryCode { get; set; }

		[DataMember(Name = "can_view_mature_content")]
		public bool CanViewMatureContent { get; set; }

		[DataMember(Name = "email")]
		public string Email { get; set; }

		[DataMember(Name = "should_send_text_to_verify_number")]
		public bool ShouldSendTextToVerifyNumber { get; set; }

		[DataMember(Name = "mobile")]
		public string Mobile { get; set; }

		[DataMember(Name = "snaps")]
		public ObservableCollection<Snap> Snaps { get; set; }
	}
}
