using System;

namespace SnapDotNet.Azure.Atlas.DataObjects
{
	public class Snap
	{
		public int UserId { get; set; }
		public virtual User User { get; set; }

		public string SnapId { get; set; }

		public string SnapSenderUsername { get; set; }

		public Boolean NotificationSent { get; set; }
	}
}