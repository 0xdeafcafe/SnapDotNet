using System;
using System.Runtime.Serialization;

namespace SnapDotNet.Data.ApiResponses
{
	[DataContract]
	internal class BestFriendsResponse
		: Response
	{
		[DataMember(Name = "best_friends")]
		public string[] BestFriends { get; set; }
	}
}
