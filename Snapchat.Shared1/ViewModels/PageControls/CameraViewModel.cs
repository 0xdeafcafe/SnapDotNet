using System.Windows.Input;
using Snapchat.Common;

namespace Snapchat.ViewModels.PageControls
{
	public class CameraViewModel
		: BaseViewModel
	{
		private readonly MainViewModel _mainViewModel;

		public CameraViewModel(MainViewModel mainViewModel)
		{
			_mainViewModel = mainViewModel;

			GoToConversationsCommand = new RelayCommand(GoToConversations);
			GoToFriendsCommand = new RelayCommand(GoToFriends);
		}

		public int ActualWidth { get; set; }

		/// <summary>
		/// Gets the command to slide to messages
		/// </summary>
		public ICommand GoToConversationsCommand
		{
			get { return _goToConversationsCommand; }
			private set { TryChangeValue(ref _goToConversationsCommand, value); }
		}
		private ICommand _goToConversationsCommand;

		/// <summary>
		/// Gets the command to slide to friends
		/// </summary>
		public ICommand GoToFriendsCommand
		{
			get { return _goToFriendsCommand; }
			private set { TryChangeValue(ref _goToFriendsCommand, value); }
		}
		private ICommand _goToFriendsCommand;

		private void GoToConversations()
		{
			_mainViewModel.ScrollToPage(Page.Conversation, ActualWidth);
		}

		private void GoToFriends()
		{
			_mainViewModel.ScrollToPage(Page.ManageFriends, ActualWidth);
		}
	}
}
