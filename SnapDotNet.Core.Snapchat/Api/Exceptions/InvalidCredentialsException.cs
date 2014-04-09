using System;

namespace SnapDotNet.Core.Snapchat.Api.Exceptions
{
	public class InvalidCredentialsException : Exception
	{
		public InvalidCredentialsException() { }

		public InvalidCredentialsException(string message)
			: base(message) { }
	}
}
