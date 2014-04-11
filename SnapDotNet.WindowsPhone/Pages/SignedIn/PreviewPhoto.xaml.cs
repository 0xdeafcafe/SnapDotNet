using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Imaging;
using Windows.Media.MediaProperties;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace SnapDotNet.Apps.Pages.SignedIn
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class PreviewPhoto : Page
	{
		public PreviewPhoto()
		{
			this.InitializeComponent();
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
