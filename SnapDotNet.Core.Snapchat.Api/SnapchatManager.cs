using System;

namespace SnapDotNet.Core.Snapchat.Api
{
	public class SnapChatManager
	{
		public String AccessCode { get; private set; }

		public SnapChatManager()
		{
			Endpoints = new Endpoints(this);
		}

		public SnapChatManager(string accessCode)
			: base()
		{
			UpdateAccessCode(accessCode);
		}

		public void UpdateAccessCode(string accessCode)
		{
			AccessCode = accessCode;
		}

		public Endpoints Endpoints { get; private set; }
	}
}
