using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.WindowsAzure.Mobile.Service;

namespace SnapDotNet.Azure.MobileService.ScheduledJobs
{
	// A simple scheduled job which can be invoked manually by submitting an HTTP
	// POST request to the path "/jobs/Updates".

	public class Updates : ScheduledJob
	{
		public override Task ExecuteAsync()
		{
			Services.Log.Info("Hello from scheduled job!");
			return Task.FromResult(true);
		}
	}
}