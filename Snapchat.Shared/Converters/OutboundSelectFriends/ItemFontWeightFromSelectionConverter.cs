using System;
using Windows.UI.Text;
using Windows.UI.Xaml.Data;

namespace Snapchat.Converters.OutboundSelectFriends
{
	public sealed class ItemFontWeightFromSelectionConverter
		: IValueConverter
	{
		#region IValueConverter Members

		public object Convert(object value, Type targetType, object parameter, string language)
		{
			return (bool) value 
				? FontWeights.SemiBold
				: FontWeights.Normal;
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}