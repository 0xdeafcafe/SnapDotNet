using System;

namespace SnapDotNet.Core.Snapchat.Api.Exceptions
{
	public class InvalidBlobDataException : Exception
	{
		public InvalidBlobDataException() { }

		public InvalidBlobDataException(string message)
			: base(message) { }
	}
}
