using Microsoft.Xaml.Interactivity;
using System.Linq;
using Windows.Phone.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
using Snapchat.Helpers;

namespace Snapchat.Pages
{
    public sealed partial class StartPage : Page
    {
		public StartPage()
        {
            InitializeComponent();
            NavigationCacheMode = NavigationCacheMode.Required;
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
			// Exit the app if it's at the start screen.
			if (VisualStateManager.GetVisualStateGroups(PageContainer).First().CurrentState.Name == "Start")
				return;

			// Otherwise, go back to the start screen.
			VisualStateManager.GoToState(VisualStateUtilities.FindNearestStatefulControl(PageContainer), "Start", false);
			e.Handled = true;
		}

		private void LogInPageEmailUsernameTextBox_KeyDown(object sender, KeyRoutedEventArgs e)
	    {
			if (e.Key == Windows.System.VirtualKey.Enter)
				LogInPagePasswordBox.Focus(FocusState.Keyboard);
	    }

	    private void LogInPagePasswordBox_KeyDown(object sender, KeyRoutedEventArgs e)
	    {
		    if (e.Key == Windows.System.VirtualKey.Enter)
		    {
				LogInPageLoginButton.Focus(FocusState.Programmatic);

			    // TODO: Invoke Sign In command in VM
		    }
	    }

		private void SignUpPageEmailTextBox_KeyDown(object sender, KeyRoutedEventArgs e)
		{
			if (e.Key == Windows.System.VirtualKey.Enter)
				SignUpPagePasswordBox.Focus(FocusState.Keyboard);
		}

		private void SignUpPagePasswordBox_KeyDown(object sender, KeyRoutedEventArgs e)
		{
			if (e.Key == Windows.System.VirtualKey.Enter)
				SignUpPageBirthdayDatePicker.Focus(FocusState.Programmatic);
		}
    }
}
