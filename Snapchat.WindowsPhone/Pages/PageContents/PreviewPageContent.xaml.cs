using Snapchat.ViewModels.PageContents;

namespace Snapchat.Pages.PageContents
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class PreviewPageContent
	{
		public PreviewPageContent()
		{
			InitializeComponent();
		}

		public void Load()
		{
			var context = DataContext as PreviewViewModel;
			if (context == null) return;

			ImageMediaElement.Source = context.WriteableBitmap;
		}
	}
}
