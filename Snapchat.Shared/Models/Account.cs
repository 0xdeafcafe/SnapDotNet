using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Snapchat.SnapLogic.Converters.Json;
using SnapDotNet.Core.Miscellaneous.CustomTypes;
using SnapDotNet.Core.Miscellaneous.Extensions;
using SnapDotNet.Core.Miscellaneous.Models;

namespace Snapchat.Models
{
	public class Account
		: NotifyPropertyChangedBase
	{
		public Account()
		{
			_addedFriends.CollectionChanged += (sender, args) => NotifyPropertyChanged("AddedFriends");
			_bestFriends.CollectionChanged += (sender, args) => NotifyPropertyChanged("BestFriends");
			_friends.CollectionChanged += (sender, args) => { NotifyPropertyChanged("Friends"); NotifyPropertyChanged("SortedFriends"); };
			_recentFriends.CollectionChanged += (sender, args) => NotifyPropertyChanged("RecentFriends");
			_requests.CollectionChanged += (sender, args) => NotifyPropertyChanged("Requests");
		}

		public ObservableCollection<AddedFriend> AddedFriends
		{
			get { return _addedFriends; }
			set { SetField(ref _addedFriends, value); }
		}
		private ObservableCollection<AddedFriend> _addedFriends = new ObservableCollection<AddedFriend>();

		[JsonConverter(typeof (UnixDateTimeConverter))]
		public DateTime AddedFriendsTimestamp
		{
			get { return _addedFriendsTimestamp; }
			set { SetField(ref _addedFriendsTimestamp, value); }
		}
		private DateTime _addedFriendsTimestamp;

		public String AuthToken
		{
			get { return _authToken; }
			set { SetField(ref _authToken, value); }
		}
		private String _authToken;

		public ObservableCollection<String> BestFriends
		{
			get { return _bestFriends; }
			set { SetField(ref _bestFriends, value); }
		}
		private ObservableCollection<String> _bestFriends = new ObservableCollection<String>();

		public Boolean CanViewMatureContent
		{
			get { return _canViewMatureContent; }
			set { SetField(ref _canViewMatureContent, value); }
		}
		private Boolean _canViewMatureContent;

		public String CountryCode
		{
			get { return _countryCode; }
			set { SetField(ref _countryCode, value); }
		}
		private String _countryCode;

		public DateTime CurrentTimestamp
		{
			get { return _currentTimestamp; }
			set { SetField(ref _currentTimestamp, value); }
		}
		private DateTime _currentTimestamp;

		public String DeviceToken
		{
			get { return _deviceToken; }
			set { SetField(ref _deviceToken, value); }
		}
		private String _deviceToken;

		public String Email
		{
			get { return _email; }
			set { SetField(ref _email, value); }
		}
		private String _email;

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
		
		public DateTime LastReplayedSnap
		{
			get { return _lastReplayedSnap; }
			set { SetField(ref _lastReplayedSnap, value); }
		}
		private DateTime _lastReplayedSnap;

		public Boolean Logged
		{
			get { return _logged; }
			set { SetField(ref _logged, value); }
		}
		private Boolean _logged;
		
		public String Mobile
		{
			get { return _mobile; }
			set { SetField(ref _mobile, value); }
		}
		private String _mobile;

		public String MobileVerificationKey
		{
			get { return _mobileVerificationKey; }
			set { SetField(ref _mobileVerificationKey, value); }
		}
		private String _mobileVerificationKey;

		public String NotificationSoundSetting
		{
			get { return _notificationSoundSetting; }
			set { SetField(ref _notificationSoundSetting, value); }
		}
		private String _notificationSoundSetting;

		public Int32 NumberOfBestFriends
		{
			get { return _numberOfBestFriends; }
			set { SetField(ref _numberOfBestFriends, value); }
		}
		private Int32 _numberOfBestFriends;

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

		public ObservableCollection<String> RecentFriends
		{
			get { return _recentFriends; }
			set { SetField(ref _recentFriends, value); }
		}
		private ObservableCollection<String> _recentFriends = new ObservableCollection<String>();

		public ObservableCollection<String> Requests
		{
			get { return _requests; }
			set { SetField(ref _requests, value); }
		}
		private ObservableCollection<String> _requests = new ObservableCollection<String>();

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

		public Boolean SearchableByPhoneNumber
		{
			get { return _searchableByPhoneNumber; }
			set { SetField(ref _searchableByPhoneNumber, value); }
		}
		private Boolean _searchableByPhoneNumber;

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

		public Boolean ShouldCallToVerifyNumber
		{
			get { return _shouldCallToVerifyNumber; }
			set { SetField(ref _shouldCallToVerifyNumber, value); }
		}
		private Boolean _shouldCallToVerifyNumber;

		public Boolean ShouldSendTextToVerifyNumber
		{
			get { return _shouldSendTextToVerifyNumber; }
			set { SetField(ref _shouldSendTextToVerifyNumber, value); }
		}
		private Boolean _shouldSendTextToVerifyNumber;

		public AccountPrivacy AccountPrivacy
		{
			get { return _accountPrivacy; }
			set { SetField(ref _accountPrivacy, value); }
		}
		private AccountPrivacy _accountPrivacy;

		public String SnapchatPhoneNumber
		{
			get { return _snapchatPhoneNumber; }
			set { SetField(ref _snapchatPhoneNumber, value); }
		}
		private String _snapchatPhoneNumber;

		public StoryPrivacy StoryPrivacy
		{
			get { return _storyPrivacy; }
			set { SetField(ref _storyPrivacy, value); }
		}
		private StoryPrivacy _storyPrivacy;

		public String Username
		{
			get { return _username; }
			set { SetField(ref _username, value); }
		}
		private String _username;

		#region Helpers



		#endregion
	}
}
