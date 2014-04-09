using SnapDotNet.Apps.Attributes;
using SnapDotNet.Apps.Common;
using SnapDotNet.Core.Snapchat.Models;
using System;
using System.Collections.ObjectModel;

namespace SnapDotNet.Apps.ViewModels
{
    public sealed class MainViewModel
		: NotifyPropertyChangedBase
	{
#if NETFX_CORE
		public const int MaximumRecentSnaps = 7;
#else
		public const int MaximumRecentSnaps = 10;
#endif

		[RequiresAuth]
		public MainViewModel()
		{
			RecentSnaps = new ObservableCollection<Snap>();

#if DEBUG
			var names = new[]{
				"alexerax",
				"Matt Saville",
				"collindaginger"
			};
			var random = new Random();
			for (var i = 0; i < MaximumRecentSnaps; i++)
			{
				var status = (SnapStatus)random.Next(5);

				RecentSnaps.Add(new Snap
				{
					RemainingSeconds = random.Next(9) + 1,
					ScreenName = names[random.Next(names.Length)],
					Status = status,
					Timestamp = DateTime.Now,
				});
			}
#endif
		}

		/// <summary>
		/// Gets or sets a collection of recent snaps.
		/// </summary>
		public ObservableCollection<Snap> RecentSnaps
		{
			get { return _recentSnaps; }
			set { SetField(ref _recentSnaps, value); }
		}
		private ObservableCollection<Snap> _recentSnaps;

		/// <summary>
		/// Gets or sets a collection of recent story updates posted by friends.
		/// </summary>
		public ObservableCollection<Stories.FriendStory> RecentFriendStories
		{
			get { return _recentFriendStories; }
			set { SetField(ref _recentFriendStories, value); }
		}
		private ObservableCollection<Stories.FriendStory> _recentFriendStories;
    }
}
