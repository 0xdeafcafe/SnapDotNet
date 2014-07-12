using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Snapchat.Models;

namespace Snapchat.Converters.Conversations
{
	public class PendingUploadIconConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			try
			{
				var conversationType = (UploadStatus)value;
				switch (conversationType)
				{
					case UploadStatus.Uploading:
						return Application.Current.Resources["upload_uploading_media"];

					case UploadStatus.Error:
						return Application.Current.Resources["upload_error_media"];

					default: return null;
				}
			}
			catch
			{
				return null;
			}
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotImplementedException();
		}
	}
}
