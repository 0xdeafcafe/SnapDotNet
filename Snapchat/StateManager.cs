using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;

namespace SnapDotNet
{
	public sealed class StateManager
	{
		private const string AccountStateFileName = "account";

		/// <summary>
		/// Gets the <see cref="StateManager"/> for the local app folder.
		/// </summary>
		public static readonly StateManager Local = new StateManager(ApplicationData.Current.LocalFolder);

		/// <summary>
		/// Initializes a new instance of the <see cref="StateManager"/> class that manages state
		/// information stored at the specified <paramref name="location"/>.
		/// </summary>
		/// <param name="location">
		/// The folder where the state managed by this instance is stored.
		/// </param>
		public StateManager(StorageFolder location)
		{
			Location = location;
		}

		/// <summary>
		/// Gets the folder where state managed by this instance is stored.
		/// </summary>
		public StorageFolder Location { get; private set; }

		/// <summary>
		/// Checks whether the account state is stored in the local app data store. This method can
		/// be used to quickly determine (without verification) whether the app user is logged in.
		/// </summary>
		/// <returns>
		/// <c>true</c> if account state is found in the folder where the state managed by this
		/// instance is stored, otherwise <c>false</c>.
		/// </returns>
		public async Task<bool> ContainsStoredAccountStateAsync()
		{
			try
			{
				await Location.GetFileAsync(AccountStateFileName);
				return true;
			}
			catch (FileNotFoundException)
			{
				return false;
			}
		}

		/// <summary>
		/// Serializes an <see cref="Account"/> to the folder where the state managed by this instance
		/// is stored.
		/// </summary>
		/// <param name="account">The account to serialize.</param>
		public async Task SaveAccountStateAsync(Account account)
		{
			Debug.WriteLine("[State Manager] Saving account state to {0}", Location.Path);
			string jsonData = await Task.Factory.StartNew(() => JsonConvert.SerializeObject(account));
			var file = await Location.CreateFileAsync(AccountStateFileName, CreationCollisionOption.ReplaceExisting);
			await FileIO.WriteTextAsync(file, jsonData, UnicodeEncoding.Utf8);
		}
		
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public async Task<Account> LoadAccountStateAsync()
		{
			try
			{
				Debug.WriteLine("[State Manager] Loading account state from {0}", Location.Path);
				var file = await Location.GetFileAsync(AccountStateFileName);
				var jsonData = await FileIO.ReadTextAsync(file, UnicodeEncoding.Utf8);
				var account = await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<Account>(jsonData));

				if (account.SortedFriends == null || (account.Friends.Count > 0 && account.SortedFriends.Count == 0))
					account.CreateSortedFriends();

				// TODO: initalize async updating

				return account;
			}
			catch (FileNotFoundException)
			{
				return null;
			}
		}
	}
}
