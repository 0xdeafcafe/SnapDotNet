using System;
using System.Windows.Input;
using Windows.System.Threading;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Snapchat.Common;

namespace Snapchat.ViewModels
{
	public sealed class MainViewModel
		: BaseViewModel
	{
		public MainViewModel(ScrollViewer scrollViewer)
		{
			PageScrollViewer = scrollViewer;

			GoToConversationsCommand = new RelayCommand(GoToConversations);
			GoToFriendsCommand = new RelayCommand(GoToFriends);
		}

		public double ActualWidth { get; set; }

		public ScrollViewer PageScrollViewer { get; private set; }

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
			ScrollToPage(Page.Conversation);
		}
		private void GoToFriends()
		{
			ScrollToPage(Page.ManageFriends);
		}

		public void ScrollToPage(Page page)
		{
			ThreadPoolTimer.CreateTimer(async source =>
			{
				await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
					() =>
					{
						double width;
						switch (page)
						{
							case Page.Conversation:
								width = ActualWidth * 0;
								break;

							default:
							case Page.Camera:
								width = ActualWidth * 1;
								break;

							case Page.Stories:
								width = ActualWidth * 2;
								break;

							case Page.ManageFriends:
								width = ActualWidth * 3;
								break;
						}
						PageScrollViewer.ChangeView(width, null, null, false);
					});
			},
			TimeSpan.FromMilliseconds(25));
		}
	}

	public enum Page
	{
		Conversation,
		Camera,
		Stories,
		ManageFriends
	}
}
