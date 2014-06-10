using System;
using System.Collections.ObjectModel;
using SnapDotNet.Core.Snapchat.Models.New;

namespace SnapDotNet.Core.Snapchat.Models.AppSpecific
{
	public interface IConversationThreadItem { }

	public class UserHeader : IConversationThreadItem
	{
		public UserHeader()
		{
			Messages = new ObservableCollection<MessageContainer>();
		}

		public String User { get; set; }

		public DateTime? FirstPostedItemDateTime
		{
			get { return Messages.Count > 0 ? (DateTime?) Messages[0].MessageMetaData.PostedAt : null; }
		}

		public ObservableCollection<MessageContainer> Messages { get; set; }
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
