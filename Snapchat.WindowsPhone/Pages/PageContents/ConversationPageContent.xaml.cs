using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using SnapDotNet.Core.Miscellaneous.Extensions;
using SnapDotNet.Core.Snapchat.Models.New;

namespace Snapchat.Pages.PageContents
{
	public sealed partial class ConversationPageContent
	{
		public ConversationPageContent()
		{
			InitializeComponent();

			Loaded += delegate
			{
				// TODO: Fix this?
				//ScrollViewer.ScrollToVerticalOffset(ScrollViewer.ActualHeight);
			};
		}

		private async void ChatMediaImage_Loaded(object sender, RoutedEventArgs e)
		{
			var imageElement = sender as Image;
			if (imageElement == null) return;
			var chatMessage = imageElement.DataContext as ChatMessage;
			if (chatMessage == null) return;
			var media = chatMessage.Body.Media;
			if (!media.HasMedia) await media.DownloadSnapBlobAsync(App.SnapchatManager);
			var imageData = await media.OpenSnapBlobAsync();
			if (imageData == null)
			{
				// Connection is down or something bad happened, display error
				
			}
			else
			{
				imageElement.Source = imageData.ToBitmapImage();
			}
		}

		private void ChatContent_OnTapped(object sender, TappedRoutedEventArgs e)
		{
			var frameworkElement = sender as FrameworkElement;
			if (frameworkElement == null) return;
			var messageContainer = frameworkElement.DataContext as MessageContainer;
			if (messageContainer == null) return;

			var containsKey = messageContainer.ChatMessage.SavedStates.ContainsKey(App.SnapchatManager.Username);
			var isSaved = containsKey && messageContainer.ChatMessage.SavedStates[App.SnapchatManager.Username].Saved;

			if (isSaved)
			{
				messageContainer.ChatMessage.SavedStates[App.SnapchatManager.Username].Saved = false;
				messageContainer.ChatMessage.SavedStates[App.SnapchatManager.Username].Version++;
				// fire event
			}
			else
			{
				if (!containsKey)
					messageContainer.ChatMessage.SavedStates.Add(App.SnapchatManager.Username, new SavedState { Version = 0 });
				messageContainer.ChatMessage.SavedStates[App.SnapchatManager.Username].Saved = true;
				messageContainer.ChatMessage.SavedStates[App.SnapchatManager.Username].Version++;
				// fire event
			}
			messageContainer.ChatMessage.NotifyPropertyChanged("SavedState");
		}
	}
}
