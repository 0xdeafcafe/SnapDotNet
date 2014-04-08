using System;
using Microsoft.WindowsAzure.Mobile.Service;

namespace SnapDotNet.Azure.MobileService.DataObjects
{
	public class Snap : EntityData
	{
		public int UserId { get; set; }
		public virtual User User { get; set; }

		public string SnapId { get; set; }

		public string SnapSenderUsername { get; set; }

		public Boolean NotificationSent { get; set; }
	}
}