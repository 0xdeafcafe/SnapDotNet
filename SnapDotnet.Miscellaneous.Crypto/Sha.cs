using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;

namespace SnapDotnet.Miscellaneous.Crypto
{
	public class Sha
	{
		/// <summary>
		/// </summary>
		/// <param name="data"></param>
		/// <returns></returns>
		public static string Sha256(string data)
		{
			var input = CryptographicBuffer.ConvertStringToBinary(data, BinaryStringEncoding.Utf8);

			var hasher = HashAlgorithmProvider.OpenAlgorithm("SHA256");
			var hashed = hasher.HashData(input);

			return CryptographicBuffer.EncodeToHexString(hashed);
		}
	}
}
