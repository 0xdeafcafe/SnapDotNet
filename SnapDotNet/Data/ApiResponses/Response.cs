using System;
using System.Runtime.Serialization;

namespace SnapDotNet.Data.ApiResponses
{
	[DataContract]
	internal class Response
	{
		/* Status codes:
		 *  -100    Wrong password
		 *  -101    Username/email does not exist
		 *	-200	Invalid email
		 *	-201	Email in use
		 *	   7	Password length < 8 characters
		 */
		[DataMember(Name = "status")]
		public int Status { get; set; }

		[DataMember(Name = "logged")]
		public bool IsLogged { get; set; }

		[DataMember(Name = "message")]
		public string Message { get; set; }

		[DataMember(Name = "param")]
		public string Parameter { get; set; }
	}
}