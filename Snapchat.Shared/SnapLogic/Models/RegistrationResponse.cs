using System.Runtime.Serialization;
using SnapDotNet.Core.Miscellaneous.Models;

namespace SnapDotNet.Core.Snapchat.Models
{
	[DataContract]
	public class RegistrationResponse : Response
	{
		[DataMember(Name = "email")]
		public string Email
		{
			get { return _email; }
			set { SetField(ref _email, value);}
		}
		private string _email;

		[DataMember(Name = "should_send_text_to_verify_number")]
		public bool ShouldSendTextToVerifyNumber
		{
			get { return _shouldSendTextToVerifyNumber; }
			set { SetField(ref _shouldSendTextToVerifyNumber, value); }
		}
		private bool _shouldSendTextToVerifyNumber;

		[DataMember(Name = "snapchat_phone_number")]
		public string PhoneNumber
		{
			get { return _phoneNumber; }
			set { SetField(ref _phoneNumber, value); }
		}
		private string _phoneNumber;

		[DataMember(Name = "captcha")]
		public Captcha CaptchaMessage
		{
			get { return _captchaMessage; }
			set { SetField(ref _captchaMessage, value); }
		}
		private Captcha _captchaMessage;

		[DataMember(Name = "auth_token")]
		public string AuthToken
		{
			get { return _authToken; }
			set { SetField(ref _authToken, value); }
		}
		private string _authToken;

		[DataContract]
		public class Captcha : NotifyPropertyChangedBase
		{
			[DataMember(Name = "prompt")]
			public string Prompt
			{
				get { return _prompt; }
				set { SetField(ref _prompt, value); }
			}
			private string _prompt;
		}
	}
}
