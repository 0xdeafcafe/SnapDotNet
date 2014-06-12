using System.Collections.ObjectModel;
using SnapDotNet.Core.Snapchat.Models.AppSpecific;
using SnapDotNet.Core.Snapchat.Models.New;

namespace Snapchat.ViewModels
{
	public sealed class ConversationViewModel
		 : BaseViewModel
	{
		public ConversationViewModel() { }

		public ConversationViewModel(ConversationResponse conversation)
		{
			Conversation = conversation;
		}

		public ConversationResponse Conversation
		{
			get { return _conversation; }
			set
			{
				TryChangeValue(ref _conversation, value);
				OnNotifyPropertyChanged("ConversationThread");
			}
		}
		private ConversationResponse _conversation;

		public ObservableCollection<IConversationThreadItem> ConversationThread
		{
			get { return Conversation.ConversationMessages.SortedMessages; }
		}
    }
}
