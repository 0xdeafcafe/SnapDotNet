using Newtonsoft.Json;
using SnapDotNet.Converters.Json;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SnapDotNet.Data.ApiResponses
{
	[DataContract]
	internal sealed class AccountResponse
		: Response
	{
		[DataMember(Name = "added_friends_timestamp")]
		[JsonConverter(typeof(UnixDateTimeConverter))]
		public DateTime AddedFriendsTimestamp { get; set; }

		[DataMember(Name = "added_friends")]
		public AddedFriendResponse[] AddedFriends { get; set; }

		[DataMember(Name = "auth_token")]
		public string AuthToken { get; set; }

		[DataMember(Name = "bests")]
		public string[] BestFriends { get; set; }

		[DataMember(Name = "can_view_mature_content")]
		public bool CanViewMatureContent { get; set; }

		[DataMember(Name = "country_code")]
		public string CountryCode { get; set; }

		[DataMember(Name = "current_timestamp")]
		[JsonConverter(typeof(UnixDateTimeConverter))]
		public DateTime CurrentTimestamp { get; set; }

		[DataMember(Name = "device_token")]
		public string DeviceToken { get; set; }

		[DataMember(Name = "email")]
		public string Email { get; set; }

		[DataMember(Name = "friends")]
		public List<FriendResponse> Friends { get; set; }

		[DataMember(Name = "last_replayed_snap_timestamp")]
		[JsonConverter(typeof(UnixDateTimeConverter))]
		public DateTime LastReplayedSnapTimestamp { get; set; }

		[DataMember(Name = "mobile")]
		public string PhoneNumber { get; set; }

		[DataMember(Name = "mobile_verification_key")]
		public string PhoneNumberVerificationKey { get; set; }

		[DataMember(Name = "notification_sound_setting")]
		public string NotificationSoundSetting { get; set; }

		[DataMember(Name = "number_of_best_friends")]
		public int NumberOfBestFriends { get; set; }

		[DataMember(Name = "received")]
		public int SnapsReceived { get; set; }

		[DataMember(Name = "recents")]
		public string[] RecentFriends { get; set; }

		[DataMember(Name = "requests")]
		public string[] FriendRequests { get; set; }

		[DataMember(Name = "score")]
		public int Score { get; set; }

		[DataMember(Name = "searchable_by_phone_number")]
		public bool SearchableByPhoneNumber { get; set; }

		[DataMember(Name = "sent")]
		public int SnapsSent { get; set; }

		[DataMember(Name = "should_call_to_verify_number")]
		public bool ShouldCallToVerifyNumber { get; set; }

		[DataMember(Name = "should_send_text_to_verify_number")]
		public bool ShouldSendTextToVerifyNumber { get; set; }

		[DataMember(Name = "snap_p")]
		public string AccountPrivacySetting { get; set; }

		[DataMember(Name = "snapchat_phone_number")]
		public string SnapchatPhoneNumber { get; set; }

		[DataMember(Name = "story_privacy")]
		public string StoryPrivacySetting { get; set; }

		[DataMember(Name = "username")]
		public string Username { get; set; }

		[DataMember(Name = "blocked_from_using_our_story")]
		public bool BlockedFromOurStory { get; set; }

		[DataMember(Name = "image_caption")]
		public bool ImageCaption { get; set; }

		[DataMember(Name = "birthday")]
		public DateTime Birthday { get; set; }

		[DataMember(Name = "feature_settings")]
		public Dictionary<string, bool> FeatureSettings { get; set; }
	}
}
