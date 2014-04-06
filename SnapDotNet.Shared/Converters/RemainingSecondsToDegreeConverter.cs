using System;
using Windows.UI.Xaml.Data;

namespace SnapDotNet.Apps.Converters
{
    public sealed class RemainingSecondsToDegreeConverter
		: IValueConverter
    {
		public int MaximumDuration { get; set; }

		public object Convert(object value, Type targetType, object parameter, string language)
		{
			if (value == null)
				value = 0;
			int remainingSeconds = System.Convert.ToInt32(value);
			return (360 / MaximumDuration) * remainingSeconds;
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotImplementedException();
		}
	}
}
