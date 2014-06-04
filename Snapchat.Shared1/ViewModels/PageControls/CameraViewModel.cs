using System.Windows.Input;
using Snapchat.Common;

namespace Snapchat.ViewModels.PageControls
{
	public class CameraViewModel
		: BaseViewModel
	{
		private readonly MainViewModel _mainViewModel;
		private readonly int _actualWidth;

		public CameraViewModel(MainViewModel mainViewModel, int actualWidth)
		{
			_mainViewModel = mainViewModel;
			_actualWidth = actualWidth;

			GoToConversationsCommand = new RelayCommand(GoToConversations);
		}

		/// <summary>
		/// Gets the command to slide to messages
		/// </summary>
		public ICommand GoToConversationsCommand
		{
			get { return _goToConversationsCommand; }
			private set { TryChangeValue(ref _goToConversationsCommand, value); }
		}
		private ICommand _goToConversationsCommand;

		private void GoToConversations()
		{
			_mainViewModel.ScrollToPage(Page.Conversation, _actualWidth);
		}
	}
}
