using System;

namespace Snapchat.SnapLogic.Api.Exceptions
{
	public class InvalidRegistrationException : Exception
	{
		public InvalidRegistrationException() { }

		public InvalidRegistrationException(string message)
			: base(message) { }
	}
}
