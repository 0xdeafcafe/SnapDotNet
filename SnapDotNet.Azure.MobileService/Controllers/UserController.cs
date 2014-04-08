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
		
		// POST tables/TodoItem/48D68C86-6EA6-4C25-AA33-223FC9A27959
		public async Task<IHttpActionResult> PostTodoItem(User user)
		{
			var current = await InsertAsync(user);

			var message = new WindowsPushMessage
			{
				XmlPayload = @"<?xml version=""1.0"" encoding=""utf-8""?>" +
							 @"<toast><visual><binding template=""ToastText01"">" +
							 @"<text id=""1"">Your have been provisioned, " + user.SnapchatUsername + @". Awesome.</text>" +
							 @"</binding></visual></toast>"
			};
			try
			{
				var result = await Services.Push.SendAsync(message);
				Services.Log.Info(result.State.ToString());
			}
			catch (System.Exception ex)
			{
				Services.Log.Error(ex.Message);
			}


			return CreatedAtRoute("Tables", new { id = current.Id }, current);
		}
	}
}