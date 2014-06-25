using SnapDotNet.Core.Snapchat.Models.New;
using System.Collections.ObjectModel;
namespace Snapchat.ViewModels.PageContents
{
    public class ConversationsViewModel
		 : BaseViewModel
    {
		public ObservableCollection<ConversationResponse> FilteredConversations
		{
			get
			{
				if (string.IsNullOrWhiteSpace(FilterText))
					return Conversations;

				var filteredConvos = new ObservableCollection<ConversationResponse>();
				foreach (var convo in Conversations)
				{
					if (convo.Participants[1].Contains(FilterText))
						filteredConvos.Add(convo);
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
