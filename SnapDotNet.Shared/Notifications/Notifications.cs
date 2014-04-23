using System;
using System.Collections.Generic;
using Windows.UI.Notifications;

namespace SnapDotNet.Apps
{
	public static partial class Notifications
	{
		/// <summary>
		/// Obtains a <see cref="ToastNotifier"/> object for the calling app.
		/// </summary>
		public static readonly ToastNotifier ToastNotifier =
			ToastNotificationManager.CreateToastNotifier();

		/// <summary>
		/// Obtains a <see cref="TileUpdater"/> object for the calling app.
		/// </summary>
		public static readonly TileUpdater TileUpdater =
			TileUpdateManager.CreateTileUpdaterForApplication();

		/// <summary>
		/// Obtains a dictionary correlating a <see cref="TileUpdater"/> object with each secondary tile.
		/// </summary>
		public static readonly Dictionary<string, TileUpdater> SecondaryTileUpdater =
			new Dictionary<string, TileUpdater>();

		/// <summary>
		/// Obtains a <see cref="BadgeUpdater"/> object for the calling app.
		/// </summary>
		public static readonly BadgeUpdater BadgeUpdater =
			BadgeUpdateManager.CreateBadgeUpdaterForApplication();

		/// <summary>
		/// Obtains a dictionary correlating a <see cref="BadgeUpdater"/> object with each secondary tile.
		/// </summary>
		public static readonly Dictionary<string, BadgeUpdater> SecondaryBadgeUpdater =
			new Dictionary<string, BadgeUpdater>();

		/// <summary>
		/// 
		/// </summary>
		/// <param name="body"></param>
		/// <param name="parameter"></param>
		/// <param name="callback"></param>
		public static void ShowToast(string body, string parameter, Action<string> callback)
		{
			ShowToast(null, body, parameter, callback);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="header"></param>
		/// <param name="body"></param>
		/// <param name="parameter"></param>
		/// <param name="callback"></param>
		public static void ShowToast(string header, string body, string parameter, Action<string> callback)
		{
			// Create the content of the toast.
			ToastContent.IToastNotificationContent toast;
			if (string.IsNullOrEmpty(header))
			{
				var toastContent = ToastContent.ToastContentFactory.CreateToastText01();
				{
					toastContent.TextBodyWrap.Text = body;
				}
				toast = toastContent;
			}
			else
			{
				var toastContent = ToastContent.ToastContentFactory.CreateToastText02();
				{
					toastContent.TextHeading.Text = header;
					toastContent.TextBodyWrap.Text = body;
				}
				toast = toastContent;
			}
			toast.Launch = parameter;

			// Create the toast object and display it.
			var notification = toast.CreateNotification();
			if (callback != null)
			{
				notification.Activated += (ToastNotification sender, object args) =>
				{

					callback.Invoke(args.ToString());
				};
			}
			ToastNotifier.Show(notification);
		}
	}
}