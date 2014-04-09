using System;
using System.IO;
using System.Text;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;

namespace SnapDotNet.Core.Miscellaneous.Crypto
{
	/// <summary>
	/// </summary>
	public static class Aes
	{
		/// <summary>
		/// </summary>
		/// <param name="data"></param>
		/// <param name="key"></param>
		/// <returns></returns>
		public static byte[] DecryptData(byte[] data, string key)
		{
			var encKey = Encoding.UTF8.GetBytes(key);

			// AES algorthim with ECB cipher & PKCS5 padding...
			var cipher = CipherUtilities.GetCipher("AES/ECB/PKCS5Padding");

			// Initialise the cipher...
			cipher.Init(false, new KeyParameter(encKey));

			// Decrypt the data and write the 'final' byte stream...
			var decryptionbytes = cipher.ProcessBytes(data);
			var decryptedfinal = cipher.DoFinal();

			// Write the decrypt bytes & final to memory...
			var decryptedstream = new MemoryStream(decryptionbytes.Length);
			decryptedstream.Write(decryptionbytes, 0, decryptionbytes.Length);
			decryptedstream.Write(decryptedfinal, 0, decryptedfinal.Length);
			decryptedstream.Flush();

			var decryptedData = new byte[decryptedstream.Length];
			decryptedstream.Position = 0;
			decryptedstream.Read(decryptedData, 0, (int)decryptedstream.Length);

			return decryptedData;
		}

		/// <summary>
		/// </summary>
		/// <param name="data"></param>
		/// <param name="key"></param>
		/// <returns></returns>
		public static byte[] EncryptData(byte[] data, string key)
		{
			throw new NotImplementedException();
		}
	}
}