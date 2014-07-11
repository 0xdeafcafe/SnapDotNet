using Snapchat.Models;
using System.Collections.ObjectModel;

namespace Snapchat.ViewModels.PageContents
{
    public class ConversationsViewModel
		 : BaseViewModel
    {
		public ObservableCollection<IConversation> FilteredConversations
		{
			get
			{
				if (string.IsNullOrWhiteSpace(FilterText))
					return Conversations;

				var filteredConvos = new ObservableCollection<IConversation>();
				foreach (var convo in Conversations)
				{
					foreach (var friend in App.SnapchatManager.Account.Friends)
					{
						if (friend.Name == ((Conversation)convo).Participants[1] && friend.FriendlyName.ToLowerInvariant().Contains(FilterText.ToLowerInvariant()))
							filteredConvos.Add(convo);
					}
				}
				return filteredConvos;
			}
		}

		public string FilterText
		{
			get { return _filterText; }
			set
			{
				TryChangeValue(ref _filterText, value);
				ExplicitOnNotifyPropertyChanged("FilteredConversations");
			}
		}
		private string _filterText;
    }
}
