using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
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

		public void UpdateAuthToken(string authToken)
		{
			AuthToken = authToken;
		}

		public void UpdateAccount(Account account)
		{
			Account = account;
		}

		public void UpdateUsername(string username)
		{
			Username = username;
		}

		public void UpdateStories(Stories stories)
		{
			Stories = stories;
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

		public async void Save()
		{
			// Seralize Account model and save as json string in Isolated Storage
			IsolatedStorage.WriteFileAsync(AccountDataFileName, await Task.Factory.StartNew(() => JsonConvert.SerializeObject(Account)));

			// Seralize Stories model and save as json string in Isolated Storage
			IsolatedStorage.WriteFileAsync(StoriesDataFileName, await Task.Factory.StartNew(() => JsonConvert.SerializeObject(Account)));

			// Save AuthToken and Username to Roaming storage
			IsolatedStorage.WriteSetting(RoamingSnapchatDataContainer, "Username", Username);
			IsolatedStorage.WriteSetting(RoamingSnapchatDataContainer, "AuthToken", AuthToken);

			// All done b
		}

		public void Load()
		{
			// Deseralize Account model from IsolatedStorage
			var accountData = IsolatedStorage.ReadFileAsync(AccountDataFileName).Result;
			if (accountData != null)
			{
				var accountDataParsed = JsonConvert.DeserializeObject<Account>(accountData);
				UpdateAccount(accountDataParsed);
			}

			// Deseralize Stories model from IsolatedStorage
			var storiesData = IsolatedStorage.ReadFileAsync(StoriesDataFileName).Result;
			if (storiesData != null)
			{
				var storiesDataParsed = JsonConvert.DeserializeObject<Stories>(storiesData);
				UpdateStories(storiesDataParsed);
			}

			// Load AuthToken and Username from Roaming storage
			UpdateUsername(IsolatedStorage.ReadSetting(RoamingSnapchatDataContainer, "Username"));
			UpdateAuthToken(IsolatedStorage.ReadSetting(RoamingSnapchatDataContainer, "AuthToken"));
		}

		public async Task UpdateAllAsync()
		{
			await Endpoints.GetUpdatesAsync();
			await Endpoints.GetStoriesAsync();
		}

		#endregion
	}
}
