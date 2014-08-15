using System;
using System.Runtime.Serialization;

namespace SnapDotNet.Responses
{
	[DataContract]
	internal sealed class MessagingGatewayInfoResponse
	{
		[DataMember(Name = "gateway_auth_token")]
		public GatewayAuthenticationTokenResponse GatewayAuthenticationToken { get; set; }

		[DataMember(Name = "gateway_server")]
		public String GatewayServer { get; set; }
	}

	[DataContract]
	internal sealed class GatewayAuthenticationTokenResponse
	{
		[DataMember(Name = "mac")]
		public String Mac { get; set; }

		[DataMember(Name = "payload")]
		public String Payload { get; set; }
	}
}