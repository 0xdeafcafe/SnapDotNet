using System;

namespace SnapDotNet.Core.Miscellaneous.Models.Atlas
{
	public class User
	{
		public string Id { get; set; }

		public string DeviceIdent { get; set; }

		public string SnapchatUsername { get; set; }

		public string SnapchatAuthToken { get; set; }

		public Boolean AuthExpired { get; set; }

		public Boolean NewUser { get; set; }
	}
}