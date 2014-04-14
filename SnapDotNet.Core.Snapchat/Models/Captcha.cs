using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using SnapDotNet.Core.Miscellaneous.Models;

namespace SnapDotNet.Core.Snapchat.Models
{
	[DataContract]
	public class Captcha : NotifyPropertyChangedBase
	{
		public Captcha(string captchaId, ObservableCollection<byte[]> images)
		{
			_id = captchaId;
			_images = images;
		}

		[DataMember]
		public string Id
		{
			get { return _id; }
			set { SetField(ref _id, value); }
		}
		private string _id;

		[DataMember]
		public ObservableCollection<byte[]> Images
		{
			get { return _images; }
			set { SetField(ref _images, value); }
		}
		private ObservableCollection<byte[]> _images;
	}
}
