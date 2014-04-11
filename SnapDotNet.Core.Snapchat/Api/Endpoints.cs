using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SnapDotNet.Core.Miscellaneous.Crypto;
using SnapDotNet.Core.Miscellaneous.CustomTypes;
using SnapDotNet.Core.Snapchat.Api.Exceptions;
using SnapDotNet.Core.Snapchat.Helpers;
using SnapDotNet.Core.Snapchat.Models;

namespace SnapDotNet.Core.Snapchat.Api
{
	public class Endpoints
	{
		private readonly SnapChatManager _snapchatManager;
		private readonly WebConnect _webConnect;
		
		private const string FriendEndpointUrl =		"friend";
		private const string LoginEndpointUrl =			"login";
		private const string LogoutEndpointUrl =		"logout";
		private const string StoriesEndpointUrl =		"stories";
		private const string SnapBlobEndpointUrl =		"blob";
		private const string UpdatesEndpointUrl =		"updates";
		private const string BestsEndpointUrl =			"bests";

		/// <summary>
		/// 
		/// </summary>
		/// <param name="snapchatManager"></param>
		public Endpoints(SnapChatManager snapchatManager)
		{
			_snapchatManager = snapchatManager;
			_webConnect = new WebConnect();
		}

		#region Authenticate

		/// <summary>
		/// 
		/// </summary>
		/// <param name="username"></param>
		/// <param name="password"></param>
		/// <returns></returns>
		public async Task<Account> AuthenticateAsync(string username, string password)
		{
			var timestamp = Timestamps.GenerateRetardedTimestamp();
			var postData = new Dictionary<string, string>
			{
				{"password", password},
				{"username", username},
				{"timestamp", timestamp.ToString(CultureInfo.InvariantCulture)}
			};

			var account =
				await
					_webConnect.PostToGenericAsync<Account>(LoginEndpointUrl, postData, Settings.StaticToken,
						timestamp.ToString(CultureInfo.InvariantCulture));

			if (account == null || !account.Logged)
				throw new InvalidCredentialsException();

			_snapchatManager.UpdateAccount(account);
			_snapchatManager.UpdateAuthToken(account.AuthToken);
			_snapchatManager.UpdateUsername(account.Username);
			_snapchatManager.Save();

			return account;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="username"></param>
		/// <param name="password"></param>
		/// <returns></returns>
		public Account Authenticate(string username, string password)
		{
			return AuthenticateAsync(username, password).Result;
		}

		#endregion

		#region Logout

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public async Task<Response> LogoutAsync()
		{
			await _snapchatManager.DeleteAsync();

			var timestamp = Timestamps.GenerateRetardedTimestamp();
			var postData = new Dictionary<string, string>
			{
				{"username", GetAuthedUsername()},
				{"timestamp", timestamp.ToString(CultureInfo.InvariantCulture)},
				{"json", "{}"}
			};

			var response =
				await
					_webConnect.PostToGenericAsync<Response>(LogoutEndpointUrl, postData, _snapchatManager.AuthToken,
						timestamp.ToString(CultureInfo.InvariantCulture));

			return response;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public Response Logout()
		{
			return LogoutAsync().Result;
		}

		#endregion

		#region Updates

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public async Task<Account> GetUpdatesAsync()
		{
			var timestamp = Timestamps.GenerateRetardedTimestamp();
			var postData = new Dictionary<string, string>
			{
				{"username", GetAuthedUsername()},
				{"timestamp", timestamp.ToString(CultureInfo.InvariantCulture)}
			};

			var account =
				await
					_webConnect.PostToGenericAsync<Account>(UpdatesEndpointUrl, postData, _snapchatManager.AuthToken,
						timestamp.ToString(CultureInfo.InvariantCulture));

			if (account == null || !account.Logged)
				throw new InvalidCredentialsException();

			_snapchatManager.UpdateAccount(account);
			_snapchatManager.UpdateAuthToken(account.AuthToken);
			_snapchatManager.UpdateUsername(account.Username);
			_snapchatManager.Save();

			return account;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public Account GetUpdates()
		{
			return GetUpdatesAsync().Result;
		}

		#endregion

		#region Snap

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public async Task<byte[]> GetSnapBlobAsync(Snap snap)
		{
			return await GetSnapBlobAsync(snap.Id);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public byte[] GetSnapBlob(Snap snap)
		{
			return GetSnapBlobAsync(snap).Result;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public async Task<byte[]> GetSnapBlobAsync(string snapId)
		{
			var timestamp = Timestamps.GenerateRetardedTimestamp();
			var postData = new Dictionary<string, string>
			{
				{"username", GetAuthedUsername()},
				{"timestamp", timestamp.ToString(CultureInfo.InvariantCulture)},
				{"id", snapId}
			};

			var data =
				await
					_webConnect.PostToByteArrayAsync(SnapBlobEndpointUrl, postData, _snapchatManager.AuthToken,
						timestamp.ToString(CultureInfo.InvariantCulture));

			var decryptedData = Aes.DecryptData(data, Convert.FromBase64String(Settings.BlobEncryptionKey));
			if (Blob.ValidateMediaBlob(decryptedData))
				return decryptedData;

			return data;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public byte[] GetSnapBlob(string snapId)
		{
			return GetSnapBlobAsync(snapId).Result;
		}

		#endregion

		#region Blob

		/// <summary>
		/// 
		/// </summary>
		/// <param name="blobPath"></param>
		/// <param name="key"></param>
		/// <param name="iv"></param>
		/// <param name="zipped"></param>
		/// <returns></returns>
		public byte[] GetStoryBlob(Uri blobPath, string key, string iv, bool zipped = false)
		{
			var data = _webConnect.GetBytes(blobPath);
			if (data == null) return null;

			var decryptedData = Aes.DecryptDataWithIv(data, Convert.FromBase64String(key), Convert.FromBase64String(iv));
			return decryptedData;
		}

		#endregion

		#region Stories

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public async Task<Stories> GetStoriesAsync()
		{
			var timestamp = Timestamps.GenerateRetardedTimestamp();
			var postData = new Dictionary<string, string>
			{
				{"username", GetAuthedUsername()},
				{"timestamp", timestamp.ToString(CultureInfo.InvariantCulture)}
			};

			var stories =
				await
					_webConnect.PostToGenericAsync<Stories>(StoriesEndpointUrl, postData, _snapchatManager.AuthToken,
						timestamp.ToString(CultureInfo.InvariantCulture));

			_snapchatManager.UpdateStories(stories);

			return stories;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public Stories GetStories()
		{
			return GetStoriesAsync().Result;
		}

		#endregion

		#region Friend Actions

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public async Task<Response> SendFriendActionAsync(string friendUsername, FriendAction action)
		{
			var timestamp = Timestamps.GenerateRetardedTimestamp();
			var postData = new Dictionary<string, string>
			{
				{"username", GetAuthedUsername()},
				{"timestamp", timestamp.ToString(CultureInfo.InvariantCulture)},
				{"action", action.ToString().ToLower()},
				{"friend", friendUsername},
			};

			var response =
				await
					_webConnect.PostToGenericAsync<Response>(FriendEndpointUrl, postData, _snapchatManager.AuthToken,
						timestamp.ToString(CultureInfo.InvariantCulture));

			return response;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public Response SendFriendAction(string friendUsername, FriendAction action)
		{
			return SendFriendActionAsync(friendUsername, action).Result;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public async Task<Response> ChangeFriendDisplayNameAsync(string friendUsername, string newDisplayName)
		{
			var timestamp = Timestamps.GenerateRetardedTimestamp();
			var postData = new Dictionary<string, string>
			{
				{"username", GetAuthedUsername()},
				{"timestamp", timestamp.ToString(CultureInfo.InvariantCulture)},
				{"action", "display"},
				{"friend", friendUsername},
				{"display", newDisplayName}
			};

			var response =
				await
					_webConnect.PostToGenericAsync<Response>(FriendEndpointUrl, postData, _snapchatManager.AuthToken,
						timestamp.ToString(CultureInfo.InvariantCulture));

			return response;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public Response ChangeFriendDisplayName(string friendUsername, string newDisplayName)
		{
			return ChangeFriendDisplayNameAsync(friendUsername, newDisplayName).Result;
		}

		#endregion

		#region Public Activity

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public async Task<ObservableDictionary<string, PublicActivity>> GetPublicActivityAsync(string[] requestedUsernames = null)
		{
			if (requestedUsernames == null)
				requestedUsernames = _snapchatManager.Account.Friends.Select(friend => friend.Name).ToArray();

			var timestamp = Timestamps.GenerateRetardedTimestamp();
			var postData = new Dictionary<string, string>
			{
				{"username", GetAuthedUsername()},
				{"friend_usernames", JsonConvert.SerializeObject(requestedUsernames)},
				{"timestamp", timestamp.ToString(CultureInfo.InvariantCulture)}
			};

			var publicActivities =
				await
					_webConnect.PostToGenericAsync<ObservableDictionary<string, PublicActivity>>(BestsEndpointUrl, postData, _snapchatManager.AuthToken,
						timestamp.ToString(CultureInfo.InvariantCulture));

			_snapchatManager.UpdatePublicActivities(publicActivities);
			_snapchatManager.Save();

			return publicActivities;
		}

		#endregion

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		private string GetAuthedUsername()
		{
			if (_snapchatManager.Account != null)
				return _snapchatManager.Account.Username;
			if (_snapchatManager.Username != null)
				return _snapchatManager.Username;
			
			throw new InvalidCredentialsException("There is no username set in the Snapchat Manager.");
		}
	}
}
