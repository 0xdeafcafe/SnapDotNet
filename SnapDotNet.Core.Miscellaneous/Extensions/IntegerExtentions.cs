using System;

namespace SnapDotNet.Core.Miscellaneous.Extensions
{
	public static class IntegerExtentions
	{
		public static string ToDelimiter(this Int32 number)
		{
			return number >= 1000 ? number.ToString("n0") : number.ToString("d");
		}
	}
}
