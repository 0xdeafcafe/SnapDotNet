using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using SnapDotNet.Core.Snapchat.Api.Tests.Helpers;

namespace SnapDotNet.Core.Snapchat.Api.Tests
{
	[TestClass]
	public class EndpointTests
	{
		[TestMethod]
		public async Task LoginAsyncTest()
		{
			var authentication = SettingsLoader.GetAuthencationInfo();
			var snapChatManager = new SnapChatManager();
			var account = await snapChatManager.Endpoints.Login(authentication.Username, authentication.Password);

			Assert.IsTrue(true);
		}
	}
}
