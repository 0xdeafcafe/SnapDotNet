using System;

namespace SnapDotNet.Core.Snapchat.Api.Exceptions
{
	public class InvalidRegistrationException : Exception
	{
		public InvalidRegistrationException() { }

		public InvalidRegistrationException(string message)
			: base(message) { }
	}
}
