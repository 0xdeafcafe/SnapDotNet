using System;

namespace SnapDotNet.Core.Snapchat.Api
{
	public class SnapchatManager
	{
		public String AccessCode { get; private set; }

		public SnapchatManager()
		{
			Endpoints = new Endpoints(this);
		}

		public SnapchatManager(string accessCode)
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
