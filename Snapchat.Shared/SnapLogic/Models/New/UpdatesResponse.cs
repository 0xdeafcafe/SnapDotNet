using System;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Snapchat.Models;
using Snapchat.SnapLogic.Converters.Json;
using SnapDotNet.Core.Miscellaneous.Models;

namespace Snapchat.SnapLogic.Models.New
{
	[DataContract]
	public class UpdatesResponse
		: NotifyPropertyChangedBase
	{
		[DataMember(Name = "added_friends")]
		public ObservableCollection<AddedFriend> AddedFriends { get; set; }

		[DataMember(Name = "added_friends_timestamp")]
		[JsonConverter(typeof (UnixDateTimeConverter))]
		public DateTime AddedFriendsTimestamp { get; set; }

		[DataMember(Name = "auth_token")]
		public String AuthToken { get; set; }

		[DataMember(Name = "bests")]
		public ObservableCollection<String> BestFriends { get; set; }

		[DataMember(Name = "can_view_mature_content")]
		public Boolean CanViewMatureContent { get; set; }

		[DataMember(Name = "country_code")]
		public String CountryCode { get; set; }

		[DataMember(Name = "current_timestamp")]
		[JsonConverter(typeof(UnixDateTimeConverter))]
		public DateTime CurrentTimestamp { get; set; }

		[DataMember(Name = "device_token")]
		public String DeviceToken { get; set; }

		[DataMember(Name = "email")]
		public String Email { get; set; }

		[DataMember(Name = "friends")]
		public ObservableCollection<Friend> Friends { get; set; }
		
		[DataMember(Name = "last_replayed_snap_timestamp")]
		[JsonConverter(typeof(UnixDateTimeConverter))]
		public DateTime LastReplayedSnap { get; set; }

		[DataMember(Name = "logged")]
		public Boolean Logged { get; set; }
		
		[DataMember(Name = "mobile")]
		public String Mobile { get; set; }

		[DataMember(Name = "mobile_verification_key")]
		public String MobileVerificationKey { get; set; }

		[DataMember(Name = "notification_sound_setting")]
		public String NotificationSoundSetting { get; set; }

		[DataMember(Name = "number_of_best_friends")]
		public Int32 NumberOfBestFriends { get; set; }

		[DataMember(Name = "received")]
		public Int32 RecievedSnaps { get; set; }

		[DataMember(Name = "recents")]
		public ObservableCollection<String> RecentFriends { get; set; }

		[DataMember(Name = "requests")]
		public ObservableCollection<String> Requests { get; set; }

		[DataMember(Name = "score")]
		public Int32 Score { get; set; }

		[DataMember(Name = "searchable_by_phone_number")]
		public Boolean SearchableByPhoneNumber { get; set; }

		[DataMember(Name = "sent")]
		public Int32 SentSnaps { get; set; }

		[DataMember(Name = "should_call_to_verify_number")]
		public Boolean ShouldCallToVerifyNumber { get; set; }

		[DataMember(Name = "should_send_text_to_verify_number")]
		public Boolean ShouldSendTextToVerifyNumber { get; set; }

		[DataMember(Name = "snap_p")]
		public AccountPrivacy AccountPrivacy { get; set; }

		[DataMember(Name = "snapchat_phone_number")]
		public String SnapchatPhoneNumber { get; set; }

		[DataMember(Name = "story_privacy")]
		public StoryPrivacy StoryPrivacy { get; set; }

		[DataMember(Name = "username")]
		public String Username { get; set; }
	}
}
