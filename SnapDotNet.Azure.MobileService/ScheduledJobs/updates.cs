using System;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.ServiceBus.Notifications;
using Microsoft.WindowsAzure.Mobile.Service;
using Microsoft.WindowsAzure.Mobile.Service.ScheduledJobs;
using Newtonsoft.Json;
using SnapDotNet.Azure.MobileService.DataObjects;
using SnapDotNet.Azure.MobileService.Helpers;
using SnapDotNet.Azure.MobileService.Models;
using SnapDotNet.Azure.MobileService.Models.RawNotificationTransfer;

namespace SnapDotNet.Azure.MobileService.ScheduledJobs
{
	// A simple scheduled job which can be invoked manually by submitting an HTTP POST
	// request to the path "https://{azure-instance}.azure-mobile.net/jobs/Updates".
	// - yolo

	public class Updates : ScheduledJob
	{
		public Context Context;

		protected override void Initialize(ScheduledJobDescriptor scheduledJobDescriptor, CancellationToken cancellationToken)
		{
			base.Initialize(scheduledJobDescriptor, cancellationToken);

			// Create a new Context
			Context = new Context();
		}

		public override async Task ExecuteAsync()
		{
			Services.Log.Info("Update season started");
			var stopwatch = new Stopwatch();
			stopwatch.Start();

			var notificationNub = NotificationHubClient.CreateClientFromConnectionString(Services.Settings["MS_NotificationHubConnectionString"], 
				Services.Settings["MS_NotificationHubName"], true);
			var endpoints = new Endpoints();
			var usersToUpdate =
				await
					Context.Users.Where(u => !u.Probation && u.Subscribed && (u.NextUpdate <= DateTime.UtcNow) && !u.AuthTokenExpired)
						.ToListAsync();

			foreach (var user in usersToUpdate)
			{
				var deviceId = user.DeviceId;
				Services.Log.Info(String.Format("Updating {0}", user.SnapchatUsername));

				// Create empty collection of the raw notification container
				var notifications = new RawNotificationTransferContainer();

				try
				{
					var conversationsRaw = await endpoints.GetConversationsAsync(user.SnapchatUsername, user.AuthToken);
					if (conversationsRaw.Item1.StatusCode == HttpStatusCode.Forbidden)
					{
						// Auth token has expired
						user.AuthTokenExpired = true;
						goto alldone;
					}

					foreach (
						var message in
							conversationsRaw.Item2.ConversationResponses.SelectMany(
								conversation => conversation.ConversationMessages.Messages))
					{
						if (message.ChatMessage != null)
						{
							// Deal with it like a chat message/media/screenshot event
							var chatMessage = message.ChatMessage;
							var existingChatMessage = Context.SnapchatChats.FirstOrDefault(m => m.ChatId == chatMessage.Id);
							if (existingChatMessage != null ||
								chatMessage.Sender == user.SnapchatUsername) continue;

							// Not already there, save it yo!
							Context.SnapchatChats.Add(new SnapchatChat
							{
								Id = Guid.NewGuid().ToString("N"),
								ChatId = chatMessage.Id,
								SenderName = chatMessage.Sender,
								UserId = user.Id
							});
							await Context.SaveChangesAsync();
							if (user.ChatNotify)
								notifications.ChatNotifications.Add(new ChatNotification
								{
									Id = chatMessage.Id,
									Sender = chatMessage.Sender,
									Type = chatMessage.Body.Type
								});
						}
						else if (message.Snap != null)
						{
							// Deal with it like a snap
							var snap = message.Snap;
							var existingSnap = Context.SnapchatSnaps.FirstOrDefault(s => s.SnapId == snap.Id && s.UserId == user.Id);
							if (existingSnap == null)
							{
								// Add snap and alert user!
								Context.SnapchatSnaps.Add(new SnapchatSnap
								{
									Id = Guid.NewGuid().ToString("N"),
									UserId = user.Id,
									SenderName = snap.SenderName,
									SnapId = snap.Id,
									SnapStatus = snap.Status
								});
								await Context.SaveChangesAsync();
								if (snap.IsIncoming && user.SnapNotify)
									notifications.SnapNotifications.Add(new SnapNotification { Id = snap.Id, Sender = snap.SenderName });

								continue;
							}

							if (snap.Status != SnapStatus.Screenshotted || existingSnap.SnapStatus == SnapStatus.Screenshotted || snap.IsIncoming)
								continue;

							// Snap status is now screenshot and wasn't before
							if (user.ScreenshotNotify)
								notifications.ScreenshotNotifications.Add(new ScreenshotNotification { Id = snap.Id, Sender = snap.SenderName });
							existingSnap.SnapStatus = snap.Status;
							await Context.SaveChangesAsync();
						}
					}

					alldone:
					;
				}
				catch (Exception ex)
				{
					base.Services.Log.Error(ex);
					if (ex.InnerException != null)
						base.Services.Log.Error(ex.InnerException);
				}
				finally
				{
					if (!user.NewUser && (notifications.ChatNotifications.Any() || notifications.FriendAddedNotifications.Any() ||
						notifications.SnapNotifications.Any()))
					{
						// Send notifications to windows phone user
						var rawNotification = new WindowsNotification(JsonConvert.SerializeObject(notifications));
						rawNotification.Headers.Add("X-WNS-Type", "wns/raw");
						var outcome = notificationNub.SendNotificationAsync(rawNotification, deviceId).Result;
						Services.Log.Info(String.Format("Push Served for {0}, outcome: {1}", user.SnapchatUsername, outcome));

						user.LastPushServed = DateTime.UtcNow;
					}

					user.NewUser = false;
					if ((DateTime.UtcNow - user.LastPushServed) > new TimeSpan(30, 0, 0, 0))
						user.NextUpdate = DateTime.UtcNow.AddHours(24);
					else if ((DateTime.UtcNow - user.LastPushServed) > new TimeSpan(7, 0, 0, 0))
						user.NextUpdate = DateTime.UtcNow.AddHours(6);
					else
						// Actually 1 minute, as the server only calls this every 60 seconds, but this is to make sure it doesn't /just/ miss
						user.NextUpdate = DateTime.UtcNow.AddSeconds(1); 
						
					// Do this in the finaly, so if there is an exception, we always save. less riskay
					Context.SaveChanges();
				}
			}

			stopwatch.Stop();
			Services.Log.Info(String.Format("Update season completed in {0}", stopwatch.Elapsed));

		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			if (disposing)
			{
				Context.Dispose();
			}
		}

		///// <summary>
		///// Gets the users friendly name
		///// </summary>
		//private static string GetFriendlyName(Account account, string name)
		//{
		//	var friend = account.Friends.FirstOrDefault(f => f.Name == name);
		//	return friend == null ? name : friend.FriendlyName;
		//}
	}
}