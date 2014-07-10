using System;
using Windows.UI;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace Snapchat.Converters.OutboundSelectFriends
{
	public sealed class ItemBackgroundFromSelectionConverter
		: IValueConverter
	{
		#region IValueConverter Members

		public object Convert(object value, Type targetType, object parameter, string language)
		{
			return (bool) value
				? new SolidColorBrush(Color.FromArgb(0xFF, 0xFA, 0xFA, 0xFA))
				: new SolidColorBrush(Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF));
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}