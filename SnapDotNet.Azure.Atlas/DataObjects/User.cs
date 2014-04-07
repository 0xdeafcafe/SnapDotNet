using System;
using Microsoft.WindowsAzure.Mobile.Service;

namespace SnapDotNet.Azure.Atlas.DataObjects
{
	public class User : EntityData
	{
		public string DeviceIdent { get; set; }

		public string SnapchatUsername { get; set; }

		public string SnapchatAuthToken { get; set; }

		public Boolean AuthExpired { get; set; }
	}
}