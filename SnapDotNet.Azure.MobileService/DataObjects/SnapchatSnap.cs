using System;
using System.ComponentModel.DataAnnotations.Schema;
using SnapDotNet.Azure.MobileService.Models;

namespace SnapDotNet.Azure.MobileService.DataObjects
{
	[Table("snapdotnet.SnapchatSnaps")]
	public class SnapchatSnap : BaseEntity
	{
		public String UserId { get; set; }

		public String SnapId { get; set; }

		public SnapStatus SnapStatus { get; set; }

		public String SenderName { get; set; }
	}
}
