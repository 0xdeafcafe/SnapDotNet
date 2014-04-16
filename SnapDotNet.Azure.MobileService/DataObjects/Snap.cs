using Microsoft.WindowsAzure.Mobile.Service;
using SnapDotNet.Azure.MobileService.Models;

namespace SnapDotNet.Azure.MobileService.DataObjects
{
	public class Snap : EntityData
	{
		public string UserId { get; set; }

		public string RecipientUsername { get; set; }

		public string SenderUsername { get; set; }

		public SnapStatus Status { get; set; }
	}
}