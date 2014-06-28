using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.OData;
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
			var context = new Context();
			DomainManager = new EntityDomainManager<User>(context, Request, Services);
		}

		// GET tables/User
		public IQueryable<User> GetAllServiceTickets()
		{
			return Query();
		}

		// GET tables/User/48D68C86-6EA6-4C25-AA33-223FC9A27959
		public SingleResult<User> GetServiceTicket(string id)
		{
			return Lookup(id);
		}

		// PATCH tables/User/48D68C86-6EA6-4C25-AA33-223FC9A27959
		public Task<User> PatchServiceTicket(string id, Delta<User> patch)
		{
			return UpdateAsync(id, patch);
		}

		// POST tables/User/48D68C86-6EA6-4C25-AA33-223FC9A27959
		public async Task<IHttpActionResult> PostServiceTicket(User item)
		{
			var current = await InsertAsync(item);
			return CreatedAtRoute("Tables", new { id = current.Id }, current);
		}

		// DELETE tables/User/48D68C86-6EA6-4C25-AA33-223FC9A27959
		public Task DeleteServiceTicket(string id)
		{
			return DeleteAsync(id);
		}
	}
}
