using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SnapDotNet.Core.Snapchat.Api.Exceptions;
using SnapDotNet.Core.Snapchat.Helpers;
using SnapDotNet.Core.Snapchat.Models;

namespace SnapDotNet.Core.Snapchat.Api
{
	public class Endpoints
	{
		private SnapChatManager _snapchatManager;
		private readonly WebConnect _webConnect;

		private const string LoginEndpointUrl = "login";

		public Endpoints(SnapChatManager snapchatManager)
		{
			_snapchatManager = snapchatManager;
			_webConnect = new WebConnect();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="username"></param>
		/// <param name="password"></param>
		/// <returns></returns>
		public async Task<Account> Login(string username, string password)
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

			return account;
		}
	}
}
