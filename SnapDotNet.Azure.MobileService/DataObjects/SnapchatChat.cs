using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SnapDotNet.Azure.MobileService.DataObjects
{
	[Table("snapdotnet.SnapchatChats")]
	public class SnapchatChat : BaseEntity
	{
		public Int64 UserId { get; set; }

		public String ChatId { get; set; }

		public String SenderName { get; set; }
	}
}
