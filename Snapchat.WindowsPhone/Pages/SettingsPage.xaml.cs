using Windows.Phone.UI.Input;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Snapchat.Attributes;

namespace Snapchat.Pages
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	[RequiresAuthentication]
	public sealed partial class SettingsPage : Page
	{
		public SettingsPage()
		{
			InitializeComponent();
		}

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			HardwareButtons.BackPressed += HardwareButtons_BackPressed;
			HardwareButtons.CameraPressed += HardwareButtons_CameraPressed;
		}

		protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
		{
			HardwareButtons.BackPressed -= HardwareButtons_BackPressed;
			HardwareButtons.CameraPressed -= HardwareButtons_CameraPressed;
		}

		private void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
		{
			App.PreviousPage = typeof(SettingsPage);
			App.RootFrame.GoBack();
			e.Handled = true;
		}

		private void HardwareButtons_CameraPressed(object sender, CameraEventArgs e)
		{
			App.PreviousPage = typeof(SettingsPage);
			App.RootFrame.GoBack();
		}
	}
}
