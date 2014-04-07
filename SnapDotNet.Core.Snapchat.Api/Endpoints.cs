using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using SnapDotNet.Core.Snapchat.Api.Exceptions;
using SnapDotNet.Core.Snapchat.Helpers;
using SnapDotNet.Core.Snapchat.Models;

namespace SnapDotNet.Core.Snapchat.Api
{
	public class Endpoints
	{
		private readonly SnapChatManager _snapchatManager;
		private readonly WebConnect _webConnect;

		private const string LoginEndpointUrl =			"login";
		private const string UpdatesEndpointUrl =		"updates";
        private const string StoriesEndpointUrl =		"stories";

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
					_webConnect.Post<Account>(LoginEndpointUrl, postData, Settings.StaticToken,
						timestamp.ToString(CultureInfo.InvariantCulture));

			if (account == null || !account.Logged)
				throw new InvalidCredentialsException();

			_snapchatManager.UpdateAccount(account);
			_snapchatManager.UpdateAccessCode(account.AuthToken);
			_snapchatManager.UpdateUsername(account.Username);

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

		#region Updates

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public async Task<Account> GetUpdatesAsync()
		{
			string username;
			if (_snapchatManager.Account != null)
				username = _snapchatManager.Account.Username;
			else if (_snapchatManager.Username != null)
				username = _snapchatManager.Username;
			else
				throw new InvalidCredentialsException("There is no username set in the Snapchat Manager.");

			var timestamp = Timestamps.GenerateRetardedTimestamp();
			var postData = new Dictionary<string, string>
			{
				{"username", username},
				{"timestamp", timestamp.ToString(CultureInfo.InvariantCulture)}
			};

			var account =
				await
					_webConnect.Post<Account>(UpdatesEndpointUrl, postData, _snapchatManager.AuthToken,
						timestamp.ToString(CultureInfo.InvariantCulture));

			if (account == null || !account.Logged)
				throw new InvalidCredentialsException();

			_snapchatManager.UpdateAccount(account);
			_snapchatManager.UpdateUsername(account.Username);

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

        #region Stories

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<Stories> GetStoriesAsync()
        {
            string username;
            if (_snapchatManager.Account != null)
                username = _snapchatManager.Account.Username;
            else if (_snapchatManager.Username != null)
                username = _snapchatManager.Username;
            else
                throw new InvalidCredentialsException("There is no username set in the Snapchat Manager.");

            var timestamp = Timestamps.GenerateRetardedTimestamp();
            var postData = new Dictionary<string, string>
			{
				{"username", username},
				{"timestamp", timestamp.ToString(CultureInfo.InvariantCulture)}
			};

            var stories =
                await
                    _webConnect.Post<Stories>(StoriesEndpointUrl, postData, _snapchatManager.AuthToken,
                        timestamp.ToString(CultureInfo.InvariantCulture));

            if (stories == null)
                throw new InvalidCredentialsException();

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
    }
}
