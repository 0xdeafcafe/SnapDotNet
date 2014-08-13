using System;

namespace SnapDotNet.Utilities
{
	public static class Timestamps
	{
		public static readonly DateTime Epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

		public static long ToJScriptTime(this DateTime value)
		{
			return Convert.ToInt64((value - Epoch).TotalMilliseconds);
		}

		public static int ToUnixTime(this DateTime value)
		{
			return Convert.ToInt32((value - Epoch).TotalSeconds);
		}

		public static DateTime FromJScriptTime(long jscriptTime)
		{
			return Epoch.AddMilliseconds(jscriptTime);
		}
	}
}
