using System;
using Windows.UI;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using SnapDotNet.Core.Snapchat.Models;

namespace SnapDotNet.Apps.Converters
{
    public class SnapColourFromSnapConverter : IValueConverter
    {
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			var snap = value as Snap;
			if (snap == null) return null;

			return (snap.MediaType == MediaType.Image || snap.MediaType == MediaType.FriendRequestImage)
				? new SolidColorBrush(new Color {A = 0xBB, R = 0xE3, G = 0x09, B = 0x47})
				: new SolidColorBrush(new Color {A = 0xBB, R = 0xB2, G = 0x08, B = 0x78});
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotImplementedException();
		}
	}
}
