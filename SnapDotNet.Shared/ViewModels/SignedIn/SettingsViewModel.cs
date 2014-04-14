using System;
using System.Collections.ObjectModel;
using System.Linq;
using SnapDotNet.Core.Miscellaneous.Helpers;
using SnapDotNet.Core.Snapchat.Models;

namespace SnapDotNet.Apps.ViewModels.SignedIn
{
	public class SettingsViewModel
		: ViewModelBase
	{
		public SettingsViewModel()
		{
			// Privacy
			SnapPrivacyTiers = new ObservableCollection<AccountPrivacy>(Enum.GetValues(typeof(AccountPrivacy)).OfType<AccountPrivacy>().ToList());
			StoryPrivacyTiers = new ObservableCollection<StoryPrivacy>(Enum.GetValues(typeof(StoryPrivacy)).OfType<StoryPrivacy>().ToList());

			// App Specific
			AutoDownloadSnapsTiers = new ObservableCollection<SnapAutoDownloadMode>(Enum.GetValues(typeof(SnapAutoDownloadMode)).OfType<SnapAutoDownloadMode>().ToList());
		}

		#region Account

		public string Email
		{
			get { return Manager.Account.Email; }
			set
			{
				Manager.Account.Email = value;

				// fire command to save this to the server
			}
		}

		public string PhoneNumber
		{
			get { return Manager.Account.PhoneNumber; }
			set
			{
				Manager.Account.PhoneNumber = value;

				// fire command to save this to the server
			}
		}

		public int BestFriendCount
		{
			get { return Manager.Account.NumberOfBestFriends; }
			set
			{
				Manager.Account.NumberOfBestFriends = value;
				
				// fire command to save this to the server
			}
		}

		public ObservableCollection<int> BestFriendTiers
		{
			get { return _bestFriendTiers; }
			set { SetField(ref _bestFriendTiers, value); }
		}
		private ObservableCollection<int> _bestFriendTiers = new ObservableCollection<int> { 3, 5, 7 };

		#endregion

		#region Privacy

		public AccountPrivacy SnapPrivacy
		{
			get { return Manager.Account.AccountPrivacy; }
			set
			{
				Manager.Account.AccountPrivacy = value;

				// fire command to save this to the server
			}
		}

		public StoryPrivacy StoryPrivacy
		{
			get { return Manager.Account.StoryPrivacy; }
			set
			{
				Manager.Account.StoryPrivacy = value;

				// fire command to save this to the server
			}
		}

		public ObservableCollection<AccountPrivacy> SnapPrivacyTiers
		{
			get { return _snapPrivacyTiers; }
			set { SetField(ref _snapPrivacyTiers, value); }
		}
		private ObservableCollection<AccountPrivacy> _snapPrivacyTiers;

		public ObservableCollection<StoryPrivacy> StoryPrivacyTiers
		{
			get { return _storyPrivacyTiers; }
			set { SetField(ref _storyPrivacyTiers, value); }
		}
		private ObservableCollection<StoryPrivacy> _storyPrivacyTiers;

		#endregion

		#region App Specific

		public ObservableCollection<SnapAutoDownloadMode> AutoDownloadSnapsTiers
		{
			get { return _autoDownloadSnapsTiers; }
			set { SetField(ref _autoDownloadSnapsTiers, value); }
		}
		private ObservableCollection<SnapAutoDownloadMode> _autoDownloadSnapsTiers;

		#endregion
	}
}
