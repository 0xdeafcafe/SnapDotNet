using SnapDotNet.Core.Snapchat.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace SnapDotNet.Apps.Converters
{
	public sealed class SnapStatusToForegroundConverter
		: IValueConverter
	{
		public Brush DeliveredForeground { get; set; }
		public Brush ScreenshottedForeground { get; set; }
		public Brush OpenedForeground { get; set; }
		public Brush DownloadingForeground { get; set; }
		public Brush SentForeground { get; set; }

		public object Convert(object value, Type targetType, object parameter, string language)
		{
			var snap = value as Snap;
			if (snap == null) return null;

			var snapStatus = (SnapStatus) snap.Status;
			switch (snapStatus)
			{
				case SnapStatus.Delivered:
					return DeliveredForeground;

				case SnapStatus.Downloading:
					return DownloadingForeground;

				case SnapStatus.Opened:
					return OpenedForeground;

				case SnapStatus.Screenshotted:
					return ScreenshottedForeground;

				case SnapStatus.Sent:
					return SentForeground;

				default:
					return new SolidColorBrush(Colors.Red);
			}
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotImplementedException();
		}
	}
}