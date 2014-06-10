using Windows.Phone.UI.Input;
using Windows.UI.Xaml.Navigation;
using Snapchat.Attributes;

namespace Snapchat.Pages
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	/// 
	[RequiresAuthentication]
	public sealed partial class ConversationPage
	{
		public ConversationPage()
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
			HardwareButtons.BackPressed += HardwareButtons_BackPressed;
			DataContext = e.Parameter;
		}

		protected override void OnNavigatedFrom(NavigationEventArgs e)
		{
			HardwareButtons.BackPressed -= HardwareButtons_BackPressed;
		}

		private static void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
		{
			App.PreviousPage = typeof(ConversationPage);
			App.RootFrame.GoBack();
			e.Handled = true;
		}
	}
}
