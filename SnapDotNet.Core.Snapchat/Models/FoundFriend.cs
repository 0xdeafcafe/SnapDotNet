using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using SnapDotNet.Core.Miscellaneous.Models;

namespace SnapDotNet.Core.Snapchat.Models
{
	/// <summary>
	/// Represents account information returned when looking up a phone number
	/// </summary>
	[DataContract]
	public class FoundFriend
		: Response
	{
		[DataMember(Name = "results")]
		public ObservableCollection<Results> Results
		{
			get { return _results; }
			set { SetField(ref _results, value); }
		}
		private ObservableCollection<Results> _results;
	}

	[DataContract]
	public class Results : NotifyPropertyChangedBase
	{
		[DataMember(Name = "name")]
		public string Name
		{
			get { return _name; }
			set { SetField(ref _name, value); }
		}
		private string _name;

		[DataMember(Name = "display")]
		public string Display
		{
			get { return _display; }
			set { SetField(ref _display, value); }
		}
		private string _display;

		[DataMember(Name = "type")]
		public AccountPrivacy AccountPrivacy
		{
			get { return _accountPrivacy; }
			set { SetField(ref _accountPrivacy, value); }
		}
		private AccountPrivacy _accountPrivacy;
	}
}
