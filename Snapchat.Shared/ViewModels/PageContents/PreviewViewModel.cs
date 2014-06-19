﻿using Windows.UI.Xaml.Media.Imaging;

namespace Snapchat.ViewModels.PageContents
{
	public class PreviewViewModel
		: BaseViewModel
	{
		public PreviewViewModel(WriteableBitmap writeableBitmap)
		{
			WriteableBitmap = writeableBitmap;
		}

		public WriteableBitmap WriteableBitmap
		{
			get { return _writeableBitmap; }
			set { TryChangeValue(ref _writeableBitmap, value); }
		}
		private WriteableBitmap _writeableBitmap;
	}
}
