using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using SnapDotNet.Core.Miscellaneous.Extensions;
using SnapDotNet.Core.Snapchat.Models.New;

namespace Snapchat.Pages.PageContents
{
	public sealed partial class ConversationPageContent
	{
		public ConversationPageContent()
		{
			InitializeComponent();
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
	}
}
