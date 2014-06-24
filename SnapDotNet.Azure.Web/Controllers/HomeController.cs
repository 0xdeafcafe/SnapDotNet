using System.Web.Mvc;

namespace SnapDotNet.Azure.Web.Controllers
{
	public class HomeController : Controller
	{
		public ActionResult Index()
		{
			return View();
		}
	}
}