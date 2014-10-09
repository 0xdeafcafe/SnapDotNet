using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using ColdSnap.Common;
using ColdSnap.Helpers;
using ColdSnap.Pages;
using SnapDotNet;
using SnapDotNet.Data;
using SnapDotNet.Utilities;

namespace ColdSnap.ViewModels
{
    public sealed class MainPageViewModel
		: BaseViewModel
    {
		private DispatcherTimer _totalSecondsElapsedTimer;

		public MainPageViewModel()
		{
			RefreshCommand = new RelayCommand(RefreshContentAsync);
			GoToSettingsCommand = new RelayCommand(() => Window.Current.Navigate(typeof(SettingsPage), Account));
			GoToCameraCommand = new RelayCommand(() => Window.Current.Navigate(typeof(CameraPage), Account));
			GoToManageFriendsCommand = new RelayCommand(() => Window.Current.Navigate(typeof(ManageFriendsPage), Account));
			RenameFriendCommand = new RelayCommand<Friend>(ChangeDisplayNameAsync);
			RemoveFriendCommand = new RelayCommand<Friend>(RemoveFriendAsync);
			BlockFriendCommand = new RelayCommand<Friend>(BlockFriendAsync);
			UnblockFriendCommand = new RelayCommand<Friend>(UnblockFriendAsync);

			ActiveStories = new ObservableCollection<Story>();
		}

		#region Commands

		public ICommand RefreshCommand
		{
			get { return _refreshCommand; }
			private set { SetValue(ref _refreshCommand, value); }
		}
		private ICommand _refreshCommand;

		public ICommand GoToSettingsCommand
		{
			get { return _goToSettingsCommand; }
			private set { SetValue(ref _goToSettingsCommand, value); }
		}
		private ICommand _goToSettingsCommand;

		public ICommand GoToCameraCommand
		{
			get { return _goToCameraCommand; }
			private set { SetValue(ref _goToCameraCommand, value); }
		}
		private ICommand _goToCameraCommand;

		public ICommand GoToManageFriendsCommand
		{
			get { return _goToManageFriendsCommand; }
			private set { SetValue(ref _goToManageFriendsCommand, value); }
		}
		private ICommand _goToManageFriendsCommand;

		public ICommand RenameFriendCommand
		{
			get { return _renameFriendCommand; }
			private set { SetValue(ref _renameFriendCommand, value); }
		}
	    private ICommand _renameFriendCommand;

		public ICommand RemoveFriendCommand
		{
			get { return _removeFriendCommand; }
			private set { SetValue(ref _removeFriendCommand, value); }
		}
		private ICommand _removeFriendCommand;

		public ICommand BlockFriendCommand
		{
			get { return _blockFriendCommand; }
			private set { SetValue(ref _blockFriendCommand, value); }
		}
		private ICommand _blockFriendCommand;

		public ICommand UnblockFriendCommand
		{
			get { return _unblockFriendCommand; }
			private set { SetValue(ref _unblockFriendCommand, value); }
		}
		private ICommand _unblockFriendCommand;

		#endregion

		public ObservableCollection<FriendsKeyGroup> SortedFriends
		{
			get { return _sortedFriends; }
			private set { SetValue(ref _sortedFriends, value); }
		}
	    private ObservableCollection<FriendsKeyGroup> _sortedFriends;

	    public ObservableCollection<Story> ActiveStories
	    {
		    get { return _activeStories; }
		    private set
		    {
			    SetValue(ref _activeStories, value); 
			    OnPropertyChanged(() => ActiveStory);
		    }
	    }
	    private ObservableCollection<Story> _activeStories;

	    public Story ActiveStory
	    {
			get { return ActiveStories.FirstOrDefault(); }
	    }

	    public int ActiveStoryTotalSecondsRemaining
	    {
			get { return _activeStoryTotalSecondsRemaining; }
			private set { SetValue(ref _activeStoryTotalSecondsRemaining, value); }
	    }
	    private int _activeStoryTotalSecondsRemaining;

		public int ActiveStorySecondsRemaining
		{
			get { return _activeStorySecondsRemaining; }
			private set { SetValue(ref _activeStorySecondsRemaining, value); }
		}
		private int _activeStorySecondsRemaining;

	    public double ActiveStoryPercentageLeft
	    {
			get { return _percentageLeft; }
			private set { SetValue(ref _percentageLeft, value); }
	    }
	    private double _percentageLeft; 

		public void HideStories()
	    {
			if (ActiveStories == null || !ActiveStories.Any())
				return;

			// TODO: ActiveStory.Dispose(); (may not be necessary)
		    ActiveStories.Clear();
			OnPropertyChanged(() => ActiveStory);
			OnPropertyChanged(() => ActiveStories);

			if (_totalSecondsElapsedTimer != null)
			{
				_totalSecondsElapsedTimer.Stop();
				_totalSecondsElapsedTimer = null;
			}
	    }

	    public void ShowStory(Friend friend)
	    {
		    if (!friend.Stories.Any())
				return;

		    if (_totalSecondsElapsedTimer != null)
			    _totalSecondsElapsedTimer.Stop();

		    ActiveStories.Clear();
		    foreach (var story in friend.Stories.Where(s => s.IsCached && !s.IsExpired))
			    ActiveStories.Add(story);
			OnPropertyChanged(() => ActiveStory);

		    if (ActiveStory == null)
			    return;

			// Calculate total seconds remaining.
		    ActiveStoryTotalSecondsRemaining = ActiveStories.Sum(s => s.Duration);
		    ActiveStorySecondsRemaining = ActiveStory.Duration;
		    ActiveStoryPercentageLeft = 100;

			// Start countdown timer.
		    _totalSecondsElapsedTimer = new DispatcherTimer {Interval = new TimeSpan(0, 0, 0, 1)};
		    _totalSecondsElapsedTimer.Tick += delegate
		    {
			    if (ActiveStorySecondsRemaining > 1)
			    {
				    ActiveStoryTotalSecondsRemaining--;
					ActiveStorySecondsRemaining--;
			    }
			    else
			    {
				    ProgressToNextStory();
				    ActiveStorySecondsRemaining = ActiveStory != null ? ActiveStory.Duration : 0;
					ActiveStoryPercentageLeft = 100;
			    }

			    if (ActiveStory == null) return;
			    ActiveStoryPercentageLeft = (ActiveStorySecondsRemaining/ActiveStory.Duration)*100;
		    };
		    _totalSecondsElapsedTimer.Start();
	    }

	    public void ProgressToNextStory()
	    {
		    if (ActiveStory == null)
				return;

		    // TODO: ActiveStory.Dispose(); (may not be necessary)
		    ActiveStories.RemoveAt(0);
			OnPropertyChanged(() => ActiveStory);

			// TODO: InitializeStory?
	    }

		/// <summary>
		/// Retrieves the latest data from Snapchat and caches it. If no longer authorized, go back
		/// to StartPage.
		/// </summary>
		private async void RefreshContentAsync()
	    {
			await StatusBarHelper.ShowStatusBarAsync(App.Strings.GetString("StatusBarUpdating"));

			// Try to update account.
		    bool notAuthenticated = false;
		    try
		    {
			    await Account.UpdateAccountAsync();
		    }
		    catch (InvalidCredentialsException)
		    {
			    notAuthenticated = true;
		    }

		    if (notAuthenticated)
		    {
				// Delete cache and go back to StartPage.
			    await StorageManager.Local.EmptyFolderAsync();
			    Window.Current.Navigate(typeof (StartPage));
		    }
		    else
		    {
				// Created a sorted list of friends.
				if (SortedFriends == null)
					await CreateSortedFriendsAsync();

				// Download story thumbnails.
			    await Account.DownloadStoryThumbnailsAsync();

			    // TODO: Save account state
		    }

		    await StatusBarHelper.HideStatusBarAsync();
	    }

		private async Task CreateSortedFriendsAsync()
		{
			await Window.Current.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, delegate
			{
				SortedFriends = FriendsKeyGroup.CreateGroups(Account.Friends, Account.Username);

				var storiesGroup = new FriendsKeyGroup(null, null, Colors.Black, Colors.Transparent);
				storiesGroup.Add(Account.Me);
				SortedFriends.Insert(0, storiesGroup);
			});
		}

		private async void ChangeDisplayNameAsync(Friend friend)
	    {
		    // TODO: to be implemented
			//await (new MessageDialog(App.Strings.GetString("FailedToRenameFriendErrorMessage"))).ShowAsync();
	    }

		private async void RemoveFriendAsync(Friend friend)
		{
			if (await friend.DeleteAsync())
			{
				// TODO: Save state
				RemoveSortedFriend(friend);
			}
		}

		private async void BlockFriendAsync(Friend friend)
		{
			if (await friend.BlockAsync())
			{
				// TODO: Save state
				RemoveSortedFriend(friend);
			}
			else
			{
				await (new MessageDialog(App.Strings.GetString("FailedToBlockFriendErrorMessage"))).ShowAsync();
			}
		}

		private async void UnblockFriendAsync(Friend friend)
		{
			if (await friend.UnblockAsync())
			{
				// TODO: Save state
				await CreateSortedFriendsAsync();
			}
			else
			{
				await (new MessageDialog(App.Strings.GetString("FailedToUnblockFriendErrorMessage"))).ShowAsync();
			}
		}

	    private void RemoveSortedFriend(Friend friend)
	    {
			if (SortedFriends == null) return;
			foreach (var f in SortedFriends)
				f.Remove(friend);
	    }
    }

	#region Friend grouping

	public interface IAlphaKeyGroup<T> : IList<T>
	{
		string Key { get; }
		string FriendlyKey { get; }
		Color BackgroundColor { get; }
		Color ForegroundColor { get; }
	}

	public class FriendsKeyGroup
		: ObservableCollection<Friend>, IAlphaKeyGroup<Friend>
	{
		public FriendsKeyGroup() { }

		public FriendsKeyGroup(string key, string friendlyKey, Color foregroundColor, Color backgroundColor)
		{
			Key = key;
			FriendlyKey = friendlyKey;
			ForegroundColor = foregroundColor;
			BackgroundColor = backgroundColor;
		}

		#region IAlphaKeyGroup<Friend> Members

		public Color BackgroundColor { get; private set; }
		public Color ForegroundColor { get; private set; }
		public string Key { get; private set; }
		public string FriendlyKey { get; private set; }

		#endregion

		public static ObservableCollection<FriendsKeyGroup> CreateGroups(ICollection<Friend> items, string userToIgnore)
		{
			var list = CreateGroups();

			InsertEntries(list, items, userToIgnore);

			//foreach (var group in list)
			//	group.OrderByDescending(g => g.FriendlyName[0]);

			return new ObservableCollection<FriendsKeyGroup>(list);
		}

		private static ObservableCollection<FriendsKeyGroup> CreateGroups()
		{
			const string keys = "abcdefghijklmnopqrstuvwxyz#";

			var list = keys.Select(
				key =>
					new FriendsKeyGroup(key.ToString(), key.ToString().ToUpperInvariant(),
						Color.FromArgb(0xFF, 0x9B, 0x55, 0xA0),
						Color.FromArgb(0x00, 0x00, 0x00, 0x00))).ToList();

			/*list.Add(new FriendsKeyGroup("!",
				"BLOCKED",
				Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF),
				Color.FromArgb(0xFF, 0xE9, 0x27, 0x54)));*/

			return new ObservableCollection<FriendsKeyGroup>(list);
		}

		public static void InsertEntries(ObservableCollection<FriendsKeyGroup> groups, IEnumerable<Friend> items, string userToIgnore)
		{
			Debug.WriteLine("[FriendsKeyGroup] Starting to Insert Items, based off of {0} friends", items.Count());

			foreach (var friend in items.Where(friend => friend.Username != userToIgnore))
			{
				if (friend.FriendRequestState == FriendRequestState.Blocked)
				{
					//groups.FirstOrDefault(a => a.Key == "!").Add(friend);
					//Debug.WriteLine("[FriendsKeyGroup] Added friend {0} to Blocked", friend.FriendlyName);
				}
				else
				{
					var key = friend.FriendlyName.ToUpperInvariant()[0];
					if (char.IsLetter(key))
					{
						groups.FirstOrDefault(a => a.Key == key.ToString().ToLowerInvariant()).Add(friend);
						Debug.WriteLine("[FriendsKeyGroup] Added friend {0} with key {1} to {2}", friend.FriendlyName, key, key);
					}
					else
					{
						groups.FirstOrDefault(a => a.Key == "#").Add(friend);
						Debug.WriteLine("[FriendsKeyGroup] Added friend {0} with key {1} to {2}", friend.FriendlyName, key, "numbers");
					}
				}
			}
		}

		public static void RemoveEntries(ObservableCollection<FriendsKeyGroup> groups, IEnumerable<Friend> items)
		{
			Debug.WriteLine("[FriendsKeyGroup] Starting to Remvoing Items, based off of {0} friends", items.Count());

			foreach (var friend in items)
			{
				foreach (var @group in groups.Where(@group => @group.Contains(friend)))
				{
					@group.Remove(friend);
					Debug.WriteLine("[FriendsKeyGroup] Removing friend {0} from {1}", friend.FriendlyName, @group.FriendlyKey);
				}
			}
		}
	}

	#endregion
}
