using System;
using System.Collections.ObjectModel;
using SnapDotNet.Apps.Pages.SignedIn;
using SnapDotNet.Core.Snapchat.Models;
using System.Windows.Input;
using SnapDotNet.Apps.Common;
using SnapDotNet.Apps.Pages;
using SnapDotNet.Core.Snapchat.Api.Exceptions;
using System.Linq;
using System.Collections.Generic;

namespace SnapDotNet.Apps.ViewModels.SignedIn
{
    public sealed class MainViewModel
		: ViewModelBase
	{
		public const int MaximumRecentSnaps = 7;
		public const int MaximumFriendRows = 11;

		public MainViewModel()
		{
			if (App.SnapchatManager.Account == null) return;

			RecentSnaps = new ObservableCollection<Snap>();

			#region Commands

			SignOutCommand = new RelayCommand(async () =>
			{
				await App.LogoutAsync();
			});

			ViewSnapsCommand = new RelayCommand(() => App.CurrentFrame.Navigate(typeof (SnapsPage)));

			#endregion

			RecentSnaps = new ObservableCollection<Snap>(App.SnapchatManager.Account.Snaps.Take(MaximumRecentSnaps));
			GetFriends();

			App.SnapchatManager.PropertyChanged += delegate
			{
				if (App.SnapchatManager.Account != null)
				{
					RecentSnaps = new ObservableCollection<Snap>(App.SnapchatManager.Account.Snaps.Take(MaximumRecentSnaps));
					GetFriends();
				}
			};
			App.SnapchatManager.Account.PropertyChanged += delegate
			{
				RecentSnaps = new ObservableCollection<Snap>(App.SnapchatManager.Account.Snaps.Take(MaximumRecentSnaps));
				GetFriends();
			};

			App.SnapchatManager.Account.Snaps.CollectionChanged += delegate
			{
				RecentSnaps = new ObservableCollection<Snap>(App.SnapchatManager.Account.Snaps.Take(MaximumRecentSnaps));
			};
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

		public ObservableCollection<object> QuickAccessItems
		{
			get { return _quickAccess; }
			set { SetField(ref _quickAccess, value); }
		}
		private ObservableCollection<object> _quickAccess;

	    public ICommand ViewSnapsCommand
	    {
		    get { return _viewSnapsCommand; }
			set { SetField(ref _viewSnapsCommand, value); }
	    }
	    private ICommand _viewSnapsCommand;

		/// <summary>
		/// Gets the command for signing out.
		/// </summary>
		public ICommand SignOutCommand
		{
			get { return _signOutCommand; }
			private set { SetField(ref _signOutCommand, value); }
		}
		private ICommand _signOutCommand;

		public string CurrentFriendSearchQuery
		{
			get { return _friendSearchQuery; }
			set { SetField(ref _friendSearchQuery, value); }
		}
		private string _friendSearchQuery;

		private void GetFriends()
		{
			if (string.IsNullOrEmpty(CurrentFriendSearchQuery))
			{
				QuickAccessItems = new ObservableCollection<object>();

				// Groups first
				// TODO: Add group support

				// Then best friends
				foreach (var bestFriend in App.SnapchatManager.Account.BestFriends)
				{
					if (QuickAccessItems.Count > MaximumFriendRows)
						break;

					foreach (var friend in App.SnapchatManager.Account.Friends)
					{
						if (friend.Name == bestFriend)
						{
							QuickAccessItems.Add(friend);
							break;
						}
					}
				}

				// Then the bottom feeders (TODO: Sort by recent interactions)
				foreach (var friend in App.SnapchatManager.Account.Friends)
				{
					if (QuickAccessItems.Count > MaximumFriendRows)
						break;

					if (!QuickAccessItems.Contains(friend))
						QuickAccessItems.Add(friend);
				}
			}
			else
			{
				// search friends
			}
		}
    }
}
