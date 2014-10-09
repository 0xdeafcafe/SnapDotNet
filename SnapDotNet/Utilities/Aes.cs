using System.IO;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;

namespace SnapDotNet.Utilities
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
		public static byte[] DecryptData(byte[] data, byte[] key)
		{
			// AES algorthim with ECB cipher & PKCS5 padding...
			var cipher = CipherUtilities.GetCipher("AES/ECB/PKCS5Padding");

			// Initialise the cipher...
			cipher.Init(false, new KeyParameter(key));

			// Decrypt the data and write the 'final' byte stream...
			var decryptionBytes = cipher.ProcessBytes(data);
			var decryptedFinal = cipher.DoFinal();

			// Write the decrypt bytes & final to memory...
			var decryptedStream = new MemoryStream(decryptionBytes.Length);
			decryptedStream.Write(decryptionBytes, 0, decryptionBytes.Length);
			decryptedStream.Write(decryptedFinal, 0, decryptedFinal.Length);
			decryptedStream.Flush();

			var decryptedData = new byte[decryptedStream.Length];
			decryptedStream.Position = 0;
			decryptedStream.Read(decryptedData, 0, (int) decryptedStream.Length);

			return decryptedData;

		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="data"></param>
		/// <param name="key"></param>
		/// <param name="iv"></param>
		/// <returns></returns>
		public static byte[] DecryptDataWithIv(byte[] data, byte[] key, byte[] iv)
		{
			// AES algorthim with CBC cipher & PKCS5 padding...
			var cipher = CipherUtilities.GetCipher("AES/CBC/PKCS5Padding");

			// Initialise the cipher...
			cipher.Init(false, new ParametersWithIV(new KeyParameter(key), iv));

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
			decryptedstream.Read(decryptedData, 0, (int) decryptedstream.Length);

			return decryptedData;
		}

		/// <summary>
		/// </summary>
		/// <param name="data"></param>
		/// <param name="key"></param>
		/// <returns></returns>
		public static Stream EncryptData(byte[] data, byte[] key)
		{
			// AES algorthim with ECB cipher & PKCS5 padding...
			var cipher = CipherUtilities.GetCipher("AES/ECB/PKCS5Padding");

			cipher.Init(true, new KeyParameter(key));

			// Encrypt the data and write the 'final' byte stream...
			var encryptionBytes = cipher.ProcessBytes(data);
			var encryptedFinal = cipher.DoFinal();

			// Write the encrypt bytes & final to memory...
			var enryptedStream = new MemoryStream(encryptionBytes.Length);
			enryptedStream.Write(encryptionBytes, 0, encryptionBytes.Length);
			enryptedStream.Write(encryptedFinal, 0, encryptedFinal.Length);
			enryptedStream.Seek(0, SeekOrigin.Begin);
			return enryptedStream;
		}
	}
}