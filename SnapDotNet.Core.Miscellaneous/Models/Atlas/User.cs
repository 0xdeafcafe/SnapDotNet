using System;

namespace SnapDotNet.Core.Miscellaneous.Models.Atlas
{
	public class User
	{
		public String Id { get; set; }

		public String DeviceId { get; set; }

		public String SnapchatUsername { get; set; }

		public Boolean Subscribed { get; set; }

		public Boolean SnapNotify { get; set; }

		public Boolean ChatNotify { get; set; }

		public Boolean ScreenshotNotify { get; set; }

		public DateTime NextUpdate { get; set; }

		public Boolean Probation { get; set; }

		public DateTime LastPushServed { get; set; }

		public Boolean NewUser { get; set; }

		public String AuthToken { get; set; }

		public Boolean AuthTokenExpired { get; set; }
	}
}
