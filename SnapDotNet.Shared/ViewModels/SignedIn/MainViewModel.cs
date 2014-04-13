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
			RecentSnaps = new ObservableCollection<Snap>();

			#region Commands

			SignOutCommand = new RelayCommand(async () =>
			{
				try
				{
					await Manager.Endpoints.LogoutAsync();
				}
				catch (InvalidHttpResponseException)
				{
					// o well
				}
				catch (InvalidCredentialsException)
				{
					// o well too
				}

				App.CurrentFrame.Navigate(typeof(StartPage));
			});

			ViewSnapsCommand = new RelayCommand(() => App.CurrentFrame.Navigate(typeof (SnapsPage)));

			#endregion

			RecentSnaps = new ObservableCollection<Snap>(App.SnapChatManager.Account.Snaps.Take(MaximumRecentSnaps));
			GetFriends();

			App.SnapChatManager.PropertyChanged += delegate
			{
				RecentSnaps = new ObservableCollection<Snap>(App.SnapChatManager.Account.Snaps.Take(MaximumRecentSnaps));
				GetFriends();
			};
			App.SnapChatManager.Account.PropertyChanged += delegate
			{
				RecentSnaps = new ObservableCollection<Snap>(App.SnapChatManager.Account.Snaps.Take(MaximumRecentSnaps));
				GetFriends();
			};

			App.SnapChatManager.Account.Snaps.CollectionChanged += delegate
			{
				RecentSnaps = new ObservableCollection<Snap>(App.SnapChatManager.Account.Snaps.Take(MaximumRecentSnaps));
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

		public ObservableCollection<string> BestFriends
		{
			get { return _bestFriends; }
			set { SetField(ref _bestFriends, value); }
		}
		private ObservableCollection<string> _bestFriends;

		public ObservableCollection<string> Friends
		{
			get { return _friends; }
			set { SetField(ref _friends, value); }
		}
		private ObservableCollection<string> _friends;

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
				Friends = new ObservableCollection<string>();
				BestFriends = new ObservableCollection<string>(App.SnapChatManager.Account.BestFriends);
				while (BestFriends.Count > MaximumFriendRows)
					BestFriends.RemoveAt(BestFriends.Count - 1);

				// TODO: Display recently interacted friends instead

				SortedSet<string> friends = new SortedSet<string>();
				foreach (var friend in App.SnapChatManager.Account.Friends)
				{
					if (!Friends.Contains(friend.Name) && friends.Count < (MaximumFriendRows - BestFriends.Count))
					{
						friends.Add(friend.FriendlyName);
					}
				}

				foreach (string name in friends)
					Friends.Add(name);
			}
			else
			{
				// search friends
			}
		}
    }
}
