using Windows.UI.Xaml.Media.Animation;
using SnapDotNet.Apps.ViewModels;
using Windows.UI.ApplicationSettings;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace SnapDotNet.Apps.Pages
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class StartPage
	{
		public StartPage()
		{
			DataContext = new StartViewModel();

			InitializeComponent();
			SizeChanged += (sender, e) => VisualStateManager.GoToState(this,
				e.NewSize.Width <= (int) Resources["MinimalViewMaxWidth"] ? "MinimalLayout" : "DefaultLayout", true);
		}

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			SettingsPane.GetForCurrentView().CommandsRequested += OnSettingsCommandsRequested;
			BackgroundAmbienceStoryboard.RepeatBehavior = new RepeatBehavior { Type = RepeatBehaviorType.Forever };
			BackgroundAmbienceStoryboard.Begin();
			base.OnNavigatedTo(e);
		}

		protected override void OnNavigatedFrom(NavigationEventArgs e)
		{
			SettingsPane.GetForCurrentView().CommandsRequested -= OnSettingsCommandsRequested;
			base.OnNavigatedFrom(e);
		}

		static void OnSettingsCommandsRequested(SettingsPane sender, SettingsPaneCommandsRequestedEventArgs args)
		{
			args.Request.ApplicationCommands.Add(new SettingsCommand("privacy_policy", "Privacy policy", delegate
			{
				// TODO: Open privacy policy
			}));
		}
	}
}
