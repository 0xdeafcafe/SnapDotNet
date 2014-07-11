using System;
using System.Windows.Input;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;
using Snapchat.Common;
using Snapchat.Dialogs;
using Snapchat.Helpers;
using Snapchat.Models;
using SnapDotNet.Core.Miscellaneous.Helpers;
using System.Collections.ObjectModel;
using SnapDotNet.Core.Miscellaneous.CustomTypes;
using System.Globalization;

namespace Snapchat.ViewModels.PageContents
{
	public class FriendsViewModel
		 : BaseViewModel
	{
		public FriendsViewModel()
		{
			ChangeFriendDisplayNameCommand = new RelayCommand<Friend>(ChangeFriendDisplayName);
		}

		public string FilterText
		{
			get { return _filterText; }
			set
			{
				TryChangeValue(ref _filterText, value);
				ExplicitOnNotifyPropertyChanged("FilteredSortedFriends");
			}
		}
		private string _filterText;

		public ObservableCollection<AlphaKeyGroup<Friend>> FilteredSortedFriends
		{
			get
			{
				if (string.IsNullOrWhiteSpace(FilterText))
					return Account.SortedFriends;

				var filteredFriends = new Collection<Friend>();
				foreach (var friend in App.SnapchatManager.Account.Friends)
				{
					if (friend.FriendlyName.ToLowerInvariant().Contains(FilterText.ToLowerInvariant()))
						filteredFriends.Add(friend);
				}
				return AlphaKeyGroup<Friend>.CreateGroups(filteredFriends, new CultureInfo("en"), f => f.FriendlyName, true, false);
			}
		}

		public ICommand ChangeFriendDisplayNameCommand
		{
			get { return _changeFriendDisplayNameCommand; }
			set { TryChangeValue(ref _changeFriendDisplayNameCommand, value); }
		}
		private ICommand _changeFriendDisplayNameCommand;

		private static async void ChangeFriendDisplayName(Friend friend)
		{
			var contentDialog = new ChangeFriendDisplayNameDialog(friend.FriendlyName);
			var result = await contentDialog.ShowAsync();
			if (result != ContentDialogResult.Primary) return;
			var oldDisplayName = friend.Display;
			friend.Display = contentDialog.NewDisplayName;

			await ProgressHelper.ShowStatusBarAsync(App.Strings.GetString("StatusBarChangeDisplay"));
			try
			{
				await App.SnapchatManager.Endpoints.ChangeFriendDisplayNameAsync(friend.Name, contentDialog.NewDisplayName);
			}
			catch (Exception ex)
			{
				SnazzyDebug.WriteLine(ex);
				friend.Display = oldDisplayName;

				new MessageDialog(String.Format(App.Strings.GetString("ChangeFriendDisplayNameErrorContent"), friend.FriendlyName),
					App.Strings.GetString("ChangeFriendDisplayNameErrorTitle")).ShowAsync();
			}

			await ProgressHelper.HideStatusBarAsync();
		}
	}
}
