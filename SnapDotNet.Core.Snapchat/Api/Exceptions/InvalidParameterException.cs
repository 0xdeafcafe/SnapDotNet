using System;

namespace SnapDotNet.Core.Snapchat.Api.Exceptions
{
	public class InvalidParameterException : Exception
	{
		public InvalidParameterException() { }

		public InvalidParameterException(string message)
			: base(message) { }
	}
}
