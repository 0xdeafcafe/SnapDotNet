using SnapDotNet.Core.Snapchat.Models.New;
using System;

namespace Snapchat.ViewModels
{
	public sealed class ConversationViewModel
		 : BaseViewModel
	{
		public ConversationViewModel(ConversationResponse convo)
		{
			Conversation = convo;
		}

		public ConversationResponse Conversation
		{
			get { return _conversation; }
			set { TryChangeValue(ref _conversation, value); }
		}
		private ConversationResponse _conversation;
    }
}
