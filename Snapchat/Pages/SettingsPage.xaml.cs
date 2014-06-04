using Windows.Phone.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Snapchat.Pages
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class SettingsPage : Page
	{
		public SettingsPage()
		{
			InitializeComponent();
		}

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			HardwareButtons.BackPressed += HardwareButtons_BackPressed;
		}

		protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
		{
			HardwareButtons.BackPressed -= HardwareButtons_BackPressed;
		}

		private void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
		{
			(Window.Current.Content as Frame).GoBack();
			e.Handled = true;
		}
	}
}
