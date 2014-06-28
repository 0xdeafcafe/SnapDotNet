using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SnapDotNet.Azure.MobileService.DataObjects
{
	[Table("snapdotnet.SnapchatAddedFriends")]
	public class SnapchatAddedFriend : BaseEntity
	{
		public Int64 UserId { get; set; }

		public String FriendUsername { get; set; }
	}
}