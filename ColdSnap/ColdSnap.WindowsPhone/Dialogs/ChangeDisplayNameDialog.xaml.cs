using Windows.UI.Xaml;

namespace ColdSnap.Dialogs
{
	public sealed partial class ChangeDisplayNameDialog
	{
		public static DependencyProperty NewDisplayNameProperty = DependencyProperty.Register("NewDisplayName", typeof (string),
			typeof (LogInDialog), new PropertyMetadata(string.Empty));

		public ChangeDisplayNameDialog(string currentName)
		{
			InitializeComponent();

			NewDisplayName = currentName;
		}

		public string NewDisplayName
		{
			get { return (string) GetValue(NewDisplayNameProperty); }
			set { SetValue(NewDisplayNameProperty, value); }
		}
	}
}
