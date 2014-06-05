using System;
using System.Linq;
using Windows.UI;
using Windows.UI.Xaml.Media;
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
		public String FriendlyPendingConversationCount
		{
			get
			{
				var updates = App.SnapchatManager.AllUpdates;
				if (updates == null || updates.ConversationResponse == null)
					return String.Empty;

				var pendingCount =
					updates.ConversationResponse.Sum(
						conversation => conversation.PendingChatsFor.Length + conversation.PendingReceivedSnaps.Length);

				return pendingCount <= 0
					? ":("
					: pendingCount > 99
						? ":o"
						: pendingCount.ToString();
			}
		}

		public SolidColorBrush PendingConversationCountButtonBorder
		{
			get
			{
				var updates = App.SnapchatManager.AllUpdates;
				if (updates == null || updates.ConversationResponse == null)
					return new SolidColorBrush(new Color { A=0xFF, R = 0x00, G = 0x00, B = 0x00 });

				Snap latestSnap = null;
				foreach (var conversation in updates.ConversationResponse)
				{
					foreach (var snap in conversation.PendingReceivedSnaps)
					{
						latestSnap = snap;
						break;
					}
					if (latestSnap != null) 
						break;
				}

				if (latestSnap == null)
					return new SolidColorBrush(new Color { A = 0xFF, R = 0x00, G = 0x00, B = 0x00 });

				if (latestSnap.MediaType == MediaType.FriendRequestVideo ||
				    latestSnap.MediaType == MediaType.FriendRequestVideoNoAudio || latestSnap.MediaType == MediaType.Video ||
				    latestSnap.MediaType == MediaType.VideoNoAudio)
					return new SolidColorBrush(new Color { A = 0xFF, R = 0x9B, G = 0x62, B = 0x99 });
				
				return new SolidColorBrush(new Color { A = 0xFF, R = 0xED, G = 0x21, B = 0x52 });
			}
		}

		public SolidColorBrush PendingConversationCountButtonBackground
		{
			get
			{
				var updates = App.SnapchatManager.AllUpdates;
				if (updates == null || updates.ConversationResponse == null)
					return new SolidColorBrush(new Color { A = 0x00, R = 0x00, G = 0x00, B = 0x00 });

				Snap latestSnap = null;
				foreach (var conversation in updates.ConversationResponse)
				{
					foreach (var snap in conversation.PendingReceivedSnaps)
					{
						latestSnap = snap;
						break;
					}
					if (latestSnap != null)
						break;
				}

				if (latestSnap == null)
					return new SolidColorBrush(new Color { A = 0x00, R = 0x00, G = 0x00, B = 0x00 });

				if (latestSnap.MediaType == MediaType.FriendRequestVideo ||
					latestSnap.MediaType == MediaType.FriendRequestVideoNoAudio || latestSnap.MediaType == MediaType.Video ||
					latestSnap.MediaType == MediaType.VideoNoAudio)
					return new SolidColorBrush(new Color { A = 0xFF, R = 0x9B, G = 0x62, B = 0x99 });

				return new SolidColorBrush(new Color { A = 0xFF, R = 0xED, G = 0x21, B = 0x52 });
			}
		}
	}
}
