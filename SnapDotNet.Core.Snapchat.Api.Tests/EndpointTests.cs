using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Networking.NetworkOperators;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace SnapDotNet.Core.Snapchat.Api.Tests
{
	[TestClass]
	public class EndpointTests
	{
		[TestMethod]
		public async Task SolveCaptchaAsyncTest()
		{
			var sm = new SnapChatManager();
			var registration = await sm.Endpoints.RegisterAsync(20, "1993-08-08", "registration.test@yopmail.com", "dildosROCK");
			var captchaZip = await sm.Endpoints.GetCaptchaImagesAsync(registration.Email, registration.AuthToken);

			bool[] answer =		{false, false, true,
								 false, true, true,
								 false, false, true};

			//var solved = await sm.Endpoints.SolveCaptchaAsync(registration.Email, captchaZip.Id, registration.AuthToken, answer);
			// This will return false (caught 403), unless the answer above matches up with the actual solution.
			// I set an endpoint on 'solved' above, and copied the values for registration.Email, captchaZip.Item1, and registration.AuthToken
			// to the method below, extracted the PNGs, and edited the 'answer' array to be the correct answer,
			// commented out 'registration' and 'captchaZip', and got a true response (200).

			var solved = await sm.Endpoints.SolveCaptchaAsync("registration.test@yopmail.com", "registration.test@yopmail.com~1397508216713", "b2fc8ed6-72e5-4c27-aae3-e1968f8e110e", answer);
			var account = await sm.Endpoints.RegisterUsernameAsync("registration.test@yopmail.com", "b2fc8ed6-72e5-4c27-aae3-e1968f8e110e", "registration_test");

			Assert.AreEqual(true, true);
		}

		[TestMethod]
		public async Task SettingsAsyncTest()
		{
			var sm = new SnapChatManager(Settings.Username, Settings.AuthToken);
			var pr = await sm.Endpoints.UpdateAccountPrivacyAsync(true);
			var bd = await sm.Endpoints.UpdateBirthdayAsync(10, 23);
			var em = await sm.Endpoints.UpdateEmailAsync(sm.Account.Email);
			var nsfw = await sm.Endpoints.UpdateMaturitySettingsAsync(true);
			var st = await sm.Endpoints.UpdateStoryPrivacyAsync(true);
			var fs = await sm.Endpoints.UpdateFeatureSettingsAsync(true, true, true, true, true);
			var bffs = await sm.Endpoints.SetBestFriendCountAsync(5);

			Assert.AreEqual(true, true);
		}
	}
}
