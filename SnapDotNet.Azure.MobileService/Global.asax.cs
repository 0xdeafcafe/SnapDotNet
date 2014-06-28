using System.Web;

namespace SnapDotNet.Azure.MobileService
{
	public class WebApiApplication : HttpApplication
	{
		protected void Application_Start()
		{
			WebApiConfig.Register();
		}
	}
}