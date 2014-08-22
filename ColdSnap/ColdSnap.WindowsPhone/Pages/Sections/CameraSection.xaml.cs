using ColdSnap.Common;
using ColdSnap.ViewModels.Sections;

namespace ColdSnap.Pages.Sections
{
	public sealed partial class CameraSection
	{
		public CameraSection()
		{
			InitializeComponent();
			DataContext = new CameraSectionViewModel();
		}

		/// <summary>
		/// Gets the view model of this section.
		/// </summary>
		public CameraSectionViewModel ViewModel
		{
			get { return DataContext as CameraSectionViewModel; }
		}

		public override void LoadState(LoadStateEventArgs e)
		{

		}

		public override void SaveState(SaveStateEventArgs e)
		{

		}
	}
}
