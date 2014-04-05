using System;
using Windows.UI.Text;
using Windows.UI.Xaml.Data;

namespace SnapDotNet.Converters
{
    public class CharacterCasingConverter
		: IValueConverter
    {
		public LetterCase LetterCase { get; set; }

		public object Convert(object value, Type targetType, object parameter, string language)
		{
			if (LetterCase == LetterCase.Lower)
				return value.ToString().ToLowerInvariant();
			else
				return value.ToString().ToUpperInvariant();
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotImplementedException();
		}
	}
}
