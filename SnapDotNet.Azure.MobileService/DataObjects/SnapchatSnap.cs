using System;
using System.ComponentModel.DataAnnotations.Schema;
using SnapDotNet.Azure.MobileService.Models;

namespace SnapDotNet.Azure.MobileService.DataObjects
{
	[Table("snapdotnet.SnapchatSnaps")]
	public class SnapchatSnap : BaseEntity
	{
		public Int64 UserId { get; set; }

		public String SnapId { get; set; }

		public String SnapMediaId { get; set; }

		public SnapStatus SnapStatus { get; set; }

		public String SenderName { get; set; }
	}
}
