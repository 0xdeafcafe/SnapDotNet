using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using SnapDotNet.Core.Miscellaneous.CustomTypes;
using SnapDotNet.Core.Miscellaneous.Extensions;
using SnapDotNet.Core.Miscellaneous.Models;
using SnapDotNet.Core.Snapchat.Converters.Json;

namespace SnapDotNet.Core.Snapchat.Models.New
{
	[DataContract]
	public class UpdatesResponse
		: NotifyPropertyChangedBase
	{
		public UpdatesResponse()
		{
			_addedFriends.CollectionChanged += (sender, args) => NotifyPropertyChanged("AddedFriends");
			_friends.CollectionChanged += (sender, args) => { NotifyPropertyChanged("Friends"); NotifyPropertyChanged("SortedFriends"); };
			_recentFriends.CollectionChanged += (sender, args) => NotifyPropertyChanged("RecentFriends");
		}

		[DataMember(Name = "added_friends")]
		public ObservableCollection<AddedFriend> AddedFriends
		{
			get { return _addedFriends; }
			set { SetField(ref _addedFriends, value); }
		}
		private ObservableCollection<AddedFriend> _addedFriends = new ObservableCollection<AddedFriend>();
		
		[DataMember(Name = "added_friends_timestamp")]
		[JsonConverter(typeof(UnixDateTimeConverter))]
		public DateTime AddedFriendsTimestamp { get; set; }

		[DataMember(Name = "auth_token")]
		public String AuthToken { get; set; }

		[DataMember(Name = "bests")]
		public String[] BestFriends { get; set; }

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

		[DataMember(Name = "feature_settings")]
		public FeatureSettings FeatureSettings { get; set; }

		[DataMember(Name = "friends")]
		public ObservableCollection<Friend> Friends
		{
			get { return _friends; }
			set { SetField(ref _friends, value); }
		}
		private ObservableCollection<Friend> _friends = new ObservableCollection<Friend>();

		[IgnoreDataMember]
		public ObservableCollection<AlphaKeyGroup<Friend>> SortedFriends
		{
			get { return AlphaKeyGroup<Friend>.CreateGroups(Friends, new CultureInfo("en"), f => f.FriendlyName, true); }
		}
			
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
		public Int32 RecievedSnaps
		{
			get { return _recievedSnaps; }
			set
			{
				SetField(ref _recievedSnaps, value);
				NotifyPropertyChanged("FriendlyRatio");
			}
		}
		private Int32 _recievedSnaps;

		[DataMember(Name = "recents")]
		public ObservableCollection<String> RecentFriends
		{
			get { return _recentFriends; }
			set { SetField(ref _recentFriends, value); }
		}
		private ObservableCollection<String> _recentFriends = new ObservableCollection<String>();

		[DataMember(Name = "requests")]
		public ObservableCollection<string> Requests { get; set; }

		[DataMember(Name = "score")]
		public Int32 Score
		{
			get { return _score; }
			set
			{
				SetField(ref _score, value);
				NotifyPropertyChanged("FriendlyRatio");
			}
		}
		private Int32 _score;

		[IgnoreDataMember]
		public String FriendlyRatio
		{
			get
			{
				return String.Format("{0} | {1}", SentSnaps.ToDelimiter(), RecievedSnaps.ToDelimiter());
			}
		}

		[DataMember(Name = "searchable_by_phone_number")]
		public Boolean SearchableByPhoneNumber { get; set; }

		[DataMember(Name = "sent")]
		public Int32 SentSnaps
		{
			get { return _sentSnaps; }
			set
			{
				SetField(ref _sentSnaps, value);
				NotifyPropertyChanged("FriendlyRatio");
			}
		}
		private Int32 _sentSnaps;

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

	[DataContract]
	public class AddedFriend
		: NotifyPropertyChangedBase
	{
		[DataMember(Name = "direction")]
		public String Direction { get; set; }

		[DataMember(Name = "display")]
		public String Display { get; set; }

		[IgnoreDataMember]
		public String FriendlyName { get { return String.IsNullOrWhiteSpace(Display) ? Name : Display; } }

		[DataMember(Name = "name")]
		public String Name { get; set; }

		[DataMember(Name = "ts")]
		[JsonConverter(typeof(UnixDateTimeConverter))]
		public DateTime AddedAt { get; set; }

		[DataMember(Name = "type")]
		public FriendRequestState Type { get; set; }
	}

	[DataContract]
	public class Friend
		: NotifyPropertyChangedBase
	{

		[DataMember(Name = "can_see_custom_stories")]
		public Boolean CanSeeCustomStories { get; set; }

		[DataMember(Name = "direction")]
		public String Direction { get; set; }

		[DataMember(Name = "display")]
		public String Display { get; set; }

		[IgnoreDataMember]
		public String FriendlyName { get { return String.IsNullOrWhiteSpace(Display) ? Name : Display; } }

		[DataMember(Name = "name")]
		public String Name { get; set; }

		[DataMember(Name = "type")]
		public FriendRequestState Type { get; set; }
	}

	[DataContract]
	public class FeatureSettings
		: NotifyPropertyChangedBase
	{
		[DataMember(Name = "font_facing_flash")]
		public Boolean FrontFacingFlash { get; set; }

		[DataMember(Name = "replay_snaps")]
		public Boolean ReplaySnaps { get; set; }

		[DataMember(Name = "smart_filters")]
		public Boolean SmartFilters { get; set; }

		[DataMember(Name = "special_text")]
		public Boolean SpecialText { get; set; }

		[DataMember(Name = "visual_filters")]
		public Boolean VisualFilters { get; set; }
	}

	/// <summary>
	/// Actions concerning other snapchat users
	/// </summary>
	public enum FriendAction
	{
		Add,
		Delete,
		Block,
		Unblock
	}

	/// <summary>
	/// Indicates the state of a friend request.
	/// </summary>
	public enum FriendRequestState
	{
		Accepted,
		Pending,
		Blocked
	}

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
	/// Indicates an account's privacy mode.
	/// </summary>
	public enum StoryPrivacy
	{
		/// <summary>
		/// Everyone can see the story.
		/// </summary>
		Everyone,

		/// <summary>
		/// Only friends can see the story.
		/// </summary>
		Friends,

		/// <summary>
		/// Only friends can see the story, but the user can block certain friends.
		/// </summary>
		Custom
	}
}
