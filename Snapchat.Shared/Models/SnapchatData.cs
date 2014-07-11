using System;
using System.Collections.ObjectModel;
using Snapchat.SnapLogic.Models.New;
using SnapDotNet.Core.Miscellaneous.Models;

namespace Snapchat.Models
{
	public class SnapchatData
		: NotifyPropertyChangedBase
	{
		public Account UserAccount
		{
			get { return _userAccount; }
			set { SetField(ref _userAccount, value); }
		}
		private Account _userAccount;

		public ObservableCollection<IConversation> Conversations
		{
			get { return _conversations; }
			set { SetField(ref _conversations, value); }
		}
		private ObservableCollection<IConversation> _conversations = new ObservableCollection<IConversation>();

		public String BackgroundFetchSecretKey
		{
			get { return _backgroundFetchSecretKey; }
			set { SetField(ref _backgroundFetchSecretKey, value); }
		}
		private String _backgroundFetchSecretKey;

		public MessagingGatewayInfo MessagingGatewayInfo
		{
			get { return _messagingGatewayInfo; }
			set { SetField(ref _messagingGatewayInfo, value); }
		}
		private MessagingGatewayInfo _messagingGatewayInfo;

		public StoriesResponse StoriesResponse
		{
			get { return _storiesResponse; }
			set { SetField(ref _storiesResponse, value); }
		}
		private StoriesResponse _storiesResponse;
	}
}
