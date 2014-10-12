using System;

namespace ColdSnap.Atlas
{
	public class AtlasUser
	{
		public string Id { get; set; }

		public string DeviceId { get; set; }

		public string SnapchatUsername { get; set; }

		public bool Subscribed { get; set; }

		public bool SnapNotify { get; set; }

		public bool ChatNotify { get; set; }

		public bool ScreenshotNotify { get; set; }

		public DateTime NextUpdate { get; set; }

		public bool Probation { get; set; }

		public DateTime LastPushServed { get; set; }

		public bool NewUser { get; set; }

		public string AuthToken { get; set; }

		public bool AuthTokenExpired { get; set; }
	}
}
