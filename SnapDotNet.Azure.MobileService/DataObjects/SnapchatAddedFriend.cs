using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SnapDotNet.Azure.MobileService.DataObjects
{
	[Table("snapdotnet.SnapchatAddedFriends")]
	public class SnapchatAddedFriend : BaseEntity
	{
		public String UserId { get; set; }

		public String FriendUsername { get; set; }
	}
}