using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using Windows.UI.Xaml.Media.Imaging;
using SnapDotNet.Core.Miscellaneous.Helpers;
using SnapDotNet.Core.Miscellaneous.Models;

namespace SnapDotNet.Core.Snapchat.Models
{
	[DataContract]
	public class Captcha : NotifyPropertyChangedBase
	{
		public Captcha(string captchaId, IEnumerable<byte[]> imagesAsBytes)
		{
			_id = captchaId;
			_images = new ObservableCollection<BitmapImage>();
			foreach (var b in imagesAsBytes)
				_images.Add(ImageConverter.ByteArrayToBitmapImageAsync(b).Result);
		}

		[DataMember]
		public string Id
		{
			get { return _id; }
			set { SetField(ref _id, value); }
		}
		private string _id;

		[DataMember]
		public ObservableCollection<BitmapImage> Images
		{
			get { return _images; }
			set { SetField(ref _images, value); }
		}
		private ObservableCollection<BitmapImage> _images;
	}
}
