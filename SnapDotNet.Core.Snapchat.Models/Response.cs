using System.Runtime.Serialization;

namespace SnapDotNet.Core.Snapchat.Models
{
	/// <summary>
	/// Represents a response from the Snapchat API.
	/// </summary>
	[DataContract]
	public abstract class Response
	{
		/// <summary>
		/// Gets or sets whether the user is still logged in after this response.
		/// </summary>
		[DataMember(Name = "logged")]
		public bool Logged { get; set; }

		/// <summary>
		/// Gets or sets the response message.
		/// </summary>
		[DataMember(Name = "message")]
		public string Message { get; set; }

		/// <summary>
		/// Gets or sets the optional parameter passed along with the response message.
		/// </summary>
		[DataMember(Name = "param")]
		public string Parameter { get; set; }
	}
}
