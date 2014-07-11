using System;
using System.Runtime.Serialization;
using SnapDotNet.Core.Miscellaneous.Models;

namespace Snapchat.SnapLogic.Models.New
{
	[DataContract]
	public class BestFriends : 
		NotifyPropertyChangedBase
	{
		[DataMember(Name = "best_friends")]
		public String[] Friends
		{
			get { return _friends; }
			set { SetField(ref _friends, value); }
		}
		private String[] _friends;
	}
}
