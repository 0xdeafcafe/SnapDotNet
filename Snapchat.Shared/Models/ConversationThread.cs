using System;
using System.Collections.ObjectModel;

namespace Snapchat.Models
{
	public interface IConversationThreadItem { }

	public class UserHeader : IConversationThreadItem
	{
		public UserHeader()
		{
			Messages = new ObservableCollection<IConversationItem>();
		}

		public String User { get; set; }

		public DateTime? FirstPostedItemDateTime
		{
			get { return Messages.Count > 0 ? (DateTime?) Messages[0].PostedAt : null; }
		}

		public ObservableCollection<IConversationItem> Messages { get; set; }
	}

	public class TimeSeperator : IConversationThreadItem
	{
		public TimeSeperator(DateTime dateTime)
		{
			SeperationDateTime = dateTime;
		}

		public DateTime SeperationDateTime { get; private set; }
	}
}
