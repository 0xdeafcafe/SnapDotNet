using System;
using Windows.UI.Xaml.Data;

namespace Snapchat.Converters.Settings
{
	public sealed class NumberOfBestFriendsToIndexConverter
		: IValueConverter
	{
		#region IValueConverter Members

		public object Convert(object value, Type targetType, object parameter, string language)
		{
			int count = System.Convert.ToInt32(value);
			switch (count)
			{
				case 3: return 0;
				case 5: return 1;
				case 7: return 2;
				default: return 0;
			}
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			int index = System.Convert.ToInt32(value);
			switch (index)
			{
				case 0: return 3;
				case 1: return 5;
				case 2: return 7;
				default: return 3;
			}
		}

		#endregion
	}
}
