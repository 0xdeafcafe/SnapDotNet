using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using ColdSnap.Common;
using ColdSnap.Dialogs;
using ColdSnap.Helpers;
using SnapDotNet;

namespace ColdSnap.ViewModels.Sections
{
	public class FriendsSectionViewModel
		: BaseViewModel
	{
		public FriendsSectionViewModel()
		{
			ChangeDisplayNameCommand = new RelayCommand<Friend>(ChangeDisplayName);
			BlockFriendCommand = new RelayCommand<Friend>(BlockFriend);
			UnBlockFriendCommand = new RelayCommand<Friend>(UnBlockFriend);

			_activeStories.CollectionChanged += (sender, args) => OnObservableCollectionChanged(args, "ActiveStories",() => OnPropertyChanged("ActiveStory"));
		}

		#region Properties

		public ObservableCollection<Story> ActiveStories
		{
			get { return _activeStories; }
			set { SetValue(ref _activeStories, value); }
		}
		private ObservableCollection<Story> _activeStories = new ObservableCollection<Story>();

		public Story ActiveStory
		{
			get { return ActiveStories.FirstOrDefault(); }
		}

		public int TotalSecondsRemaining
		{
			get { return _totalSecondsRemaining; }
			set { SetValue(ref _totalSecondsRemaining, value); }
		}
		private int _totalSecondsRemaining;

		public ICommand ChangeDisplayNameCommand
		{
			get { return _changeDisplayNameCommand; }
			set { SetValue(ref _changeDisplayNameCommand, value); }
		}
		private ICommand _changeDisplayNameCommand;

		public ICommand BlockFriendCommand
		{
			get { return _blockFriendCommand; }
			set { SetValue(ref _blockFriendCommand, value); }
		}
		private ICommand _blockFriendCommand;

		public ICommand UnBlockFriendCommand
		{
			get { return _unBlockFriendCommand; }
			set { SetValue(ref _unBlockFriendCommand, value); }
		}
		private ICommand _unBlockFriendCommand;

		#endregion

		#region Friend Actions

		public async void ChangeDisplayName(Friend friend)
		{
			var dialog = new ChangeDisplayNameDialog(friend.FriendlyName);
			var response = await dialog.ShowAsync();
			if (response != ContentDialogResult.Primary || dialog.NewDisplayName == null)
				return;

			await ProgressHelper.ShowStatusBarAsync(App.Strings.GetString("StatusBarChangeDisplayName"));
			var success = await friend.UpdateDisplayName(dialog.NewDisplayName, Account);
			await ProgressHelper.HideStatusBarAsync();
			if (!success)
			{
				// tell user it went tits up
				var alertDialog = new MessageDialog(App.Strings.GetString("MessageDialogChangeDisplayFailedTitle"),
					App.Strings.GetString("MessageDialogChangeDisplayFailedContent"));
				await alertDialog.ShowAsync();
			}
			else
			{
				// Save
				await StateManager.Local.SaveAccountStateAsync(Account);
			}

			Account.UpdateSortedFriends();
		}

		public async void BlockFriend(Friend friend)
		{
			await ProgressHelper.ShowStatusBarAsync(App.Strings.GetString("StatusBarBlockFriend"));
			var success = await friend.UpdateFriend(FriendAction.Block, Account);
			await ProgressHelper.HideStatusBarAsync();
			if (!success)
			{
				// tell user it went tits up
				var alertDialog = new MessageDialog(App.Strings.GetString("MessageDialogBlockFriendFailedTitle"),
					App.Strings.GetString("MessageDialogBlockFriendFailedContent"));
				await alertDialog.ShowAsync();
			}
			else
			{
				// Save
				friend.FriendRequestState = FriendRequestState.Blocked;
				await StateManager.Local.SaveAccountStateAsync(Account);
			}

			Account.UpdateSortedFriends();
		}

		public async void UnBlockFriend(Friend friend)
		{
			await ProgressHelper.ShowStatusBarAsync(App.Strings.GetString("StatusBarUnBlockFriend"));
			var success = await friend.UpdateFriend(FriendAction.Unblock, Account);
			await ProgressHelper.HideStatusBarAsync();
			if (!success)
			{
				// tell user it went tits up
				var alertDialog = new MessageDialog(App.Strings.GetString("MessageDialogUnBlockFriendFailedTitle"), 
					App.Strings.GetString("MessageDialogUnBlockFriendFailedContent"));
				await alertDialog.ShowAsync();
			}
			else
			{
				// Save
				friend.FriendRequestState = FriendRequestState.Accepted;
				await StateManager.Local.SaveAccountStateAsync(Account);
			}
			Account.UpdateSortedFriends();
		}

		#endregion

		#region Stories Logic

		private DispatcherTimer _totalSecondsElapsedTimer;

		public void PrepareStory(Friend friend)
		{
			if (!friend.Stories.Any()) return;

			if (_totalSecondsElapsedTimer != null)
				_totalSecondsElapsedTimer.Stop();

			// this way, we can keep our inpc shit xox
			ActiveStories.Clear();
			foreach (var story in friend.Stories.Where(s => s.LocalMedia && !s.Expired))
				ActiveStories.Add(story);

			// Calculate seconds
			TotalSecondsRemaining = (int) ActiveStories.Sum(s => s.SecondLength);

			// Start timer
			_totalSecondsElapsedTimer = new DispatcherTimer { Interval = new TimeSpan(0, 0, 0, 1) };
			_totalSecondsElapsedTimer.Tick += delegate
			{
				if (TotalSecondsRemaining > 1)
					TotalSecondsRemaining--;
				else
					TotalSecondsRemaining = 1;
			};
			_totalSecondsElapsedTimer.Start();

			ProgressToNextStory(true);
		}

		public void ProgressToNextStory(bool first = false)
		{
			if (!ActiveStories.Any()) return;
			if (ActiveStory == null) return;
			if (!first)
			{
				ActiveStory.DisposeStory();
				ActiveStories.RemoveAt(0);
			}
			OnPropertyChanged("ActiveStory");
			if (ActiveStory == null)
			{
				HideStories();
				return;
			}

			ActiveStory.InitalizeStory(sender => ProgressToNextStory()); // story is over! roll onto the next!
		}

		public void HideStories()
		{
			if (ActiveStories == null || !ActiveStories.Any()) return;
			ActiveStory.DisposeStory();
			ActiveStories.Clear();
			OnPropertyChanged("ActiveStory");

			_totalSecondsElapsedTimer.Stop();
			_totalSecondsElapsedTimer = null;
		}

		#endregion
	}
}
