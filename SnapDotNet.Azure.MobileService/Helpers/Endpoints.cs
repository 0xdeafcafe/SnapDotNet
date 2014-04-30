using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;
using SnapDotNet.Azure.MobileService.Models;

namespace SnapDotNet.Azure.MobileService.Helpers
{
	public class Endpoints
	{
		private readonly WebConnect _webConnect;
		private const string UpdatesEndpointUrl = "updates";

		public Endpoints()
		{
			_webConnect = new WebConnect();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public async Task<Tuple<HttpResponseMessage, Account>> GetUpdatesAsync(string username, string authToken)
		{
			var timestamp = Timestamps.GenerateRetardedTimestamp();
			var postData = new Dictionary<string, string>
			{
				{"username", username},
				{"timestamp", timestamp.ToString(CultureInfo.InvariantCulture)}
			};

			return
				await
					_webConnect.PostAsync(UpdatesEndpointUrl, postData, authToken,
						timestamp.ToString(CultureInfo.InvariantCulture));
		}
	}
}
