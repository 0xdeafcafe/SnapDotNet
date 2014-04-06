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
			Snaps = new ObservableCollection<Snap>();

#if DEBUG
			var random = new Random();
			for (int i = 0; i < 13; i++)
			{
				Snaps.Add(new Snap { RemainingSeconds = random.Next(10) });
			}
#endif

			DispatcherTimer timer = new DispatcherTimer();
			timer.Interval = TimeSpan.FromSeconds(1);
			timer.Tick += delegate
			{
				foreach (var snap in Snaps)
				{
					if (snap.RemainingSeconds != null && snap.RemainingSeconds.Value > 0)
						snap.RemainingSeconds--;
				}
			};
			timer.Start();
		}

		public ObservableCollection<Snap> Snaps
		{
			get { return _snaps; }
			set { SetField(ref _snaps, value); }
		}
		private ObservableCollection<Snap> _snaps;
    }
}
