using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using SnapDotNet.Core.Snapchat.Api.Tests.Helpers;

namespace SnapDotNet.Core.Snapchat.Api.Tests
{
	[TestClass]
	public class EndpointTests
	{
		[TestMethod]
		public async Task GetUpdatesAsyncTest()
		{
			var authentication = SettingsLoader.GetAuthencationInfo();
			var snapChatManager = new SnapChatManager(authentication.Username, authentication.AuthToken);
			var account = await snapChatManager.Endpoints.GetUpdatesAsync(authentication.Username);

			Assert.AreEqual(true, account.Logged);
			Assert.AreEqual(authentication.Username.ToLowerInvariant(), account.Username.ToLowerInvariant());
		}
	}
}
