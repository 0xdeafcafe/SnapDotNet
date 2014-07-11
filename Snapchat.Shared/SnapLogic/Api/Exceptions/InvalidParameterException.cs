using System;

namespace Snapchat.SnapLogic.Api.Exceptions
{
	public class InvalidParameterException : Exception
	{
		public InvalidParameterException() { }

		public InvalidParameterException(string message)
			: base(message) { }
	}
}
