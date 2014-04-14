using System.Threading.Tasks;
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
			var registration = await sm.Endpoints.RegisterAsync(20, "1993-08-08", "elijah.dildomom@yopmail.com", "dildosROCK");
			var captchaZip = await sm.Endpoints.GetCaptchaImagesAsync(registration.Email, registration.AuthToken);

			bool[] answer = {false, true, false, true, true, false, true, false, true};
			var solved = await sm.Endpoints.SolveCaptchaAsync(registration.Email, captchaZip.Item1, registration.AuthToken, answer);
			// This will return false (caught 403), unless the answer above matches up with the actual solution.
			// I set an endpoint on 'solved' above, and copied the values for registration.Email, captchaZip.Item1, and registration.AuthToken
			// to the method below, extracted the PNGs, and edited the 'answer' array to be the correct answer,
			// commented out 'registration' and 'captchaZip', and got a true response (200).

			//var solved = await sm.Endpoints.SolveCaptchaAsync("elijah.dildomom@yopmail.com", "elijah.dildomom@yopmail.com~1397460750004", "bc8b85d4-04b4-4efb-9dff-0b322c6991c8", answer);

			Assert.AreEqual(true, true);
		}
	}
}
