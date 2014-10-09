using System;
using System.Runtime.Serialization;

namespace SnapDotNet.Data.ApiResponses
{
	[DataContract]
	internal sealed class MessagingGatewayInfoResponse
	{
		[DataMember(Name = "gateway_auth_token")]
		public GatewayAuthenticationTokenResponse GatewayAuthenticationToken { get; set; }

		[DataMember(Name = "gateway_server")]
		public string GatewayServer { get; set; }
	}

	[DataContract]
	internal sealed class GatewayAuthenticationTokenResponse
	{
		[DataMember(Name = "mac")]
		public string Mac { get; set; }

		[DataMember(Name = "payload")]
		public string Payload { get; set; }
	}
}
