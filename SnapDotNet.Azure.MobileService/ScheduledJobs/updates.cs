using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.WindowsAzure.Mobile.Service;
using SnapDotNet.Azure.MobileService.Helpers;
using SnapDotNet.Azure.MobileService.Models;
using Snap = SnapDotNet.Azure.MobileService.DataObjects.Snap;

namespace SnapDotNet.Azure.MobileService.ScheduledJobs
{
	// A simple scheduled job which can be invoked manually by submitting an HTTP POST
	// request to the path "https://{azure-instance}.azure-mobile.net/jobs/Updates".
	//
	// This is called every 60 seconds by Azure, and ensured clients recieve notifications
	// on time. This has not been stress tested, so I wouldn't be suprised if it can't
	// handle extreme load. Only time shall tell.
	// - yolo

	public class Updates : ScheduledJob
	{
		public override async Task ExecuteAsync()
		{
			var stopwatch = new Stopwatch();
			stopwatch.Start();

			while(true)
			{
				if (stopwatch.Elapsed.TotalSeconds > 55)
					break;

				using (var context = new Context(Services.Settings.Schema))
				{
					var endpoints = new Endpoints();
					var usersToUpdate = await context.Users.Where(u => !u.AuthExpired).ToListAsync();

					// download snap status
					foreach (var user in usersToUpdate)
					{
						var safeUser = user;
						var updateRaw = await endpoints.GetUpdatesAsync(user.SnapchatUsername, user.SnapchatAuthToken);
						if (updateRaw.Item1.StatusCode == HttpStatusCode.Forbidden)
						{
							user.AuthExpired = true;
						}
						else if (updateRaw.Item1.StatusCode != HttpStatusCode.OK || updateRaw.Item2 == null)
						{
							// skip to end, something fucked
							goto alldone;
						}

						// Get account data
						var updates = updateRaw.Item2;

						foreach (var newSnap in updates.Snaps)
						{
							var safeNewSnap = newSnap;
							var currentSnap = await context.Snaps.FirstOrDefaultAsync(s => s.Id == safeNewSnap.Id);
							if (currentSnap == null)
							{
								context.Snaps.Add(new Snap
								{
									Id = safeNewSnap.Id,
									UserId = safeUser.Id,
									SenderUsername = safeNewSnap.SenderName,
									RecipientUsername = safeNewSnap.RecipientName,
									Status = safeNewSnap.Status
								});

								// you have a new snap
								if (!safeUser.NewUser && safeNewSnap.Status == SnapStatus.Delivered && safeNewSnap.RecipientName == null)
									await SendWncToastAsync(String.Format("New Snap from {0}", GetFriendlyName(updates, safeNewSnap.SenderName)), "Snapchat", safeUser.DeviceIdent);

								// bastard screenshotted
								if (!safeUser.NewUser && safeNewSnap.Status == SnapStatus.Screenshotted && safeNewSnap.SenderName == null)
									await SendWncToastAsync(String.Format("{0} screenshotted your snap", GetFriendlyName(updates, safeNewSnap.RecipientName)), "Snapchat", safeUser.DeviceIdent);

								continue;
							}

							if (safeNewSnap.RecipientName == null)
								continue;

							// check to see if the snap was screenshotted
							if (currentSnap.Status == SnapStatus.Screenshotted || safeNewSnap.Status != SnapStatus.Screenshotted)
								continue;

							currentSnap.Status = safeNewSnap.Status;
							await SendWncToastAsync(String.Format("{0} screenshotted your snap", GetFriendlyName(updates, safeNewSnap.RecipientName)), "Snapchat", safeUser.DeviceIdent);
						}

						user.NewUser = false;

					alldone:

						// update Database
						try
						{
							await context.SaveChangesAsync();
						}
						catch (DbEntityValidationException exception)
						{
							Services.Log.Error(exception);
						}
						catch (DbUpdateException exception)
						{
							Services.Log.Error(exception);
						}
						catch (Exception exception)
						{
							Services.Log.Error(exception);
						}
					}
				}

				Services.Log.Info("Update cycle completed");
				Thread.Sleep(new TimeSpan(0, 0, 10));
			}

			stopwatch.Stop();
			Services.Log.Info("Update season completed");
		}

		/// <summary>
		/// Gets the users friendly name
		/// </summary>
		private static string GetFriendlyName(Account account, string name)
		{
			var friend = account.Friends.FirstOrDefault(f => f.Name == name);
			return friend == null ? name : friend.FriendlyName;
		}

		/// <summary>
		/// Send a Toast to the device
		/// </summary>
		private async Task SendWncToastAsync(string text1, string text2, string ident)
		{
			try
			{
				var message = new WindowsPushMessage
				{
					XmlPayload = @"<?xml version=""1.0"" encoding=""utf-8""?>" +
								 @"<toast><visual><binding template=""ToastText01"">" +
								 @"<text id=""2"">"+text1+"</text>" +
								 @"<text id=""1"">" + text2 + "</text>" +
								 @"</binding></visual></toast>"
				};
				await Services.Push.SendAsync(message, ident);
			}
			catch (Exception exception)
			{
				base.Services.Log.Error(exception);
			}
		}

	}
}