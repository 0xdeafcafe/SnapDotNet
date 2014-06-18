using Windows.UI.Xaml.Controls;

namespace Snapchat.Dialogs
{
	public sealed partial class ChangeFriendDisplayNameDialog
	{
		public ChangeFriendDisplayNameDialog(string currentName)
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
