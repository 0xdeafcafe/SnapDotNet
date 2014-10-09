using System;
using System.Diagnostics.Contracts;

namespace SnapDotNet
{
	public static class ApiSettings
	{
		/// <summary>
		/// Indicates the duration (in hours) before replaying a Snap is permitted.
		/// </summary>
		public const int ReplayCooldownDuration = 24;

		internal static readonly string HashingPattern = "0001110111101110001111010101111011010001001110011000110001000110";
		internal static readonly string Secret = "iEk21fuwZApXlz93750dmW22pw389dPwOk";
		internal static readonly string StaticToken = "m198sOkJEn37DjqZ32lpRu76xmw288xSQ9";
		internal static readonly string UserAgent = "User-Agent: Snapchat/5.0.5 (Nexus 4; Android 19; gzip)";
	}
}