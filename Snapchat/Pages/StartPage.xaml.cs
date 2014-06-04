using Windows.ApplicationModel;
using Microsoft.Xaml.Interactivity;
using System.Linq;
using Windows.Phone.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;

namespace Snapchat.Pages
{
    public sealed partial class StartPage : Page
    {
		public StartPage()
        {
            InitializeComponent();
            NavigationCacheMode = NavigationCacheMode.Required;
        }

	    protected override async void OnNavigatedTo(NavigationEventArgs e)
	    {
			HardwareButtons.BackPressed += HardwareButtons_BackPressed;

			if (!App.SnapchatManager.Loaded)
				await App.SnapchatManager.LoadAsync();
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
				LogInPageLoginButton.Command.Execute(null);
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
