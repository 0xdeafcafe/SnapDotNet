using System.Data.Entity.Migrations;
using Microsoft.WindowsAzure.Mobile.Service;
using SnapDotNet.Azure.MobileService.Migrations;

namespace SnapDotNet.Azure.MobileService
{
	public static class WebApiConfig
	{
		public static void Register()
		{
			// Use this class to set WebAPI configuration options
			var config = ServiceConfig.Initialize(new ConfigBuilder(new ConfigOptions()));

			// To display errors in the browser during development, uncomment the following
			// line. Comment it out again when you deploy your service for production use.
			// config.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;

			// Code first migrations
			var migrator = new DbMigrator(new Configuration());
			migrator.Update();  
		}
	}
}

