using SnapDotNet.Core.Snapchat.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace SnapDotNet.Apps.Converters
{
    public sealed class SnapStatusToIconConverter
		: IValueConverter
    {
		public Symbol DeliveredIcon { get; set; }
		public Symbol ScreenshottedIcon { get; set; }
		public Symbol OpenedIcon { get; set; }
		public Symbol DownloadingIcon { get; set; }
		public Symbol SentIcon { get; set; }

		public object Convert(object value, Type targetType, object parameter, string language)
		{
			var snapStatus = (SnapStatus) value;
			switch (snapStatus)
			{
				case SnapStatus.Delivered:
					return DeliveredIcon;

				case SnapStatus.Downloading:
					return DownloadingIcon;
					
				case SnapStatus.Opened:
					return OpenedIcon;

				case SnapStatus.Screenshotted:
					return ScreenshottedIcon;

				case SnapStatus.Sent:
					return SentIcon;

				default:
					return null;
			}
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotImplementedException();
		}
	}
}
