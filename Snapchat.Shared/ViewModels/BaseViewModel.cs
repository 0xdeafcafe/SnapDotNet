using System;
using System.Linq;
using Snapchat.Common;
using SnapDotNet.Core.Snapchat.Models.New;

namespace Snapchat.ViewModels
{
	public class BaseViewModel
		: ObservableObject
	{
		public BaseViewModel()
		{
			Helpers = new SnapchatDataHelpers();
		}

		public AllUpdatesResponse Updates
		{
			get { return App.SnapchatManager.AllUpdates; }
		}

		public UpdatesResponse Account
		{
			get { return App.SnapchatManager.AllUpdates.UpdatesResponse; }
		}

		public SnapchatDataHelpers Helpers { get; set; }
	}

	public class SnapchatDataHelpers
		: ObservableObject
	{
		public string FriendlyPendingConversationCount()
		{
			var updates = App.SnapchatManager.AllUpdates;
			if (updates == null || updates.ConversationResponse == null)
				return String.Empty;

			var pendingCount = updates.ConversationResponse.Sum(conversation => conversation.PendingChatsFor.Length + conversation.PendingReceivedSnaps.Length);

			return pendingCount <= 0 ? 
				"" : pendingCount > 99 ? 
					":o" : pendingCount.ToString();
		}
	}
}
