using System;
using SnapDotNet.Core.Miscellaneous.Models;
using SnapDotNet.Core.Snapchat.Models.New;

namespace SnapDotNet.Core.Snapchat.Models.AppSpecific
{
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

	public class SelectedStory
		: SelectedItem
	{
		public String StoryName
		{
			get { return _storyName; }
			set { SetField(ref _storyName, value); }
		}
		private String _storyName;
	}

	public class SelectedRecent
		: SelectedItem
	{
		public String RecentName
		{
			get { return _recentName; }
			set { SetField(ref _recentName, value); }
		}
		private String _recentName;
	}
}
