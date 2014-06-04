using Windows.ApplicationModel;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236
using Snapchat.Helpers;

namespace Snapchat.Pages.PageContents
{
	public sealed partial class CameraPageContent : UserControl
	{
		public CameraPageContent()
		{
			this.InitializeComponent();
			Loaded += async delegate
			{
				if (!DesignMode.DesignModeEnabled)
					await MediaCaptureManager.StartPreviewAsync(CapturePreview);
			};
		}
	}
}
