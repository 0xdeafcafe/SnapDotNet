using SnapDotNet.Core.Snapchat.Models;
using System;
using Windows.UI.Xaml.Data;

namespace SnapDotNet.Apps.Converters
{
    public sealed class SnapStatusToFriendlyTextConverter
		: IValueConverter
    {
		const string StringResourceNamePrefix = "SnapStatus";

		public object Convert(object value, Type targetType, object parameter, string language)
		{
			if (!(value is Snap))
				throw new ArgumentException();

			var snap = value as Snap;
			switch (snap.Status)
			{
				case SnapStatus.None:
					return null;

				case SnapStatus.Delivered:
					if (snap.RecipientName == null)
						return App.Loader.GetString(StringResourceNamePrefix + (snap.HasMedia ? "TapAndHold" : "TapToLoad"));
					break;
			}

			return App.Loader.GetString(StringResourceNamePrefix + snap.Status);
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotImplementedException();
		}
	}
}
