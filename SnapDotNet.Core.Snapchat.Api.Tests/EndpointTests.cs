using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace SnapDotNet.Core.Snapchat.Api.Tests
{
	[TestClass]
	public class EndpointTests
	{
		[TestMethod]
		public async Task RegisterReturnCaptchaAsyncTest()
		{
			var captchaZip = await new SnapChatManager().Endpoints.RegisterAndGetCaptchaAsync(19, "1994-08-18", "i.love.xerax@yopmail.com", "iloveALEX");
			// captchaZip is a List of byte arrays which are the data of PNG files

			Assert.AreEqual(true, true);
		}
	}
}
