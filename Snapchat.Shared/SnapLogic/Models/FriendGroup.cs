using SnapDotNet.Core.Miscellaneous.Models;
using System;
using System.Collections.ObjectModel;

namespace SnapDotNet.Core.Snapchat.Models
{
	/// <summary>
	/// Represents a group of friends.
	/// </summary>
	public class FriendGroup
		: NotifyPropertyChangedBase
	{
		public string Name
		{
			get { return _name; }
			set { SetField(ref _name, value); }
		}
		private string _name;

		public ObservableCollection<Friend> Friends
		{
			get { return _friends; }
			set { SetField(ref _friends, value); }
		}
		private ObservableCollection<Friend> _friends;
	}
}
