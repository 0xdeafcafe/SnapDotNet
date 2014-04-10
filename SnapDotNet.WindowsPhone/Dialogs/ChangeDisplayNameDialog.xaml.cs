using Windows.UI.Xaml.Controls;

namespace SnapDotNet.Apps.Dialogs
{
	public sealed partial class ChangeDisplayNameDialog
	{
		public ChangeDisplayNameDialog(string currentName)
		{
			InitializeComponent();

			DisplyNameTextBox.Text = currentName;
		}

		private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
		{
			NewDisplayName = DisplyNameTextBox.Text;
		}

		private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args) { }

		public string NewDisplayName { get; set; }
	}
}
