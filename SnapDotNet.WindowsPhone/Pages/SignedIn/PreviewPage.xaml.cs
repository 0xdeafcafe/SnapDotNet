using System.Diagnostics;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556
using SnapDotNet.Apps.Attributes;

namespace SnapDotNet.Apps.Pages.SignedIn
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	[RequiresAuthentication]
	public sealed partial class PreviewPage
	{
		public PreviewPage()
		{
			InitializeComponent();
		}

		/// <summary>
		/// Invoked when this page is about to be displayed in a Frame.
		/// </summary>
		/// <param name="e">Event data that describes how this page was reached.
		/// This parameter is typically used to configure the page.</param>
		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			dynamic d = e.Parameter;
			InMemoryRandomAccessStream stream = d.Stream;
			bool isPhoto = d.IsPhoto;

			Debug.WriteLine("Navigated to PhotoPreview, caught stream of size {0}", stream.Size);

			//if IsPhoto = x put stream into and show relevant control, etc
			// <-implement->

			//var bmpimg = new BitmapImage();
			//bmpimg.ImageFailed += bmpimg_ImageFailed;
			//bmpimg.SetSource(stream);
			//ImageReview.Source = bmpimg;
		}

		void bmpimg_ImageFailed(object sender, ExceptionRoutedEventArgs e)
		{
			Debug.WriteLine("Image Decoding Failed");
		}
	}
}
