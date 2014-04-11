using System;
using Windows.UI.Xaml.Data;
using SnapDotNet.Core.Miscellaneous.Models;

namespace SnapDotNet.Apps.Converters
{
	public class StringCaseConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			return StringCase == StringCase.Uppercase
				? ((string) value).ToUpperInvariant() :
				((string) value).ToLowerInvariant();
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotImplementedException();
		}

		public StringCase StringCase { get; set; }
	}
}
