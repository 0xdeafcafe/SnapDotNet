using System;
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace Snapchat.ViewModels.PageContents
{
	public class PreviewViewModel
		: BaseViewModel
	{
		public PreviewViewModel(ImageSource source)
		{
			_drawnLines.CollectionChanged += delegate { ExplicitOnNotifyPropertyChanged("DrawnLines"); };

			ImageSource = source;
		}

		public ImageSource ImageSource
		{
			get { return _imageSource; }
			set { TryChangeValue(ref _imageSource, value); }
		}
		private ImageSource _imageSource;

		public ObservableCollection<Line> DrawnLines
		{
			get { return _drawnLines; }
			set { TryChangeValue(ref _drawnLines, value); }
		}
		private ObservableCollection<Line> _drawnLines = new ObservableCollection<Line>(); 

		public string CurrentTimeFilterTime { get { return DateTime.Now.ToString("h:mm"); } }
		public string CurrentTimeFilterDesignator { get { return DateTime.Now.ToString("tt"); } }
	}
}

