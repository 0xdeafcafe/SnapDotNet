using System;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using SnapDotNet.Core.Miscellaneous.Models;

namespace SnapDotNet.Core.Snapchat.Models.New
{
	[DataContract]
	public class AllUpdatesResponse
		: NotifyPropertyChangedBase
	{
		public AllUpdatesResponse()
		{
			ConversationResponse.CollectionChanged += (sender, args) => NotifyPropertyChanged("ConversationResponse");
		}

		/// <summary>
		/// 
		/// </summary>
		[DataMember(Name = "background_fetch_secret_key")]
		public String BackgroundFetchSecretKey
		{
			get { return _backgroundFetchSecretKey; }
			set { SetField(ref _backgroundFetchSecretKey, value); }
		}
		private String _backgroundFetchSecretKey;

		/// <summary>
		/// 
		/// </summary>
		[DataMember(Name = "conversations_response")]
		public ObservableCollection<ConversationResponse> ConversationResponse
		{
			get { return _conversationResponse; }
			set { SetField(ref _conversationResponse, value); }
		}
		private ObservableCollection<ConversationResponse> _conversationResponse = new ObservableCollection<ConversationResponse>();

		/// <summary>
		/// 
		/// </summary>
		[DataMember(Name = "messaging_gateway_info")]
		public MessagingGatewayInfo MessagingGatewayInfo
		{
			get { return _messagingGatewayInfo; }
			set { SetField(ref _messagingGatewayInfo, value); }
		}
		private MessagingGatewayInfo _messagingGatewayInfo;

		/// <summary>
		/// 
		/// </summary>
		[DataMember(Name = "stories_response")]
		public StoriesResponse StoriesResponse
		{
			get { return _storiesResponse; }
			set { SetField(ref _storiesResponse, value); }
		}
		private StoriesResponse _storiesResponse;

		/// <summary>
		/// 
		/// </summary>
		[DataMember(Name = "updates_response")]
		public UpdatesResponse UpdatesResponse
		{
			get { return _updatesResponse; }
			set { SetField(ref _updatesResponse, value); }
		}
		private UpdatesResponse _updatesResponse;
	}
}
