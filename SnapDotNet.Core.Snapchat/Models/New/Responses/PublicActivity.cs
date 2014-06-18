using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;

namespace SnapDotNet.Core.Snapchat.Models.New.Responses
{
	/// <summary>
	/// Represents a person's activity which is visible to other users.
	/// </summary>
	[DataContract]
	public class PublicActivity
		: Response
	{
		public PublicActivity ()
		{
			_bestFriends.CollectionChanged += delegate { NotifyPropertyChanged("BestFriends"); NotifyPropertyChanged("HasBestFriends"); };
		}

		[DataMember(Name = "best_friends")]
		public ObservableCollection<string> BestFriends
		{
			get { return _bestFriends; }
			set
			{
				SetField(ref _bestFriends, value);
				NotifyPropertyChanged("HasBestFriends");
			}
		}
		private ObservableCollection<string> _bestFriends = new ObservableCollection<string>();

		[DataMember(Name = "score")]
		public int Score
		{
			get { return _score; }
			set { SetField(ref _score, value); }
		}
		private int _score;

		#region Helpers

		[IgnoreDataMember]
		public Boolean HasBestFriends
		{
			get { return BestFriends.Any(); }
		}

		#endregion
	}
}
