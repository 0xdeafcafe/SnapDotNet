using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SnapDotNet.Core.Snapchat.Api.Exceptions;
using SnapDotNet.Core.Snapchat.Api.Helpers;
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

		public async Task<Account> Login(string username, string password)
		{
			var timestamp = Timestamps.GenerateRetardedTimestamp();
			var postData = new Dictionary<string, string>
			{
				{"password", password},
				{"username", username},
				{"timestamp", timestamp.ToString(CultureInfo.InvariantCulture)}
			};
			var response =
				await _webConnect.Post(LoginEndpointUrl, postData, Settings.StaticToken, timestamp.ToString(CultureInfo.InvariantCulture));

			switch (response.StatusCode)
			{
				case HttpStatusCode.OK:
				{
					// Http Request Worked
					var data = await response.Content.ReadAsStringAsync();
					var accountData = await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<Account>(data));

					// Check if we were logged in
					if (!accountData.Logged)
						throw new InvalidCredentialsException();

					// Yup, save the data and return true
					return accountData;
				}
				default:
					// Well, fuck
					throw new InvalidHttpResponseException("Unable to connect to SnapChat's services.", response);
			}
		}
	}
}
