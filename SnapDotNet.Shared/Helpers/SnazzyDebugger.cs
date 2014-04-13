using System;
using System.Diagnostics;

namespace SnapDotNet.Apps.Helpers
{
	public static class SnazzyDebug
	{
		public static void WriteLine(Exception exception)
		{
			Debug.WriteLine(exception.ToString());
#if DEBUG
			throw exception;
#endif
		}
	}
}
