using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace SnapDotNet.Azure.MobileService.Models
{
	[DataContract]
	public class ConversationReponseWrapper
	{
		[DataMember(Name = "conversations_response")]
		public ObservableCollection<ConversationResponse> ConversationResponses { get; set; } 
	}
}