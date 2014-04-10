using System;
using System.Collections.ObjectModel;
using SnapDotNet.Core.Snapchat.Models;

namespace SnapDotNet.Apps.ViewModels.SignedIn
{
    public sealed class MainViewModel
		: ViewModelBase
	{
		public const int MaximumRecentSnaps = 7;

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
		public ObservableCollection<FriendStory> RecentFriendStories
		{
			get { return _recentFriendStories; }
			set { SetField(ref _recentFriendStories, value); }
		}
		private ObservableCollection<FriendStory> _recentFriendStories;
    }
}
