using System.Runtime.Serialization;
using SnapDotNet.Core.Miscellaneous.Models;

namespace SnapDotNet.Core.Snapchat.Models
{
	/// <summary>
	/// Represents a response from the Snapchat API.
	/// </summary>
	[DataContract]
	public class Response
		: NotifyPropertyChangedBase
	{
		/// <summary>
		/// Gets or sets whether the user is still logged in after this response.
		/// </summary>
		[DataMember(Name = "logged")]
		public bool Logged
		{
			get { return _logged; }
			set { SetField(ref _logged, value); }
		}
		private bool _logged;

		/// <summary>
		/// Gets or sets the response message.
		/// </summary>
		[DataMember(Name = "message")]
		public string Message
		{
			get { return _message; }
			set { SetField(ref _message, value); }
		}
		private string _message;

		/// <summary>
		/// Gets or sets the optional parameter passed along with the response message.
		/// </summary>
		[DataMember(Name = "param")]
		public string Parameter
		{
			get { return _parameter; }
			set { SetField(ref _parameter, value); }
		}
		private string _parameter;
	}
}
