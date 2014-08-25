using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace SnapDotNet.Responses
{
	[DataContract]
	internal class PublicActivityResponse
	{
		[DataMember(Name = "best_friends")]
		public ObservableCollection<string> BestFriends { get; set; }

		[DataMember(Name = "score")]
		public uint Score { get; set; }
	}
}
