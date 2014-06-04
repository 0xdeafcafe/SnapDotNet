

using Windows.UI.Xaml.Controls;

namespace Snapchat.ViewModels
{
	public sealed class MainViewModel
		: BaseViewModel
	{
		public MainViewModel(ScrollViewer scrollViewer)
		{
			PageScrollViewer = scrollViewer;
		}

		public ScrollViewer PageScrollViewer { get; private set; }

		public void ScrollToPage(Page page, int actualWidth)
		{
			switch (page)
			{
				case Page.Conversation:
					PageScrollViewer.ChangeView(actualWidth * 0, null, null, false);
					break;

				case Page.Camera:
					PageScrollViewer.ChangeView(actualWidth * 1, null, null, false);
					break;

				case Page.Stories:
					PageScrollViewer.ChangeView(actualWidth * 2, null, null, false);
					break;

				case Page.ManageFriends:
					PageScrollViewer.ChangeView(actualWidth * 3, null, null, false);
					break;
			}
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
