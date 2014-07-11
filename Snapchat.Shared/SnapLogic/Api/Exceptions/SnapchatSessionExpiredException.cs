using System;

namespace Snapchat.SnapLogic.Api.Exceptions
{
	public class SnapchatSessionExpiredException : Exception
	{
		public SnapchatSessionExpiredException() { }

		public SnapchatSessionExpiredException(string message)
			: base(message) { }
	}
}
