using System;
using Windows.UI.Xaml.Data;

namespace Snapchat.Converters
{
	public sealed class CaseConverter
		: IValueConverter
	{
		#region IValueConverter Members

		public object Convert(object value, Type targetType, object parameter, string language)
		{
			switch (SelectedCase)
			{
				case Case.Lowercase:
					return ((value as String) ?? "").ToLowerInvariant();

				case Case.Uppercase:
					return ((value as String) ?? "").ToUpperInvariant();

				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotImplementedException();
		}

		public Case SelectedCase { get; set; }

		#endregion
	}

	public enum Case
	{
		Uppercase,
		Lowercase
	}
}