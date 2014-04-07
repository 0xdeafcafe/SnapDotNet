using System.Web;

namespace SnapDotNet.Azure.Atlas
{
	public class WebApiApplication : HttpApplication
	{
		protected void Application_Start()
		{
			WebApiConfig.Register();
		}
	}
}