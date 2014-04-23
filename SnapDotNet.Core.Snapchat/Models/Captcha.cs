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
		public Captcha(string captchaId, ObservableCollection<byte[]> images)
		{
			_id = captchaId;
			_image0 = ImageConverter.ByteArrayToBitmapImageAsync(images[0]).Result;
			_image1 = ImageConverter.ByteArrayToBitmapImageAsync(images[1]).Result;
			_image2 = ImageConverter.ByteArrayToBitmapImageAsync(images[2]).Result;
			_image3 = ImageConverter.ByteArrayToBitmapImageAsync(images[3]).Result;
			_image4 = ImageConverter.ByteArrayToBitmapImageAsync(images[4]).Result;
			_image5 = ImageConverter.ByteArrayToBitmapImageAsync(images[5]).Result;
			_image6 = ImageConverter.ByteArrayToBitmapImageAsync(images[6]).Result;
			_image7 = ImageConverter.ByteArrayToBitmapImageAsync(images[7]).Result;
			_image8 = ImageConverter.ByteArrayToBitmapImageAsync(images[8]).Result;
		}

		[DataMember]
		public string Id
		{
			get { return _id; }
			set { SetField(ref _id, value); }
		}
		private string _id;

		//[DataMember]
		//public ObservableCollection<BitmapImage> Images
		//{
		//	get { return _images; }
		//	set { SetField(ref _images, value); }
		//}
		//private ObservableCollection<BitmapImage> _images;

		[DataMember]
		public BitmapImage Image0
		{
			get { return _image0; }
			set { SetField(ref _image0, value); }
		}
		private BitmapImage _image0;

		[DataMember]
		public BitmapImage Image1
		{
			get { return _image1; }
			set { SetField(ref _image1, value); }
		}
		private BitmapImage _image1;

		[DataMember]
		public BitmapImage Image2
		{
			get { return _image2; }
			set { SetField(ref _image2, value); }
		}
		private BitmapImage _image2;

		[DataMember]
		public BitmapImage Image3
		{
			get { return _image3; }
			set { SetField(ref _image3, value); }
		}
		private BitmapImage _image3;

		[DataMember]
		public BitmapImage Image4
		{
			get { return _image4; }
			set { SetField(ref _image4, value); }
		}
		private BitmapImage _image4;

		[DataMember]
		public BitmapImage Image5
		{
			get { return _image5; }
			set { SetField(ref _image5, value); }
		}
		private BitmapImage _image5;

		[DataMember]
		public BitmapImage Image6
		{
			get { return _image6; }
			set { SetField(ref _image6, value); }
		}
		private BitmapImage _image6;

		[DataMember]
		public BitmapImage Image7
		{
			get { return _image7; }
			set { SetField(ref _image7, value); }
		}
		private BitmapImage _image7;

		[DataMember]
		public BitmapImage Image8
		{
			get { return _image8; }
			set { SetField(ref _image8, value); }
		}
		private BitmapImage _image8;
	}
}
