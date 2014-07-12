using System;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI;
using Windows.UI.Xaml.Media;
using Snapchat.Common;
using Snapchat.Models;

namespace Snapchat.ViewModels
{
	public class BaseViewModel
		: ObservableObject
	{
		public BaseViewModel()
		{
			Helpers = new SnapchatDataHelpers();
		}

		public SnapchatData Updates
		{
			get { return App.SnapchatManager.SnapchatData; }
		}

		public Account Account
		{
			get { return App.SnapchatManager.SnapchatData.UserAccount; }
		}

		public ObservableCollection<IConversation> Conversations
		{
			get { return App.SnapchatManager.SnapchatData.Conversations; }
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
				var updates = App.SnapchatManager.SnapchatData;
				if (updates == null || updates.Conversations == null)
					return String.Empty;

				var pendingCount =
					updates.Conversations.Where(c => c is Conversation).Sum(
						conversation => ((Conversation)conversation).ConversationMessages.Snaps.Count(s => s.IsIncoming && s.Status == SnapStatus.Delivered) + ((Conversation)conversation).PendingChatMessages.Count);

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
				var updates = App.SnapchatManager.SnapchatData;
				if (updates == null || updates.Conversations == null)
					return new SolidColorBrush(new Color { A=0xFF, R = 0x00, G = 0x00, B = 0x00 });

				Snap latestSnap = null;
				foreach (var conversation in updates.Conversations.Where(c => c is Conversation))
				{
					foreach (var snap in ((Conversation)conversation).ConversationMessages.Snaps.Where(s => s.IsIncoming && s.Status == SnapStatus.Delivered))
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
				var updates = App.SnapchatManager.SnapchatData;
				if (updates == null || updates.Conversations == null)
					return new SolidColorBrush(new Color { A = 0x00, R = 0x00, G = 0x00, B = 0x00 });

				Snap latestSnap = null;
				foreach (var conversation in updates.Conversations.Where(c => c is Conversation))
				{
					foreach (var snap in ((Conversation)conversation).ConversationMessages.Snaps.Where(s => s.IsIncoming && s.Status == SnapStatus.Delivered))
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
