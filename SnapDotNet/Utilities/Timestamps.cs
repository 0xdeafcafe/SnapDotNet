using System;

namespace SnapDotNet.Utilities
{
	/// <summary>
	/// Provides utility methods used to convert between various timestamp formats.
	/// </summary>
	public static class Timestamps
	{
		/// <summary>
		/// Represents the Unix epoch.
		/// </summary>
		public static readonly DateTime Epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

		/// <summary>
		/// Converts this <see cref="DateTime"/> object into a JScript-based timestamp.
		/// </summary>
		/// <returns></returns>
		public static long ToJScriptTime(this DateTime value)
		{
			return Convert.ToInt64((value - Epoch).TotalMilliseconds);
		}

		/// <summary>
		/// Converts this <see cref="DateTime"/> object into a Unix-based timestamp.
		/// </summary>
		/// <returns></returns>
		public static int ToUnixTime(this DateTime value)
		{
			return Convert.ToInt32((value - Epoch).TotalSeconds);
		}

		/// <summary>
		/// Converts a JScript-based timestamp into a <see cref="DateTime"/> object.
		/// </summary>
		/// <returns></returns>
		public static DateTime FromJScriptTime(long jscriptTime)
		{
			return Epoch.AddMilliseconds(jscriptTime);
		}
	}
}