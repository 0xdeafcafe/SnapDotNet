using System;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace SnapDotNet.Core.SnapchatApi.DataModels
{
	/// <summary>
	/// Represents a person's activity which is visible to other users.
	/// </summary>
	[DataContract]
	public class PublicActivity
		: Response
	{
		[DataMember(Name = "best_friends")]
		public Collection<string> BestFriends { get; set; }

		[DataMember(Name = "score")]
		public int Score { get; set; }
	}
}
