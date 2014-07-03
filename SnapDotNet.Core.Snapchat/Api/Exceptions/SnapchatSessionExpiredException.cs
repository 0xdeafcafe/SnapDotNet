using System;

namespace SnapDotNet.Core.Snapchat.Api.Exceptions
{
	public class SnapchatSessionExpiredException : Exception
	{
		public SnapchatSessionExpiredException() { }

		public SnapchatSessionExpiredException(string message)
			: base(message) { }
	}
}
