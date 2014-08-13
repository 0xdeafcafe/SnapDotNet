using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Snapchat.Models;
using SnapDotNet.Core.Miscellaneous.Extensions;

namespace Snapchat.Pages.PageContents
{
	public sealed partial class ConversationPageContent
	{
		public ConversationPageContent()
		{
			InitializeComponent();
		}

		public void Load()
		{
			var timer = new DispatcherTimer { Interval = new TimeSpan(0, 0, 0, 0, 200) };
			timer.Tick += delegate
			{
				timer.Stop();
				ScrollViewer.ChangeView(0, ScrollViewer.ScrollableHeight, 1.0f);
			};
			timer.Start();
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

			var isSaved = messageContainer.ChatMessage.LocalSavedState.Saved;

			if (isSaved)
			{
				messageContainer.ChatMessage.LocalSavedState.Saved = false;
				messageContainer.ChatMessage.LocalSavedState.Version++;
				// fire event
			}
			else
			{
				messageContainer.ChatMessage.LocalSavedState.Saved = true;
				messageContainer.ChatMessage.LocalSavedState.Version++;
				// fire event
			}
			messageContainer.ChatMessage.NotifyPropertyChanged("SavedState");
		}
	}
}
