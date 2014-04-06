using System;
using SnapDotNet.Core.Snapchat.Models;

namespace SnapDotNet.Core.Snapchat.Api
{
	public class SnapChatManager
	{
		public String AuthToken { get; private set; }

		public String Username { get; private set; }

		public Account Account { get; private set; }

		public SnapChatManager()
		{
			Endpoints = new Endpoints(this);
		}

		public SnapChatManager(string username, string authToken, bool getUpdates = false)
		{
			UpdateUsername(username);
			UpdateAccessCode(authToken);

			if (!getUpdates)
				return;

			Endpoints.GetUpdatesAsync(username).Wait();
		}

		#region Setters

		public void UpdateAccessCode(string authToken)
		{
			AuthToken = authToken;
		}

		public void UpdateAccount(Account account)
		{
			Account = account;
		}

		public void UpdateUsername(string username)
		{
			Username = username;
		}

		#endregion

		public Endpoints Endpoints { get; private set; }
	}
}
