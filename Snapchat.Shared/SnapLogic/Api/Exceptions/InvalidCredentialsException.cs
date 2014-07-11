using System;

namespace Snapchat.SnapLogic.Api.Exceptions
{
	public class InvalidCredentialsException : Exception
	{
		public InvalidCredentialsException() { }

		public InvalidCredentialsException(string message)
			: base(message) { }
	}
}
