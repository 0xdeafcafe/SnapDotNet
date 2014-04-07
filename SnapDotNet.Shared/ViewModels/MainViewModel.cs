using SnapDotNet.Apps.Common;
using SnapDotNet.Core.Snapchat.Models;
using System;
using System.Collections.ObjectModel;
using Windows.UI.Xaml;

namespace SnapDotNet.Apps.ViewModels
{
    public sealed class MainViewModel
		: NotifyPropertyChangedBase
    {
		public MainViewModel()
		{
			RecentSnaps = new ObservableCollection<Snap>();

#if DEBUG
			string[] names = new[]{
				"alexerax",
				"Matt Saville",
				"collindaginger"
			};
			var random = new Random();
			for (int i = 0; i < 24; i++)
			{
				SnapStatus status = (SnapStatus) random.Next(5);

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

		public ObservableCollection<Snap> RecentSnaps
		{
			get { return _recentSnaps; }
			set { SetField(ref _recentSnaps, value); }
		}
		private ObservableCollection<Snap> _recentSnaps;
    }
}
