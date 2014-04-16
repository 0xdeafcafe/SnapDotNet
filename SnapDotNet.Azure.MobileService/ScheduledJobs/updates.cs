using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.WindowsAzure.Mobile.Service;
using SnapDotNet.Azure.MobileService.Helpers;
using SnapDotNet.Azure.MobileService.Models;
using Snap = SnapDotNet.Azure.MobileService.DataObjects.Snap;

namespace SnapDotNet.Azure.MobileService.ScheduledJobs
{
	// A simple scheduled job which can be invoked manually by submitting an HTTP
	// POST request to the path "/jobs/Updates".

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
						var updates = await endpoints.GetUpdatesAsync(user.SnapchatUsername, user.SnapchatAuthToken);
						if (updates == null)
						{
							user.AuthExpired = true;
							goto alldone;
						}

						List<Snap> snaps;
						try
						{
							var snapsQuery = context.Snaps.Where(s => s.UserId == safeUser.Id);
							snaps = await snapsQuery.ToListAsync();
						}
						catch (Exception exception)
						{
							snaps = new List<Snap>();
							base.Services.Log.Error(exception);
						}
						foreach (var newSnap in updates.Snaps)
						{
							var currentSnap = snaps.FirstOrDefault(s => s.Id == newSnap.Id);
							if (currentSnap == null)
							{
								context.Snaps.Add(new Snap
								{
									Id = newSnap.Id,
									UserId = safeUser.Id,
									SenderUsername = newSnap.SenderName,
									RecipientUsername = newSnap.RecipientName,
									Status = newSnap.Status
								});

								// you have a new snap
								if (!safeUser.NewUser && newSnap.Status == SnapStatus.Delivered && newSnap.RecipientName == null)
									await SendWncToastAsync(String.Format("New Snap from {0}", GetFriendlyName(updates, newSnap.SenderName)), "Snapchat", safeUser.DeviceIdent);

								// bastard screenshotted
								if (!safeUser.NewUser && newSnap.Status == SnapStatus.Screenshotted && newSnap.SenderName == null)
									await SendWncToastAsync(String.Format("{0} screenshotted your snap", GetFriendlyName(updates, newSnap.RecipientName)), "Snapchat", safeUser.DeviceIdent);

								continue;
							}

							if (newSnap.RecipientName == null)
								continue;

							// check to see if the snap was screenshotted
							if (currentSnap.Status == SnapStatus.Screenshotted || newSnap.Status != SnapStatus.Screenshotted)
								continue;

							currentSnap.Status = newSnap.Status;
							await SendWncToastAsync(String.Format("{0} screenshotted your snap", GetFriendlyName(updates, newSnap.RecipientName)), "Snapchat", safeUser.DeviceIdent);
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