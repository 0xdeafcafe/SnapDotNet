using System;
using SnapDotNet.Core.Miscellaneous.Models;

namespace Snapchat.Models
{
	public class MessagingGatewayInfo
		: NotifyPropertyChangedBase
	{
		public GatewayAuthenticationToken GatewayAuthenticationToken
		{
			get { return _gatewayAuthenticationToken; }
			set { SetField(ref _gatewayAuthenticationToken, value); }
		}
		private GatewayAuthenticationToken _gatewayAuthenticationToken;

		public String GatewayServer
		{
			get { return _gatewayServer; }
			set { SetField(ref _gatewayServer, value); }
		}
		private String _gatewayServer;
	}

	public class GatewayAuthenticationToken
		: NotifyPropertyChangedBase
	{
		public String Mac
		{
			get { return _mac; }
			set { SetField(ref _mac, value); }
		}
		private String _mac;

		public String Payload
		{
			get { return _payload; }
			set { SetField(ref _payload, value); }
		}
		private String _payload;
	}
}
