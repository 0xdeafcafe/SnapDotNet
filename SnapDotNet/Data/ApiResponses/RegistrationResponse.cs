using System;
using System.Runtime.Serialization;

namespace SnapDotNet.Data.ApiResponses
{
	[DataContract]
	internal class RegistrationResponse
		: Response
	{
		[DataMember(Name = "email")]
		public string Email { get; set; }

		[DataMember(Name = "should_send_text_to_verify_number")]
		public bool ShouldSendTextToVerifyNumber { get; set; }

		[DataMember(Name = "snapchat_phone_number")]
		public string SnapchatPhoneNumber { get; set; }

		[DataMember(Name = "auth_token")]
		public string AuthToken { get; set; }
	}
}
