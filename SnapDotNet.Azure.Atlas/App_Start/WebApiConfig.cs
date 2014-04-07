using Microsoft.WindowsAzure.Mobile.Service;

namespace SnapDotNet.Azure.Atlas
{
	public static class WebApiConfig
	{
		public static void Register()
		{
			// Use this class to set configuration options for your mobile service
			var options = new ConfigOptions();

			// Use this class to set WebAPI configuration options
			var config = ServiceConfig.Initialize(new ConfigBuilder(options));

			// To display errors in the browser during development, uncomment the following
			// line. Comment it out again when you deploy your service for production use.
			// config.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;
		}
	}
}

