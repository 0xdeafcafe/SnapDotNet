using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace SnapDotNet.Core.Snapchat.Models
{
	/// <summary>
	/// Represents a person's activity which is visible to other users.
	/// </summary>
	[DataContract]
	public class PublicActivity
		: Response
	{
		[DataMember(Name = "best_friends")]
		public ObservableCollection<string> BestFriends
		{
			get { return _bestFriends; }
			set { SetField(ref _bestFriends, value); }
		}
		private ObservableCollection<string> _bestFriends;

		[DataMember(Name = "score")]
		public int Score
		{
			get { return _score; }
			set { SetField(ref _score, value); }
		}
		private int _score;
	}
}
