using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SnapDotNet.Core.Miscellaneous.Helpers.Storage;
using SnapDotNet.Core.Miscellaneous.Models;
using SnapDotNet.Core.Snapchat.Models.New;

namespace SnapDotNet.Core.Snapchat.Api
{
	public sealed class SnapchatManager
		: NotifyPropertyChangedBase
	{
		private const string AccountDataFileName = "update.json";
		private const string RoamingSnapchatDataContainer = "SnapchatData";

		public AllUpdatesResponse AllUpdates
		{
			get { return _allUpdates; }
			set { SetField(ref _allUpdates, value); }
		}
		private AllUpdatesResponse _allUpdates;

		public String AuthToken
		{
			get
			{
				if (AllUpdates != null && AllUpdates.UpdatesResponse != null)
					return AllUpdates.UpdatesResponse.AuthToken;
				
				return null;
			}
		}

		public StoriesResponse Stories
		{
			get { return AllUpdates.StoriesResponse; }
		}

		public Endpoints Endpoints
		{
			get { return _endpoints; }
			set { SetField(ref _endpoints, value); }
		}
		private Endpoints _endpoints;

		public bool Loaded { get; private set; }

		#region Constructors

		/// <summary>
		/// 
		/// </summary>
		public SnapchatManager()
		{
			Endpoints = new Endpoints(this);

			//Load();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="username"></param>
		/// <param name="authToken"></param>
		/// <param name="getUpdates"></param>
		public SnapchatManager(string username, string authToken, bool getUpdates = false)
		{
			Endpoints = new Endpoints(this);

			UpdateUsername(username);
			UpdateAuthToken(authToken);

			if (!getUpdates) return;

			Endpoints.GetAllUpdatesAsync().Wait();
		}

		#endregion


		#region Setters

		public async Task<bool> Logout()
		{
			try
			{
				AllUpdates = null;
				await DeleteAsync();
				await Endpoints.LogoutAsync();
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}

		public void UpdateAuthToken(string authToken)
		{
			if (AllUpdates == null)
				AllUpdates = new AllUpdatesResponse
				{
					ConversationResponse = new ObservableCollection<ConversationResponse>(),
					MessagingGatewayInfo = new MessagingGatewayInfo(),
					StoriesResponse = new StoriesResponse(),
					UpdatesResponse = new UpdatesResponse()
				};

			AllUpdates.UpdatesResponse.AuthToken = authToken;
		}

		public void Update(AllUpdatesResponse allUpdates)
		{
			AllUpdates = allUpdates;

			//// we got some house keeping to do
			//foreach (var newSnap in account.Snaps)
			//{
			//	var found = false;
			//	foreach (var oldSnap in Account.Snaps.Where(oldSnap => newSnap.Id == oldSnap.Id))
			//	{
			//		found = true;
			//		if (newSnap.RecipientName == Account.Username || newSnap.RecipientName == null)
			//			break;

			//		// replace, instead
			//		var position = Account.Snaps.IndexOf(oldSnap);
			//		if (position == -1) break;
			//		Account.Snaps.RemoveAt(position);
			//		Account.Snaps.Insert(position, newSnap);
			//		break;
			//	}

			//	if (!found)
			//		Account.Snaps.Insert(0, newSnap);
			//}

			//Account.Snaps = new ObservableCollection<Snap>(Account.Snaps.OrderByDescending(s => s.SentTimestamp).Take(50));
			//account.Snaps = Account.Snaps;
			//Account = account;
		}

		public void UpdateUsername(string username)
		{
			if (AllUpdates == null)
				AllUpdates = new AllUpdatesResponse
				{
					ConversationResponse = new ObservableCollection<ConversationResponse>(),
					MessagingGatewayInfo = new MessagingGatewayInfo(),
					StoriesResponse = new StoriesResponse(),
					UpdatesResponse = new UpdatesResponse()
				};

			AllUpdates.UpdatesResponse.Username = username;
		}

		#endregion


		#region Cool Functions

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public bool IsAuthenticated()
		{
			return (AllUpdates != null && AllUpdates.UpdatesResponse != null && AllUpdates.UpdatesResponse.Logged);
		}

		#endregion


		#region Actions

		#region Actions:Save

		public async void Save()
		{
			await SaveAccountDataAsync();
			
			// All done b
		}

		public async Task SaveAccountDataAsync()
		{
			// Seralize the Account model and save as json string in Isolated Storage
			await IsolatedStorage.WriteFileAsync(AccountDataFileName, await Task.Factory.StartNew(() => JsonConvert.SerializeObject(AllUpdates)));
		}

		#endregion

		public async Task DeleteAsync()
		{
			await IsolatedStorage.DeleteFileAsync(AccountDataFileName);
			IsolatedStorage.DeleteRoamingSetting(RoamingSnapchatDataContainer, "Username");
			IsolatedStorage.DeleteRoamingSetting(RoamingSnapchatDataContainer, "AuthToken");
		}

		public async Task LoadAsync()
		{
			// Deseralize the Account model from IsolatedStorage
			var accountData = await IsolatedStorage.ReadFileAsync(AccountDataFileName);
			if (accountData != null && !String.IsNullOrEmpty(accountData) && accountData != "null")
			{
				try
				{
					var accountDataParsed = JsonConvert.DeserializeObject<AllUpdatesResponse>(accountData);
					Update(accountDataParsed);
				}
				catch (Exception exception)
				{
					Debug.WriteLine(exception.ToString());
				}
			}

			Loaded = true;
		}

		public async Task UpdateAllAsync(Action hidependingUiAction)
		{
			await Endpoints.GetAllUpdatesAsync();
			hidependingUiAction();
		}

		public async Task DownloadSnapsAsync()
		{
			// Note: Auto-download settings is moved to the app

			//foreach (var conversation in AllUpdates.ConversationResponse)
			//	foreach (var snap in conversation.ConversationMessages.Messages.Where(m => m.Snap != null))
			//		await snap.DownloadSnapBlobAsync(this);

			// yea son
			await SaveAccountDataAsync();
		}

		#endregion
	}
}
