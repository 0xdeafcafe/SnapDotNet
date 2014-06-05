using System;
using System.Runtime.Serialization;
using SnapDotNet.Core.Miscellaneous.Models;

namespace SnapDotNet.Core.Snapchat.Models.New
{
	[DataContract]
	public class MessagingGatewayInfo
		: NotifyPropertyChangedBase
	{
		[DataMember(Name = "gateway_auth_token")]
		public GatewayAuthenticationToken GatewayAuthenticationToken { get; set; }

		[DataMember(Name = "gateway_server")]
		public String GatewayServer { get; set; }
	}

	[DataContract]
	public class GatewayAuthenticationToken
		: NotifyPropertyChangedBase
	{
		[DataMember(Name = "mac")]
		public String Mac { get; set; }

		[DataMember(Name = "payload")]
		public String Payload { get; set; }
	}
}
