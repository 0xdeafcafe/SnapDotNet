using SnapDotnet.Miscellaneous.Crypto;

namespace SnapDotNet.Core.Snapchat.Helpers
{
	public static class Tokens
	{
		/// <summary>
		///     Generates a Request Token from Post Data and a Static Token
		/// </summary>
		/// <param name="secret">The secret token</param>
		/// <param name="pattern">The Hashing pattern</param>
		/// <param name="postData">The Html Encoded Post Data</param>
		/// <param name="staticToken">The Snapchat Static Token.</param>
		/// <returns>The Request Token, all nice.</returns>
		public static string GenerateRequestToken(string secret, string pattern, string postData, string staticToken)
		{
			var s1 = secret + postData;
			var s2 = staticToken + secret;

			var s3 = Sha.Sha256(s1);
			var s4 = Sha.Sha256(s2);

			var output = "";
			for (var i = 0; i < pattern.Length; i++)
				if (pattern[i] == '0') 
					output += s3[i];
				else 
					output += s4[i];

			return output;
		}
	}
}
