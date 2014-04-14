using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SnapDotNet.Core.Miscellaneous.Helpers.Compression;
using SnapDotNet.Core.Snapchat.Api.Exceptions;
using SnapDotNet.Core.Snapchat.Helpers;

namespace SnapDotNet.Core.Snapchat.Api
{
	public class WebConnect
	{
		#region Post

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
		public async Task<T> PostToGenericAsync<T>(string endpoint, Dictionary<string, string> postData,
			string typeToken, string timeStamp, Dictionary<string, string> headers = null)
		{
			var response = await PostAsync(endpoint, postData, typeToken, timeStamp, headers);

			// Do GZip
			string data;
			if (response.Content.Headers.ContentEncoding.Contains("gzip"))
				data = Gzip.DecompressToString(await response.Content.ReadAsByteArrayAsync());
			else
				data = await response.Content.ReadAsStringAsync();

			// Http Request Worked
			var deseralizedData = await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<T>(data));
			return deseralizedData;
		}

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
		public async Task<string> PostToStringAsync(string endpoint, Dictionary<string, string> postData,
			string typeToken, string timeStamp, Dictionary<string, string> headers = null)
		{
			var response = await PostAsync(endpoint, postData, typeToken, timeStamp, headers);

			// Do GZip
			string data;
			if (response.Content.Headers.ContentEncoding.Contains("gzip"))
				data = Gzip.DecompressToString(await response.Content.ReadAsByteArrayAsync());
			else
				data = await response.Content.ReadAsStringAsync();

			// Http Request Worked
			return data;
		}

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
		public async Task<HttpResponseMessage> PostToResponseAsync(string endpoint, Dictionary<string, string> postData,
			string typeToken, string timeStamp, Dictionary<string, string> headers = null)
		{
			return await PostAsync(endpoint, postData, typeToken, timeStamp, headers);
		}

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
		public async Task<byte[]> PostToByteArrayAsync(string endpoint, Dictionary<string, string> postData,
			string typeToken, string timeStamp, Dictionary<string, string> headers = null)
		{
			var response = await PostAsync(endpoint, postData, typeToken, timeStamp, headers);

			// Do GZip
			byte[] data;
			if (response.Content.Headers.ContentEncoding.Contains("gzip"))
				data = Gzip.Decompress(await response.Content.ReadAsByteArrayAsync());
			else
				data = await response.Content.ReadAsByteArrayAsync();

			// Http Request Worked
			return data;
		}

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
		private static async Task<HttpResponseMessage> PostAsync(string endpoint, Dictionary<string, string> postData,
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

			postData["req_token"] = Tokens.GenerateRequestToken(Settings.Secret, Settings.HashingPattern, typeToken, timeStamp);
			var postBody = PostBodyParser(postData);
			var response =
				await
					webClient.PostAsync(new Uri(Settings.ApiBasePoint + endpoint),
						new StringContent(postBody, Encoding.UTF8, "application/x-www-form-urlencoded"));

			switch (response.StatusCode)
			{
				case HttpStatusCode.OK:
					return response;

				default:
					// Well, fuck
					throw new InvalidHttpResponseException(response.ReasonPhrase, response);
			}
		}

		#endregion

		#region Get

		/// <summary>
		///     Gets data from the Snapchat API
		/// </summary>
		/// <param name="endpoint">The endpoint to point to (ie; login, logout)</param>
		/// <param name="headers">Optional Bonus Headers</param>
		/// <returns>Http Response Message</returns>
		public async Task<byte[]> GetBytesAsync(string endpoint, Dictionary<string, string> headers = null)
		{
			return await GetBytesAsync(new Uri(Settings.ApiBasePoint + endpoint), headers);
		}

		/// <summary>
		///     Gets data from the Snapchat API
		/// </summary>
		/// <param name="uri">The full URL to connect to</param>
		/// <param name="headers">Optional Bonus Headers</param>
		/// <returns>Http Response Message</returns>
		public async Task<byte[]> GetBytesAsync(Uri uri, Dictionary<string, string> headers = null)
		{
			var webClient = new HttpClient();
			webClient.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", Settings.UserAgent);
			webClient.DefaultRequestHeaders.TryAddWithoutValidation("Content-Length", "160");
			webClient.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "*/*");
			webClient.DefaultRequestHeaders.TryAddWithoutValidation("Accept-Encoding", "gzip,deflate");
			webClient.DefaultRequestHeaders.TryAddWithoutValidation("version", "5.0.0");

			if (headers != null)
				foreach (var header in headers)
					webClient.DefaultRequestHeaders.TryAddWithoutValidation(header.Key, header.Value);

			var response = await webClient.GetAsync(uri);

			switch (response.StatusCode)
			{
				case HttpStatusCode.OK:
					return response.Content.Headers.ContentEncoding.Contains("gzip")
						? Gzip.Decompress(await response.Content.ReadAsByteArrayAsync())
						: await response.Content.ReadAsByteArrayAsync();

				default:
					// Well, fuck
					throw new InvalidHttpResponseException(response.ReasonPhrase, response);
			}
		}

		/// <summary>
		///     Gets data from the Snapchat API
		/// </summary>
		/// <param name="uri">The full URL to connect to</param>
		/// <param name="headers">Optional Bonus Headers</param>
		/// <returns>Http Response Message</returns>
		public byte[] GetBytes(Uri uri, Dictionary<string, string> headers = null)
		{
			var webClient = new HttpClient();
			webClient.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", Settings.UserAgent);
			webClient.DefaultRequestHeaders.TryAddWithoutValidation("Content-Length", "160");
			webClient.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "*/*");
			webClient.DefaultRequestHeaders.TryAddWithoutValidation("Accept-Encoding", "gzip,deflate");
			webClient.DefaultRequestHeaders.TryAddWithoutValidation("version", "5.0.0");

			if (headers != null)
				foreach (var header in headers)
					webClient.DefaultRequestHeaders.TryAddWithoutValidation(header.Key, header.Value);

			var response = webClient.GetAsync(uri).Result;
			
			switch (response.StatusCode)
			{
				case HttpStatusCode.OK:
					return response.Content.Headers.ContentEncoding.Contains("gzip")
						? Gzip.Decompress(response.Content.ReadAsByteArrayAsync().Result)
						: response.Content.ReadAsByteArrayAsync().Result;

				default:
					// Well, fuck
					throw new InvalidHttpResponseException(response.ReasonPhrase, response);
			}
		}

		#endregion

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
