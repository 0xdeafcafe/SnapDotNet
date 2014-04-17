using System.Runtime.Serialization;
using SnapDotNet.Core.Miscellaneous.Models;

namespace SnapDotNet.Core.Snapchat.Models
{
	[DataContract]
	public class BestFriends : NotifyPropertyChangedBase
	{
		[DataMember(Name = "best_friends")]
		public string[] Friends
		{
			get { return _friends; }
			set { SetField(ref _friends, value); }
		}
		private string[] _friends;
	}
}
