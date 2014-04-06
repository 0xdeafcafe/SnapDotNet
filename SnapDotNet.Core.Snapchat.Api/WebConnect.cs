using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using SnapDotNet.Core.Snapchat.Api.Helpers;

namespace SnapDotNet.Core.Snapchat.Api
{
	public class WebConnect
	{
		/// <summary>
		///     Posts data to the Snapchat API
		/// </summary>
		/// <param name="endpoint">The endpoint to point to (ie; login, logout)</param>
		/// <param name="postData">Dictionary of data to post</param>
		/// <param name="typeToken">
		///     The token to generate the req_token (StaticToken for Unauthorized Requests, AuthToken for
		///     Authorized Requests)
		/// </param>
		/// <param name="timeStamp">The retarded Snapchat Timestamp</param>
		/// <param name="headers">Optional Bonus Headers</param>
		/// <returns>Http Response Message</returns>
		public async Task<HttpResponseMessage> Post(string endpoint, Dictionary<string, string> postData,
			string typeToken, string timeStamp, Dictionary<string, string> headers = null)
		{
			var webClient = new HttpClient();
			webClient.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", Settings.UserAgent);
			webClient.DefaultRequestHeaders.TryAddWithoutValidation("Content-Length", "160");
			webClient.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "*/*");
			webClient.DefaultRequestHeaders.TryAddWithoutValidation("Accept-Encoding", "gzip,deflate");

			if (headers != null)
				foreach (var header in headers)
					webClient.DefaultRequestHeaders.TryAddWithoutValidation(header.Key, header.Value);

			postData["req_token"] = Tokens.GenerateRequestToken(typeToken, timeStamp);
			postData["version"] = "5.0.0";
			var postBody = PostBodyParser(postData);
			var response =
				await
					webClient.PostAsync(new Uri(Settings.ApiBasePoint + endpoint),
						new StringContent(postBody, Encoding.UTF8, "application/x-www-form-urlencoded"));
			return response;
		}

		/// <summary>
		///     Generates a Post Body Query String from a Dictionary of Post Data Entries
		/// </summary>
		/// <param name="postEntries">A dictionary of post data entries.</param>
		/// <returns>The post body query string</returns>
		private static string PostBodyParser(Dictionary<string, string> postEntries)
		{
			var first = true;
			var output = "";
			foreach (var postEntry in postEntries)
			{
				if (!first)
					output += string.Format("&{0}={1}", postEntry.Key, Uri.EscapeDataString(postEntry.Value));
				else
					output += string.Format("{0}={1}", postEntry.Key, Uri.EscapeDataString(postEntry.Value));

				first = false;
			}

			return output;
		}
	}
}
