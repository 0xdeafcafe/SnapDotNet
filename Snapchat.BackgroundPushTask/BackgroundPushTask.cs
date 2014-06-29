using System;
using System.Diagnostics;
using System.Linq;
using Windows.ApplicationModel.Background;
using Windows.Networking.PushNotifications;
using Windows.UI.Notifications;
using Newtonsoft.Json;
using SnapDotNet.Core.Snapchat.Models.AppSpecific.RawNotificationTransfer;
using SnapDotNet.Core.Snapchat.Models.New;

namespace Snapchat.BackgroundPushTask
{
	public sealed class BackgroundPushTask : IBackgroundTask
	{
		public void Run(IBackgroundTaskInstance taskInstance)
		{
			var rawNotification = taskInstance.TriggerDetails as RawNotification;
			if (rawNotification == null) return;

			ShowToast("Recieved Raw Notification", "ColdSnap", "debug", "", false, false);

			try
			{
				var notifications = JsonConvert.DeserializeObject<RawNotificationTransferContainer>(rawNotification.Content);
				var snapNotifications = notifications.SnapNotifications;
				var chatMessageNotifications = notifications.ChatNotifications.Where(c => c.Type == MessageBodyType.Text || c.Type == MessageBodyType.Media).ToList();
				var chatScreenshotMessageNotifications = notifications.ChatNotifications.Where(c => c.Type == MessageBodyType.Screenshot).ToList();
				var screenshotNotifications = notifications.ScreenshotNotifications;

				foreach (var snap in snapNotifications)
					ShowToast(String.Format("New snap from {0}", snap.Sender), "ColdSnap", "snap", snap.Id, snapNotifications.Count() > 1, false);
				if (snapNotifications.Count() > 1)
					ShowToast(String.Format("{0} new snaps", snapNotifications.Count()), "ColdSnap", "snapcount", "", false, true);

				#region Process Chat Notifications

				foreach (var chatMessage in chatMessageNotifications)
					ShowToast(String.Format("New chat from {0}", chatMessage.Sender), "ColdSnap", "chat", chatMessage.Id, chatMessageNotifications.Count() > 1, false);
				if (chatMessageNotifications.Count() > 1)
					ShowToast(String.Format("{0} new chat", chatMessageNotifications.Count()), "ColdSnap", "chatcount", "", false, true);

				foreach (var chatScreenshotMessage in chatScreenshotMessageNotifications)
					ShowToast(String.Format("{0} screenshotted your chat!", chatScreenshotMessage.Sender), "ColdSnap", "chatscreenshot", chatScreenshotMessage.Id, chatScreenshotMessageNotifications.Count() > 1, false);
				if (chatScreenshotMessageNotifications.Count() > 1)
					ShowToast(String.Format("{0} chat screenshots!", chatScreenshotMessageNotifications.Count()), "ColdSnap", "chatscreenshotcount", "", false, true);

				#endregion

				foreach (var screenshotSnap in screenshotNotifications)
					ShowToast(String.Format("{0} screenshotted your snap!", screenshotSnap.Sender), "ColdSnap", "screenshot", screenshotSnap.Id, screenshotNotifications.Count() > 1, false);
				if (screenshotNotifications.Count() > 1)
					ShowToast(String.Format("{0} new snap screenshots!", screenshotNotifications.Count()), "ColdSnap", "screenshotcount", "", false, true);
			}
			catch (JsonReaderException jsonReaderException)
			{
				// corrupted json
				Debug.WriteLine(jsonReaderException);
			}
		}

		public void ShowToast(string body, string header, string group, string tag, bool suppressPopup, bool ghost)
		{
			const ToastTemplateType toastType = ToastTemplateType.ToastText02;
			var toastXml = ToastNotificationManager.GetTemplateContent(toastType);
			var toastText = toastXml.GetElementsByTagName("text");
			toastText[0].InnerText = header;
			toastText[1].InnerText = body;

			var toast = new ToastNotification(toastXml)
			{
				Group = @group,
				SuppressPopup = suppressPopup
			};
			// TODO: work out how the fuck to do tags. There is a max length, that is no where near the length of guid's.
			// So I guess I'll have to store the ID in isolated storage, and then give the notification a key that relates. (fucking ugh)

			//if (!String.IsNullOrWhiteSpace(tag))
			//	toast.Tag = tag;

			if (ghost)
				toast.ExpirationTime = DateTimeOffset.Now.AddMilliseconds(400);

			ToastNotificationManager.CreateToastNotifier().Show(toast);
		}
	}  
}
