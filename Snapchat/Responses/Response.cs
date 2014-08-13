using SnapDotNet.Utilities;
using System;
using System.Runtime.Serialization;

namespace SnapDotNet.Responses
{
	[DataContract]
	internal class Response
	{
		/// <summary>
		/// Gets or sets a boolean value indicating whether the request has been logged (successful).
		/// </summary>
		[DataMember(Name = "logged")]
		public bool IsLogged { get; set; }

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