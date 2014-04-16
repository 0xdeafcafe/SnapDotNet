using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using Microsoft.WindowsAzure.Mobile.Service;
using SnapDotNet.Azure.MobileService.DataObjects;
using SnapDotNet.Azure.MobileService.Models;

namespace SnapDotNet.Azure.MobileService.Controllers
{
	public class UserController : TableController<User>
	{
		protected override void Initialize(HttpControllerContext controllerContext)
		{
			base.Initialize(controllerContext);
			var context = new Context(Services.Settings.Schema);
			DomainManager = new EntityDomainManager<User>(context, Request, Services);
		}

		// POST tables/User/48D68C86-6EA6-4C25-AA33-223FC9A27959
		public async Task<IHttpActionResult> PostUser(User user)
		{
			var current = await InsertAsync(user);
			return CreatedAtRoute("Tables", new { id = current.Id }, current);
		}
	}
}