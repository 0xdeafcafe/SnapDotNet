using SnapDotnet.Miscellaneous.Crypto;

namespace SnapDotNet.Core.Snapchat.Api.Helpers
{
	internal static class Tokens
	{
		/// <summary>
		///     Generates a Request Token from Post Data and a Static Token
		/// </summary>
		/// <param name="postData">The Html Encoded Post Data</param>
		/// <param name="staticToken">The Snapchat Static Token.</param>
		/// <returns>The Request Token, all nice.</returns>
		public static string GenerateRequestToken(string postData, string staticToken)
		{
			var s1 = Settings.Secret + postData;
			var s2 = staticToken + Settings.Secret;

			var s3 = Sha.Sha256(s1);
			var s4 = Sha.Sha256(s2);

			var output = "";
			for (var i = 0; i < Settings.HashingPattern.Length; i++)
				if (Settings.HashingPattern[i] == '0') 
					output += s3[i];
				else 
					output += s4[i];

			return output;
		}
	}
}
