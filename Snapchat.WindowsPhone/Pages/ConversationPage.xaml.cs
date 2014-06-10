using Windows.Phone.UI.Input;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml.Navigation;
using Snapchat.Attributes;
using Snapchat.ViewModels;
using SnapDotNet.Core.Snapchat.Models.New;

namespace Snapchat.Pages
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	/// 
	[RequiresAuthentication]
	public sealed partial class ConversationPage
	{
		public ConversationViewModel ViewModel { get; private set; }

		public ConversationPage()
		{
			InitializeComponent();

			//var data = App.SnapchatManager.AllUpdates.ConversationResponse[0].ConversationMessages.SortedMessages;
		}

		/// <summary>
		/// Invoked when this page is about to be displayed in a Frame.
		/// </summary>
		/// <param name="e">Event data that describes how this page was reached.
		/// This parameter is typically used to configure the page.</param>
		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			HardwareButtons.BackPressed += HardwareButtons_BackPressed;
			DataContext = ViewModel = new ConversationViewModel((ConversationResponse) e.Parameter);
			SetStatusBar();
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

		private static void SetStatusBar()
		{
			ApplicationView.GetForCurrentView().SetDesiredBoundsMode(ApplicationViewBoundsMode.UseCoreWindow);

			var statusBar = StatusBar.GetForCurrentView();
			statusBar.BackgroundOpacity = 0.0f;
			statusBar.BackgroundColor = Colors.Transparent;
			statusBar.ForegroundColor = Colors.White;
		}
	}
}
