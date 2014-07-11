using System;

namespace Snapchat.SnapLogic.Api.Exceptions
{
	public class InvalidBlobDataException : Exception
	{
		public InvalidBlobDataException() { }

		public InvalidBlobDataException(string message)
			: base(message) { }
	}
}
