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
			//var snapChatManager = new SnapChatManager();
			//await snapChatManager.Endpoints.AuthenticateAsync(authentication.Username, authentication.Password);
			var account = await snapChatManager.Endpoints.GetUpdatesAsync();
			var image = await snapChatManager.Endpoints.GetSnapBlobAsync(account.Snaps[0]);

			Assert.AreEqual(true, account.Logged);
			Assert.AreEqual(authentication.Username.ToLowerInvariant(), account.Username.ToLowerInvariant());
		}
	}
}
