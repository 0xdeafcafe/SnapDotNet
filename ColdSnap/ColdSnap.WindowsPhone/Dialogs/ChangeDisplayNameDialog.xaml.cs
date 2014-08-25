using Windows.UI.Xaml.Controls;

namespace ColdSnap.Dialogs
{
	public sealed partial class ChangeDisplayNameDialog
	{
		public ChangeDisplayNameDialog(string currentName)
		{
			InitializeComponent();

			DisplayNameTextBox.Text = currentName;
			NewDisplayName = null;
		}

		private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
		{
			NewDisplayName = DisplayNameTextBox.Text;
		}

		private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args) {  }

		public string NewDisplayName { get; set; }
	}
}
