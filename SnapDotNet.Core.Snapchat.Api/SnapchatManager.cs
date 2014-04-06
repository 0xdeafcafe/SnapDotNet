using System;
using SnapDotNet.Core.Snapchat.Models;

namespace SnapDotNet.Core.Snapchat.Api
{
	public class SnapChatManager
	{
		public String AuthToken { get; private set; }

		public String Username { get; private set; }

		public Account Account { get; private set; }

		/// <summary>
		/// 
		/// </summary>
		public SnapChatManager()
		{
			Endpoints = new Endpoints(this);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="username"></param>
		/// <param name="authToken"></param>
		/// <param name="getUpdates"></param>
		public SnapChatManager(string username, string authToken, bool getUpdates = false)
		{
			Endpoints = new Endpoints(this);

			UpdateUsername(username);
			UpdateAccessCode(authToken);

			if (getUpdates)
				Endpoints.GetUpdatesAsync().Wait();
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
