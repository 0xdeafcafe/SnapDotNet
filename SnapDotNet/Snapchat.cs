using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Web.Http;
using SnapDotNet.Data;
using SnapDotNet.Data.ApiResponses;

namespace SnapDotNet
{
	public static class Snapchat
	{
		/// <summary>
		/// Authenticates a user using the given <paramref name="username"/> and <paramref name="password"/>,
		/// and retrieves an <see cref="Account"/> object containing the account data.
		/// </summary>
		/// <param name="username">The username.</param>
		/// <param name="password">The password.</param>
		/// <returns>
		/// If authenticated, an <see cref="Account"/> object containing the account data.
		/// </returns>
		/// <exception cref="InvalidCredentialsException">
		/// Given set of credentials is incorrect.
		/// </exception>
		[Pure]
		public static async Task<Account> AuthenticateAsync(string username, string password)
		{
			Contract.Requires<ArgumentNullException>(username != null && password != null);
			return await Account.AuthenticateAsync(username, password);
		}

		/*var postData = new Dictionary<string, string>
		//	{
		//		{"age",age.ToString()},
		//		{"birthday", birthday}, // YYYY-MM-DD
		//		{"timestamp", timestamp.ToString(CultureInfo.InvariantCulture)},
		//		{"email", email},
		//		{"password", password}
		//	};*/

		/// <summary>
		/// 
		/// </summary>
		/// <param name="email"></param>
		/// <param name="password"></param>
		/// <param name="birthday"></param>
		/// <returns></returns>
		public static async Task<RegistrationSession> RegisterAsync(string email, string password, DateTime birthday)
		{
			Contract.Requires<ArgumentNullException>(email != null && password != null);
			Debug.WriteLine("[Account] Creating new account...", email, password);

			var today = DateTime.Today;
			int age = today.Year - birthday.Year;
			if (birthday > today.AddYears(-age)) age--;
			string birthdayStr = string.Format("{0:D4}-{1:D2}-{2:D2}", birthday.Year, birthday.Month, birthday.Day);

			var data = new Dictionary<string, string>
			{
				{ "email", email },
				{ "password", password },
				{ "age", age.ToString() },
				{ "birthday", birthdayStr }
			};

			var response = await EndpointManager.Managers["bq"].PostAsync<RegistrationResponse>("register", data);
			return RegistrationSession.CreateContinuation(response);
		}
	}

	/// <summary>
	/// Provides a continuation to <see cref="Snapchat.RegisterAsync"/>.
	/// </summary>
	public sealed class RegistrationSession
	{
		private RegistrationSession() { }

		/// <summary>
		/// Gets a boolean value indicating whether an error occurred during registration.
		/// </summary>
		public bool HasError { get { return ErrorMessage != null; } }

		/// <summary>
		/// Gets the error message (if any).
		/// </summary>
		public string ErrorMessage { get; private set; }

		private string AuthToken { get; set; }
		private string Email { get; set; }

		internal static RegistrationSession CreateContinuation(RegistrationResponse response)
		{ 
			if (response.IsLogged)
				return new RegistrationSession { AuthToken = response.AuthToken, Email = response.Email };
			else
				return new RegistrationSession { ErrorMessage = response.Message };
		}

		public async Task<CaptchaChallenge> GetCaptchaAsync()
		{
			Contract.Requires<InvalidOperationException>(!HasError);

			var data = new Dictionary<string, string>
			{
				{ "username", Email },
			};

			var response = await EndpointManager.Managers["bq"].PostAsync("get_captcha", data, AuthToken, null);

			var captchaId = response.Content.Headers.ToString();
			captchaId = captchaId.Substring(captchaId.IndexOf("filename=") + 9);
			captchaId = captchaId.Substring(0, captchaId.IndexOf(".zip\r\n"));

			// Extract all captcha images.
			var captchaImages = await Task.Run<List<byte[]>>(async () =>
			{
				using (var stream = new MemoryStream((await response.Content.ReadAsBufferAsync()).ToArray()))
				using (var zip = new ZipArchive(stream))
				{
					var files = new List<byte[]>();
					foreach (var file in zip.Entries)
					{
						var buffer = new byte[file.Length];
						using (var fileStream = file.Open())
						{
							await fileStream.ReadAsync(buffer, 0, buffer.Length);
							files.Add(buffer);
						}
					}
					return files;
				}
			});

			return new CaptchaChallenge(captchaImages, captchaId);
		}

		public async Task<bool> RegisterUsernameAsync(CaptchaChallenge captcha, string username)
		{
			Contract.Requires<ArgumentNullException>(captcha != null && username != null);
			Contract.Requires<InvalidOperationException>(captcha.IsSolved, "Captcha must be solved first");

			var data = new Dictionary<string, string>
			{
				{ "username", Email },
				{ "captcha_id", captcha.CaptchaId },
				{ "captcha_solution", captcha.Solution }
			};

			try
			{
				var response = await EndpointManager.Managers["bq"].PostAsync<Response>("solve_captcha", data, AuthToken);

				// Now register username
				return true;
			}
			catch (InvalidHttpResponseException ex)
			{
				if (ex.HttpResponseMessage.StatusCode == HttpStatusCode.Forbidden)
					return false;

				throw;
			}

			/*

		//	try
		//	{
		//		// Http request went through, meaning the captcha was solved correctly
		//		await
		//			_webConnect.PostToResponseAsync(SolveCaptchaEndpointUrl, postData, authToken,
		//				timestamp.ToString(CultureInfo.InvariantCulture));

		//		return true;
		//	}

		//	catch (InvalidHttpResponseException ex)
		//	{
		//		switch (ex.HttpResponseMessage.StatusCode)
		//		{
		//			// This is given when the captcha is solved incorrectly.
		//			case HttpStatusCode.Forbidden:
		//				return false;

		//			default:
		//				throw;
		//		}
		//	}*/
		}

		// solve_captcha, register_username
	}

	public sealed class CaptchaChallenge
	{
		private List<byte[]> _images;

		internal CaptchaChallenge(List<byte[]> captchaImages, string captchaId)
		{
			_images = captchaImages;
			CaptchaId = captchaId;
		}

		public IReadOnlyList<byte[]> Images { get { return _images; } }

		public bool IsSolved { get { return !string.IsNullOrEmpty(Solution); } }

		public string Solution { get; private set; }

		internal string CaptchaId { get; private set; }

		public void Solve(params bool[] solution)
		{
			Contract.Requires<ArgumentNullException>(solution != null);
			Contract.Requires<ArgumentException>(solution.Length == Images.Count);
			Solution = solution.Aggregate("", (current, b) => current + (b ? "1" : "0"));
		}
	}
}
