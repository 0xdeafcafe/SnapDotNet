using System;
using System.Runtime.Serialization;
using SnapDotNet.Core.Miscellaneous.Models;

namespace Snapchat.SnapLogic.Models.New
{
	[DataContract]
	public class MessagingGatewayInfo
		: NotifyPropertyChangedBase
	{
		[DataMember(Name = "gateway_auth_token")]
		public GatewayAuthenticationToken GatewayAuthenticationToken
		{
			get { return _gatewayAuthenticationToken; }
			set { SetField(ref _gatewayAuthenticationToken, value); }
		}
		private GatewayAuthenticationToken _gatewayAuthenticationToken;

		[DataMember(Name = "gateway_server")]
		public String GatewayServer
		{
			get { return _gatewayServer; }
			set { SetField(ref _gatewayServer, value); }
		}
		private String _gatewayServer;
	}

	[DataContract]
	public class GatewayAuthenticationToken
		: NotifyPropertyChangedBase
	{
		[DataMember(Name = "mac")]
		public String Mac
		{
			get { return _mac; }
			set { SetField(ref _mac, value); }
		}
		private String _mac;

		[DataMember(Name = "payload")]
		public String Payload
		{
			get { return _payload; }
			set { SetField(ref _payload, value); }
		}
		private String _payload;
	}
}
