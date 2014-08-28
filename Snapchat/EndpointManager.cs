using System.Runtime.InteropServices.WindowsRuntime;
using Newtonsoft.Json;
using SnapDotNet.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Windows.Web.Http;
using Windows.Web.Http.Headers;

using UnicodeEncoding = Windows.Storage.Streams.UnicodeEncoding;

namespace SnapDotNet
{
	internal sealed class EndpointManager
	{
		/// <summary>
		/// Contains a <see cref="EndpointManager"/> for each endpoint library.
		/// </summary>
		public static readonly IReadOnlyDictionary<string, EndpointManager> Managers = new Dictionary<string, EndpointManager>
		{
			{ "bq", new EndpointManager(new Uri("https://feelinsonice-hrd.appspot.com/bq/")) },
			{ "ph", new EndpointManager(new Uri("https://feelinsonice-hrd.appspot.com/ph/")) },
			{ "loq", new EndpointManager(new Uri("https://feelinsonice-hrd.appspot.com/loq/")) },
		};

		public EndpointManager(Uri baseUri)
		{
			BaseUri = baseUri;
		}

		/// <summary>
		/// Gets or sets the base URI of the API.
		/// </summary>
		private Uri BaseUri { get; set; }

		#region POST

		/// <summary>
		/// Sends a POST request to an API endpoint, and deserializes the response into the type
		/// specified by <see cref="TResult"/>.
		/// </summary>
		/// <param name="endpointName">The name of the endpoint.</param>
		/// <param name="data">A dictionary containing the data to include in the POST request.</param>
		/// <returns>Upon success, the deserialized response data.</returns>
		public async Task<TResult> PostAsync<TResult>(string endpointName, Dictionary<string, string> data)
		{
			Contract.Requires<ArgumentNullException>(endpointName != null);
			return await PostAsync<TResult>(endpointName, data, ApiSettings.StaticToken, null);
		}

		/// <summary>
		/// Sends a POST request to an API endpoint, and deserializes the response into the type
		/// specified by <see cref="TResult"/>.
		/// </summary>
		/// <param name="endpointName">The name of the endpoint.</param>
		/// <param name="data">A dictionary containing the data to include in the POST request.</param>
		/// <param name="token">The token that will be used to generate a request token.</param>
		/// <returns>Upon success, the deserialized response data.</returns>
		public async Task<TResult> PostAsync<TResult>(string endpointName, Dictionary<string, string> data, string token)
		{
			Contract.Requires<ArgumentNullException>(endpointName != null && token != null);
			return await PostAsync<TResult>(endpointName, data, token, null);
		}

		/// <summary>
		/// Sends a POST request to an API endpoint, and deserializes the response into the type
		/// specified by <see cref="TResult"/>.
		/// </summary>
		/// <param name="endpointName">The name of the endpoint.</param>
		/// <param name="data">A dictionary containing the data to include in the POST request.</param>
		/// <param name="token">The token that will be used to generate a request token.</param>
		/// <param name="headers">
		/// A dictionary containing additional headers to include in the POST request.
		/// </param>
		/// <typeparam name="TResult">The type of the response.</typeparam>
		/// <returns>Upon success, the deserialized response data.</returns>
		public async Task<TResult> PostAsync<TResult>(string endpointName, Dictionary<string, string> data, string token, Dictionary<string, string> headers)
		{
			Contract.Requires<ArgumentNullException>(endpointName != null);

			var response = await PostAsync(endpointName, data, token, headers);

			// Obtain the JSON data (and decompress it if it is gzipped).
			string jsonData;
			if (response.Content.Headers.ContentEncoding.Contains(HttpContentCodingHeaderValue.Parse("gzip")))
			{
				var compressedData = await response.Content.ReadAsBufferAsync();
				jsonData = await GZip.DecompressToStringAsync(compressedData, Encoding.UTF8);
			}
			else
			{
				jsonData = await response.Content.ReadAsStringAsync();
			}

			// Deserialize the JSON data and return it.
			Debug.WriteLine("[Endpoint Manager] Incoming json data: {0}", jsonData);
			return await Task.Run(() => JsonConvert.DeserializeObject<TResult>(jsonData));
		}

		/// <summary>
		/// Sends a POST request to an API endpoint.
		/// </summary>
		/// <param name="endpointName">The name of the endpoint.</param>
		/// <param name="data">A dictionary containing the data to include in the POST request.</param>
		/// <returns>Upon success, a <see cref="HttpResponseMessage"/> obtained from the API.</returns>
		public async Task<HttpResponseMessage> PostAsync(string endpointName, Dictionary<string, string> data)
		{
			Contract.Requires<ArgumentNullException>(endpointName != null);
			return await PostAsync(endpointName, data, ApiSettings.StaticToken, null);
		}

		/// <summary>
		/// Sends a POST request to an API endpoint.
		/// </summary>
		/// <param name="endpointName">The name of the endpoint.</param>
		/// <param name="data">A dictionary containing the data to include in the POST request.</param>
		/// <param name="token">The token that will be used to generate a request token.</param>
		/// <param name="headers">
		/// A dictionary containing additional headers to include in the POST request.
		/// </param>
		/// <returns>Upon success, a <see cref="HttpResponseMessage"/> obtained from the API.</returns>
		public async Task<HttpResponseMessage> PostAsync(string endpointName, Dictionary<string, string> data, string token, Dictionary<string, string> headers)
		{
			Contract.Requires<ArgumentNullException>(endpointName != null);

			var client = new HttpClient();
			data = data ?? new Dictionary<string, string>();
			
			// Do timestamp stuff
			var timestamp = DateTime.UtcNow.ToJScriptTime().ToString(CultureInfo.InvariantCulture);
			data["timestamp"] = timestamp;

			// Generate request token.
			data["req_token"] = GenerateRequestToken(token, timestamp);

			// Set up HTTP request headers.
			client.DefaultRequestHeaders.TryAppendWithoutValidation("User-Agent", ApiSettings.UserAgent);
			client.DefaultRequestHeaders.TryAppendWithoutValidation("Content-Length", "160");
			client.DefaultRequestHeaders.TryAppendWithoutValidation("Accept", "*/*");
			client.DefaultRequestHeaders.TryAppendWithoutValidation("Accept-Encoding", "gzip,deflate");
			if (headers != null)
			{
				foreach (var header in headers)
					client.DefaultRequestHeaders.Add(header.Key, header.Value);
			}

			// Encode the data and build endpoint URI.
			var postBody = String.Join("&", data.Select(o => String.Format("{0}={1}", o.Key, Uri.EscapeDataString(o.Value))));
			var endpoint = new Uri(BaseUri, endpointName);

			// POST to endpoint and return the response if it succeeded.
			Debug.WriteLine("[Endpoint Manager] POST to {0}", endpoint);
			var response = await client.PostAsync(endpoint, new HttpStringContent(postBody, UnicodeEncoding.Utf8, "application/x-www-form-urlencoded"));
			if (response.StatusCode == HttpStatusCode.Ok)
				return response;

			// Something bad happened.
			Debug.WriteLine("[Endpoint Manager] Invalid HTTP response (reason: {0})", response.ReasonPhrase);
			throw new InvalidHttpResponseException(response.ReasonPhrase, response);
		}

		#endregion

		#region GET

		public async Task<byte[]> GetAsync(string endpointName)
		{
			Contract.Requires<ArgumentNullException>(endpointName != null);

			var response = await GetAsync(endpointName, null);

			// Obtain the JSON data (and decompress it if it is gzipped).
			byte[] data;
			if (response.Content.Headers.ContentEncoding.Contains(HttpContentCodingHeaderValue.Parse("gzip")))
			{
				var compressedData = await response.Content.ReadAsBufferAsync();
				data = await GZip.DecompressAsync(compressedData);
			}
			else
			{
				data = (await response.Content.ReadAsBufferAsync()).ToArray();
			}

			// Deserialize the JSON data and return it.
			Debug.WriteLine("[Endpoint Manager] Incoming data");
			return data;
		}

		public async Task<HttpResponseMessage> GetAsync(string endpointName, Dictionary<string, string> headers)
		{
			Contract.Requires<ArgumentNullException>(endpointName != null);

			var client = new HttpClient();
			client.DefaultRequestHeaders.TryAppendWithoutValidation("User-Agent", ApiSettings.UserAgent);
			client.DefaultRequestHeaders.TryAppendWithoutValidation("Content-Length", "160");
			client.DefaultRequestHeaders.TryAppendWithoutValidation("Accept", "*/*");
			client.DefaultRequestHeaders.TryAppendWithoutValidation("Accept-Encoding", "gzip,deflate");
			if (headers != null)
				foreach (var header in headers)
					client.DefaultRequestHeaders.Add(header.Key, header.Value);

			// Encode the data and build endpoint URI.
			var endpoint = new Uri(BaseUri, endpointName);

			// GET to endpoint and return the response if it succeeded.
			Debug.WriteLine("[Endpoint Manager] GET to {0}", endpoint);
			var response = await client.GetAsync(endpoint);
			if (response.StatusCode == HttpStatusCode.Ok)
				return response;

			// Something bad happened.
			Debug.WriteLine("[Endpoint Manager] Invalid HTTP response (reason: {0})", response.ReasonPhrase);
			throw new InvalidHttpResponseException(response.ReasonPhrase, response);
		}

		#endregion

		/// <summary>
		/// Generates a request token from the given POST data and static token.
		/// </summary>
		/// <param name="postData">The encoded POST data.</param>
		/// <param name="staticToken">The Snapchat static token.</param>
		/// <returns>A request token, all nice.</returns>
		private string GenerateRequestToken(string postData, string staticToken)
		{
			Contract.Requires<ArgumentNullException>(postData != null && staticToken != null);

			// SHA-256 hashing function
			Func<string, string> hash = data =>
			{
				var input = CryptographicBuffer.ConvertStringToBinary(data, BinaryStringEncoding.Utf8);
				var hashedData = HashAlgorithmProvider.OpenAlgorithm("SHA256").HashData(input);
				return CryptographicBuffer.EncodeToHexString(hashedData);
			};

			var s1 = hash(ApiSettings.Secret + postData);
			var s2 = hash(staticToken + ApiSettings.Secret);

			var output = new StringBuilder();
			for (var i = 0; i < ApiSettings.HashingPattern.Length; i++)
			{
				output.Append(ApiSettings.HashingPattern[i] == '0' 
					? s1[i] 
					: s2[i]);
			}
			return output.ToString();
		}
	}
}
