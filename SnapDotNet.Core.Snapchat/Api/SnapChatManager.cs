using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SnapDotNet.Core.Miscellaneous.CustomTypes;
using SnapDotNet.Core.Miscellaneous.Helpers;
using SnapDotNet.Core.Miscellaneous.Helpers.Storage;
using SnapDotNet.Core.Miscellaneous.Models;
using SnapDotNet.Core.Snapchat.Models;

namespace SnapDotNet.Core.Snapchat.Api
{
	public class SnapChatManager
		: NotifyPropertyChangedBase
	{
		private const string AccountDataFileName = "accountData.json";
		private const string StoriesDataFileName = "storiesData.json";
		private const string PublicActivityDataFileName = "publicActivityData.json";
		private const string RoamingSnapchatDataContainer = "SnapchatData";


		public String AuthToken
		{
			get { return _authToken; }
			set { SetField(ref _authToken, value); }
		}
		private String _authToken;

		public String Username
		{
			get { return _username; }
			set { SetField(ref _username, value); }
		}
		private String _username;

		public Account Account
		{
			get { return _account; }
			set { SetField(ref _account, value); }
		}
		private Account _account;

		public ObservableDictionary<string, PublicActivity> PublicActivities
		{
			get { return _publicActivities; }
			set { SetField(ref _publicActivities, value); }
		}
		private ObservableDictionary<string, PublicActivity> _publicActivities;

		public Stories Stories
		{
			get { return _stories; }
			set { SetField(ref _stories, value); }
		}
		private Stories _stories;

		public Endpoints Endpoints
		{
			get { return _endpoints; }
			set { SetField(ref _endpoints, value); }
		}
		private Endpoints _endpoints;


		#region Constructors

		/// <summary>
		/// 
		/// </summary>
		public SnapChatManager()
		{
			Endpoints = new Endpoints(this);

			Load();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="username"></param>
		/// <param name="authToken"></param>
		/// <param name="getUpdates"></param>
		public SnapChatManager(string username, string authToken, bool getUpdates = false)
		{
			Endpoints = new Endpoints(this);

			UpdateUsername(username);
			UpdateAuthToken(authToken);

			if (!getUpdates) return;

			Endpoints.GetUpdatesAsync().Wait();
			Endpoints.GetStoriesAsync().Wait();
		}

		#endregion


		#region Setters

		public async Task<bool> Logout()
		{
			try
			{
				var response = await Endpoints.LogoutAsync();
				if (response != null) throw new Exception();

				Account = null;
				AuthToken = null;
				Username = null;
				Stories = null;
				PublicActivities = null;
				await DeleteAsync();

				return true;
			}
			catch
			{
				return false;
			}
		}

		public void UpdateAuthToken(string authToken)
		{
			AuthToken = authToken;
		}

		public void UpdateAccount(Account account)
		{
			if (Account == null)
			{
				Account = account;
				return;
			}

			// we got some house keeping to do
			foreach (var newSnap in account.Snaps)
			{
				var found = false;
				foreach (var oldSnap in Account.Snaps.Where(oldSnap => newSnap.Id == oldSnap.Id))
				{
					found = true;
					if (newSnap.RecipientName == Account.Username || newSnap.RecipientName == null)
						break;

					// replace, instead
					var position = Account.Snaps.IndexOf(oldSnap);
					if (position == -1) break;
					Account.Snaps.RemoveAt(position);
					Account.Snaps.Insert(position, newSnap);
					break;
				}

				if (!found)
					Account.Snaps.Insert(0, newSnap);
			}

			Account.Snaps = new ObservableCollection<Snap>(Account.Snaps.OrderByDescending(s => s.SentTimestamp).Take(50));
			account.Snaps = Account.Snaps;
			Account = account;
		}

		public void UpdateUsername(string username)
		{
			Username = username;
		}

		public void UpdateStories(Stories stories)
		{
			if (Stories == null)
			{
				Stories = stories;
				return;
			}

			// we got some house keeping to do
			var downloadingFriendsStories = (from friendStory in Stories.FriendStories from story in friendStory.Stories where story.Story.IsDownloading select story.Story).ToList();
			var downloadingMyStories = (from story in Stories.MyStories where story.Story.IsDownloading select story.Story).ToList();

			foreach (var story in stories.FriendStories.SelectMany(friendStory => friendStory.Stories))
				foreach (var downloadingStory in downloadingFriendsStories.Where(downloadingStory => downloadingStory.Id == story.Story.Id))
				{
					story.Story.IsDownloading = true;
					break;
				}

			foreach (var story in stories.MyStories)
				foreach (var downloadingStory in downloadingMyStories.Where(downloadingStory => downloadingStory.Id == story.Story.Id))
				{
					story.Story.IsDownloading = true;
					break;
				}

			Stories = stories;
		}

		public void UpdatePublicActivities(ObservableDictionary<string, PublicActivity> publicActivities)
		{
			PublicActivities = publicActivities;
		}

		#endregion


		#region Cool Functions

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public bool IsAuthenticated()
		{
			return (Account != null && AuthToken != null && Account.AuthToken == AuthToken && Account.Logged);
		}

		#endregion


		#region Actions

		#region Actions:Save

		public async void Save()
		{
			SaveAccountData();

			// Seralize the Stories model and save as json string in Isolated Storage
			IsolatedStorage.WriteFileAsync(StoriesDataFileName, await Task.Factory.StartNew(() => JsonConvert.SerializeObject(Stories)));

			// Seralize the PublicActivies model and save as json string in Isolated Storage
			IsolatedStorage.WriteFileAsync(PublicActivityDataFileName, await Task.Factory.StartNew(() => JsonConvert.SerializeObject(PublicActivities)));

			// Save AuthToken and Username to Roaming storage
			IsolatedStorage.WriteRoamingSetting(RoamingSnapchatDataContainer, "Username", Username);
			IsolatedStorage.WriteRoamingSetting(RoamingSnapchatDataContainer, "AuthToken", AuthToken);

			// All done b
		}

		public async void SaveAccountData()
		{
			// Seralize the Account model and save as json string in Isolated Storage
			IsolatedStorage.WriteFileAsync(AccountDataFileName, await Task.Factory.StartNew(() => JsonConvert.SerializeObject(Account)));
		}

		#endregion

		public async Task DeleteAsync()
		{
			await IsolatedStorage.DeleteFileAsync(AccountDataFileName);
			await IsolatedStorage.DeleteFileAsync(StoriesDataFileName);
			await IsolatedStorage.DeleteFileAsync(PublicActivityDataFileName);
			IsolatedStorage.DeleteRoamingSetting(RoamingSnapchatDataContainer, "Username");
			IsolatedStorage.DeleteRoamingSetting(RoamingSnapchatDataContainer, "AuthToken");
		}

		public void Load()
		{
			// Deseralize the Account model from IsolatedStorage
			var accountData = IsolatedStorage.ReadFileAsync(AccountDataFileName).Result;
			if (accountData != null && accountData != "null")
			{
				try
				{
					var accountDataParsed = JsonConvert.DeserializeObject<Account>(accountData);
					UpdateAccount(accountDataParsed);
				}
				catch (Exception exception)
				{
					Debug.WriteLine(exception.ToString());
				}
			}

			// Deseralize the Stories model from IsolatedStorage
			var storiesData = IsolatedStorage.ReadFileAsync(StoriesDataFileName).Result;
			if (storiesData != null && storiesData != "null")
			{
				try 
				{
					var storiesDataParsed = JsonConvert.DeserializeObject<Stories>(storiesData);
					UpdateStories(storiesDataParsed);
				}
				catch (Exception exception)
				{
					Debug.WriteLine(exception.ToString());
				}
			}

			// Deseralize the PublicActivity model from IsolatedStorage
			var publicActiviesData = IsolatedStorage.ReadFileAsync(PublicActivityDataFileName).Result;
			if (publicActiviesData != null && publicActiviesData != "null")
			{
				try
				{
					var publicActiviesDataParsed =
						JsonConvert.DeserializeObject<ObservableDictionary<string, PublicActivity>>(storiesData);
					UpdatePublicActivities(publicActiviesDataParsed);
				}
				catch (Exception exception)
				{
					Debug.WriteLine(exception.ToString());
				}
			}

			// Load AuthToken and Username from Roaming storage
			UpdateUsername(IsolatedStorage.ReadRoamingSetting(RoamingSnapchatDataContainer, "Username"));
			UpdateAuthToken(IsolatedStorage.ReadRoamingSetting(RoamingSnapchatDataContainer, "AuthToken"));
		}

		public async Task UpdateAllAsync(Action hidependingUiAction, ApplicationSettings settings)
		{
			await Endpoints.GetUpdatesAsync();
			await Endpoints.GetStoriesAsync();
			await Endpoints.GetPublicActivityAsync();
			hidependingUiAction();
			await DownloadSnaps(settings);
		}

		private async Task DownloadSnaps(ApplicationSettings settings)
		{
			// Never check
			if (settings.SnapAutoDownloadMode == SnapAutoDownloadMode.Never) return;

			// No Cellular check
			if (settings.SnapAutoDownloadMode == SnapAutoDownloadMode.Cellular &&
				!NetworkInformationHelper.OnCellularDataConnection()) return;

			// No Wifi check
			if (settings.SnapAutoDownloadMode == SnapAutoDownloadMode.Wifi &&
				!NetworkInformationHelper.OnWifiConnection()) return;

			foreach (var snap in Account.Snaps)
				await snap.DownloadSnapBlobAsync(this);

			// yea son
			SaveAccountData();
		}

		#endregion
	}
}
