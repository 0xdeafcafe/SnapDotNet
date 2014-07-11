using System;
using SnapDotNet.Core.Miscellaneous.Models;

namespace Snapchat.Models
{
	public enum OtherType
	{
		Recent,
		BestFriend,
		Story
	}

	public enum SelectionType
	{
		Stories,
		BestFriends,
		Recents,
		Friends
	}

	public class SelectedItem
		: NotifyPropertyChangedBase
	{
		public Boolean Selected
		{
			get { return _selected; }
			set { SetField(ref _selected, value); }
		}
		private Boolean _selected;
	}

	public class SelectedFriend
		: SelectedItem
	{
		public Friend Friend
		{
			get { return _friend; }
			set { SetField(ref _friend, value); }
		}
		private Friend _friend;
	}
	
	public class SelectedOther
		: SelectedItem
	{
		public String OtherName
		{
			get { return _otherName; }
			set { SetField(ref _otherName, value); }
		}
		private String _otherName;

		public OtherType OtherType
		{
			get { return _otherType; }
			set { SetField(ref _otherType, value); }
		}
		private OtherType _otherType;
	}
}
