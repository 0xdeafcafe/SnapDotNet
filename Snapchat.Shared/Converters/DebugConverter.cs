using System;
using System.Diagnostics;
using Windows.UI.Xaml.Data;

namespace Snapchat.Converters
{
	public sealed class DebugConverter
		: IValueConverter
	{
		#region IValueConverter Members

		public object Convert(object value, Type targetType, object parameter, string language)
		{
			Debugger.Break();
			return value;
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}