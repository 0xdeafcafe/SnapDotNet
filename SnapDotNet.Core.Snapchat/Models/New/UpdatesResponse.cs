using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using SnapDotNet.Core.Miscellaneous.CustomTypes;
using SnapDotNet.Core.Miscellaneous.Extensions;
using SnapDotNet.Core.Miscellaneous.Models;
using SnapDotNet.Core.Snapchat.Converters.Json;
using SnapDotNet.Core.Snapchat.Models.New.Responses;

namespace SnapDotNet.Core.Snapchat.Models.New
{
	[DataContract]
	public class UpdatesResponse
		: NotifyPropertyChangedBase
	{
		public UpdatesResponse()
		{
			_addedFriends.CollectionChanged += (sender, args) => NotifyPropertyChanged("AddedFriends");
			_bestFriends.CollectionChanged += (sender, args) => NotifyPropertyChanged("BestFriends");
			_friends.CollectionChanged += (sender, args) => { NotifyPropertyChanged("Friends"); NotifyPropertyChanged("SortedFriends"); };
			_recentFriends.CollectionChanged += (sender, args) => NotifyPropertyChanged("RecentFriends");
			_requests.CollectionChanged += (sender, args) => NotifyPropertyChanged("Requests");
		}

		[DataMember(Name = "added_friends")]
		public ObservableCollection<AddedFriend> AddedFriends
		{
			get { return _addedFriends; }
			set { SetField(ref _addedFriends, value); }
		}
		private ObservableCollection<AddedFriend> _addedFriends = new ObservableCollection<AddedFriend>();

		[DataMember(Name = "added_friends_timestamp")]
		[JsonConverter(typeof (UnixDateTimeConverter))]
		public DateTime AddedFriendsTimestamp
		{
			get { return _addedFriendsTimestamp; }
			set { SetField(ref _addedFriendsTimestamp, value); }
		}
		private DateTime _addedFriendsTimestamp;

		[DataMember(Name = "auth_token")]
		public String AuthToken
		{
			get { return _authToken; }
			set { SetField(ref _authToken, value); }
		}
		private String _authToken;

		[DataMember(Name = "bests")]
		public ObservableCollection<String> BestFriends
		{
			get { return _bestFriends; }
			set { SetField(ref _bestFriends, value); }
		}
		private ObservableCollection<String> _bestFriends = new ObservableCollection<String>();

		[DataMember(Name = "can_view_mature_content")]
		public Boolean CanViewMatureContent
		{
			get { return _canViewMatureContent; }
			set { SetField(ref _canViewMatureContent, value); }
		}
		private Boolean _canViewMatureContent;

		[DataMember(Name = "country_code")]
		public String CountryCode
		{
			get { return _countryCode; }
			set { SetField(ref _countryCode, value); }
		}
		private String _countryCode;

		[DataMember(Name = "current_timestamp")]
		[JsonConverter(typeof(UnixDateTimeConverter))]
		public DateTime CurrentTimestamp
		{
			get { return _currentTimestamp; }
			set { SetField(ref _currentTimestamp, value); }
		}
		private DateTime _currentTimestamp;

		[DataMember(Name = "device_token")]
		public String DeviceToken
		{
			get { return _deviceToken; }
			set { SetField(ref _deviceToken, value); }
		}
		private String _deviceToken;

		[DataMember(Name = "email")]
		public String Email
		{
			get { return _email; }
			set { SetField(ref _email, value); }
		}
		private String _email;

		[DataMember(Name = "feature_settings")]
		public FeatureSettings FeatureSettings
		{
			get { return _featureSettings; }
			set { SetField(ref _featureSettings, value); }
		}
		private FeatureSettings _featureSettings;

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
			get { return AlphaKeyGroup<Friend>.CreateGroups(Friends, new CultureInfo("en"), f => f.FriendlyName, true, false); }
		}
			
		[DataMember(Name = "last_replayed_snap_timestamp")]
		[JsonConverter(typeof(UnixDateTimeConverter))]
		public DateTime LastReplayedSnap
		{
			get { return _lastReplayedSnap; }
			set { SetField(ref _lastReplayedSnap, value); }
		}
		private DateTime _lastReplayedSnap;

		[DataMember(Name = "logged")]
		public Boolean Logged
		{
			get { return _logged; }
			set { SetField(ref _logged, value); }
		}
		private Boolean _logged;
		
		[DataMember(Name = "mobile")]
		public String Mobile
		{
			get { return _mobile; }
			set { SetField(ref _mobile, value); }
		}
		private String _mobile;

		[DataMember(Name = "mobile_verification_key")]
		public String MobileVerificationKey
		{
			get { return _mobileVerificationKey; }
			set { SetField(ref _mobileVerificationKey, value); }
		}
		private String _mobileVerificationKey;

		[DataMember(Name = "notification_sound_setting")]
		public String NotificationSoundSetting
		{
			get { return _notificationSoundSetting; }
			set { SetField(ref _notificationSoundSetting, value); }
		}
		private String _notificationSoundSetting;

		[DataMember(Name = "number_of_best_friends")]
		public Int32 NumberOfBestFriends
		{
			get { return _numberOfBestFriends; }
			set { SetField(ref _numberOfBestFriends, value); }
		}
		private Int32 _numberOfBestFriends;

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
		public ObservableCollection<String> Requests
		{
			get { return _requests; }
			set { SetField(ref _requests, value); }
		}
		private ObservableCollection<String> _requests = new ObservableCollection<String>();

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
		public Boolean SearchableByPhoneNumber
		{
			get { return _searchableByPhoneNumber; }
			set { SetField(ref _searchableByPhoneNumber, value); }
		}
		private Boolean _searchableByPhoneNumber;

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
		public Boolean ShouldCallToVerifyNumber
		{
			get { return _shouldCallToVerifyNumber; }
			set { SetField(ref _shouldCallToVerifyNumber, value); }
		}
		private Boolean _shouldCallToVerifyNumber;

		[DataMember(Name = "should_send_text_to_verify_number")]
		public Boolean ShouldSendTextToVerifyNumber
		{
			get { return _shouldSendTextToVerifyNumber; }
			set { SetField(ref _shouldSendTextToVerifyNumber, value); }
		}
		private Boolean _shouldSendTextToVerifyNumber;

		[DataMember(Name = "snap_p")]
		public AccountPrivacy AccountPrivacy
		{
			get { return _accountPrivacy; }
			set { SetField(ref _accountPrivacy, value); }
		}
		private AccountPrivacy _accountPrivacy;

		[DataMember(Name = "snapchat_phone_number")]
		public String SnapchatPhoneNumber
		{
			get { return _snapchatPhoneNumber; }
			set { SetField(ref _snapchatPhoneNumber, value); }
		}
		private String _snapchatPhoneNumber;

		[DataMember(Name = "story_privacy")]
		public StoryPrivacy StoryPrivacy
		{
			get { return _storyPrivacy; }
			set { SetField(ref _storyPrivacy, value); }
		}
		private StoryPrivacy _storyPrivacy;

		[DataMember(Name = "username")]
		public String Username
		{
			get { return _username; }
			set { SetField(ref _username, value); }
		}
		private String _username;

		#region Helpers

		#endregion
	}

	[DataContract]
	public class AddedFriend
		: NotifyPropertyChangedBase
	{
		[DataMember(Name = "direction")]
		public String Direction
		{
			get { return _direction; }
			set { SetField(ref _direction, value); }
		}
		private String _direction;

		[DataMember(Name = "display")]
		public String Display
		{
			get { return _display; }
			set { SetField(ref _display, value); }
		}
		private String _display;

		[DataMember(Name = "name")]
		public String Name
		{
			get { return _name; }
			set { SetField(ref _name, value); }
		}
		private String _name;

		[DataMember(Name = "ts")]
		[JsonConverter(typeof(UnixDateTimeConverter))]
		public DateTime AddedAt
		{
			get { return _addedAt; }
			set { SetField(ref _addedAt, value); }
		}
		private DateTime _addedAt;

		[DataMember(Name = "type")]
		public FriendRequestState Type
		{
			get { return _type; }
			set { SetField(ref _type, value); }
		}
		private FriendRequestState _type;

		#region Helpers

		[IgnoreDataMember]
		public String FriendlyName { get { return String.IsNullOrWhiteSpace(Display) ? Name : Display; } }

		#endregion
	}

	[DataContract]
	public class Friend
		: NotifyPropertyChangedBase
	{
		[DataMember(Name = "can_see_custom_stories")]
		public Boolean CanSeeCustomStories
		{
			get { return _canSeeCustomStories; }
			set { SetField(ref _canSeeCustomStories, value); }
		}
		private Boolean _canSeeCustomStories;

		[DataMember(Name = "direction")]
		public String Direction
		{
			get { return _direction; }
			set { SetField(ref _direction, value); }
		}
		private String _direction;

		[DataMember(Name = "display")]
		public String Display
		{
			get { return _display; }
			set
			{
				SetField(ref _display, value);
				NotifyPropertyChanged("Name");
				NotifyPropertyChanged("FriendlyName");
				NotifyPropertyChanged("HasFriendlyName");
			}
		}
		private String _display;
		
		[DataMember(Name = "name")]
		public String Name
		{
			get { return _name; }
			set
			{
				SetField(ref _name, value);
				NotifyPropertyChanged("Display");
				NotifyPropertyChanged("FriendlyName");
				NotifyPropertyChanged("HasFriendlyName");
			}
		}
		private String _name;

		[DataMember(Name = "type")]
		public FriendRequestState Type
		{
			get { return _type; }
			set { SetField(ref _type, value); }
		}
		private FriendRequestState _type;

		[DataMember(Name = "public_activity")]
		public PublicActivity PublicActivity
		{
			get { return _publicActivity; }
			set { SetField(ref _publicActivity, value); }
		}
		private PublicActivity _publicActivity = new PublicActivity();

		#region Helpers

		[IgnoreDataMember]
		public String FriendlyName { get { return String.IsNullOrWhiteSpace(Display) ? Name : Display; } }

		[IgnoreDataMember]
		public Boolean HasFriendlyName { get { return FriendlyName == Display; } }

		#endregion
	}

	[DataContract]
	public class FeatureSettings
		: NotifyPropertyChangedBase
	{
		[DataMember(Name = "font_facing_flash")]
		public Boolean FrontFacingFlash
		{
			get { return _frontFacingFlash; }
			set { SetField(ref _frontFacingFlash, value); }
		}
		private Boolean _frontFacingFlash;

		[DataMember(Name = "replay_snaps")]
		public Boolean ReplaySnaps
		{
			get { return _replaySnaps; }
			set { SetField(ref _replaySnaps, value); }
		}
		private Boolean _replaySnaps;

		[DataMember(Name = "smart_filters")]
		public Boolean SmartFilters
		{
			get { return _smartFilters; }
			set { SetField(ref _smartFilters, value); }
		}
		private Boolean _smartFilters;

		[DataMember(Name = "special_text")]
		public Boolean SpecialText
		{
			get { return _specialText; }
			set { SetField(ref _specialText, value); }
		}
		private Boolean _specialText;

		[DataMember(Name = "visual_filters")]
		public Boolean VisualFilters
		{
			get { return _visualFilters; }
			set { SetField(ref _visualFilters, value); }
		}
		private Boolean _visualFilters;
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
