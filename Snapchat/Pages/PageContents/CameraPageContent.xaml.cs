using Windows.ApplicationModel;
using Snapchat.Helpers;
using Snapchat.ViewModels;
using Snapchat.ViewModels.PageControls;

namespace Snapchat.Pages.PageContents
{
	public sealed partial class CameraPageContent
	{
		public CameraViewModel ViewModel { get; private set; }

		/// <summary>
		/// Only use this in the designer
		/// </summary>
		public CameraPageContent() {  }

		public CameraPageContent(MainViewModel viewModel)
		{
			InitializeComponent();
			DataContext = ViewModel = new CameraViewModel(viewModel);
			Loaded += async delegate
			{
				if (!DesignMode.DesignModeEnabled)
					await MediaCaptureManager.StartPreviewAsync(CapturePreview);

				ViewModel.ActualWidth = (int) ActualWidth;
			};
		}
	}
}
