using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SnapDotNet.Core.Miscellaneous.Helpers.Storage;
using SnapDotNet.Core.Snapchat.Models;

namespace SnapDotNet.Core.Snapchat.Api
{
	public class SnapChatManager
	{
		private const string AccountDataFileName = "accountData.json";
		private const string RoamingSnapchatDataContainer = "SnapchatData";

		public String AuthToken { get; private set; }

		public String Username { get; private set; }

		public Account Account { get; private set; }

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
		}

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

		#endregion

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public bool IsAuthenticated()
		{
			return (Account != null && AuthToken != null && Account.AuthToken == AuthToken && Account.Logged);
		}

		/// <summary>
		/// 
		/// </summary>
		public async void Save()
		{
			// Seralize Account model and save as json string in Isolated Storage
			var accountData = await Task.Factory.StartNew(() => JsonConvert.SerializeObject(Account));
			IsolatedStorage.WriteFile(AccountDataFileName, accountData);

			// Save AuthToken and Username to Roaming storage
			IsolatedStorage.WriteSetting(RoamingSnapchatDataContainer, "Username", Username);
			IsolatedStorage.WriteSetting(RoamingSnapchatDataContainer, "AuthToken", AuthToken);

			// All done b
		}

		/// <summary>
		/// 
		/// </summary>
		public async void Load()
		{
			// Deseralize Account model from IsolatedStorage
			var accountData = IsolatedStorage.ReadFile(AccountDataFileName);
			if (accountData != null)
			{
				var accountDataParsed = JsonConvert.DeserializeObject<Account>(accountData);
				UpdateAccount(accountDataParsed);
			}

			// Load AuthToken and Username from Roaming storage
			UpdateUsername(IsolatedStorage.ReadSetting(RoamingSnapchatDataContainer, "Username"));
			UpdateAuthToken(IsolatedStorage.ReadSetting(RoamingSnapchatDataContainer, "AuthToken"));

			if (Account != null || AuthToken == null) return;
			try
			{
				await Endpoints.GetUpdatesAsync();
			}
			catch (Exception)
			{
				// Sign us out
				UpdateAccount(null);
				UpdateAuthToken(null);
				UpdateUsername(null);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public Endpoints Endpoints { get; private set; }
	}
}
