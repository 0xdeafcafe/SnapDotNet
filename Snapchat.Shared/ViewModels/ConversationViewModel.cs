using System.Collections.ObjectModel;
using Snapchat.Models;

namespace Snapchat.ViewModels
{
	public sealed class ConversationViewModel
		 : BaseViewModel
	{
		public ConversationViewModel() { }

		public ConversationViewModel(Conversation conversation)
		{
			Conversation = conversation;
		}

		public Conversation Conversation
		{
			get { return _conversation; }
			set
			{
				TryChangeValue(ref _conversation, value);
				OnNotifyPropertyChanged("ConversationThread");
			}
		}
		private Conversation _conversation;

		public ObservableCollection<IConversationThreadItem> ConversationThread
		{
			get { return Conversation.ConversationMessages.SortedMessages; }
		}
    }
}
