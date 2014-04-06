using SnapDotNet.Core.Snapchat.Api.Tests.Models;

namespace SnapDotNet.Core.Snapchat.Api.Tests.Helpers
{
	public static class SettingsLoader
	{
		public static Authentication GetAuthencationInfo()
		{
			return new Authentication
			{
				AccessToken = Settings.AccessToken,
				Password = Settings.Password,
				Username = Settings.Username
			};
		}
	}
}
