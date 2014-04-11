using System;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Windows.Storage.Streams;
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

		public static byte[] DecryptDataWithIv(byte[] data, byte[] key, byte[] iv)
		{
			// AES algorthim with ECB cipher & PKCS5 padding...
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

		// TODO: rewrite this later
		//public static byte[] Encrypt(byte[] data, string key, string iv)
		//{
		//	var pwBuffer = CryptographicBuffer.ConvertStringToBinary(pw, BinaryStringEncoding.Utf8);
		//	var saltBuffer = CryptographicBuffer.ConvertStringToBinary(salt, BinaryStringEncoding.Utf16LE);

		//	// Derive key material for password size 32 bytes for AES256 algorithm
		//	var keyDerivationProvider = Windows.Security.Cryptography.Core.KeyDerivationAlgorithmProvider.OpenAlgorithm("PBKDF2_SHA1");
		//	// using salt and 1000 iterations
		//	var pbkdf2Parms = KeyDerivationParameters.BuildForPbkdf2(saltBuffer, 1000);

		//	// create a key based on original key and derivation parmaters
		//	var keyOriginal = keyDerivationProvider.CreateKey(pwBuffer);
		//	var keyMaterial = CryptographicEngine.DeriveKeyMaterial(keyOriginal, pbkdf2Parms, 32);
		//	var derivedPwKey = keyDerivationProvider.CreateKey(pwBuffer);

		//	// derive buffer to be used for encryption salt from derived password key 
		//	var saltMaterial = CryptographicEngine.DeriveKeyMaterial(derivedPwKey, pbkdf2Parms, 16);

		//	// display the buffers – because KeyDerivationProvider always gets cleared after each use, they are very similar unforunately
		//	var keyMaterialString = CryptographicBuffer.EncodeToBase64String(keyMaterial);
		//	var saltMaterialString = CryptographicBuffer.EncodeToBase64String(saltMaterial);

		//	var symProvider = SymmetricKeyAlgorithmProvider.OpenAlgorithm("AES_CBC_PKCS7");
		//	// create symmetric key from derived password key
		//	var symmKey = symProvider.CreateSymmetricKey(keyMaterial);

		//	// encrypt data buffer using symmetric key and derived salt material
		//	var resultBuffer = CryptographicEngine.Encrypt(symmKey, data, saltMaterial);
		//	byte[] result;
		//	CryptographicBuffer.CopyToByteArray(resultBuffer, out result);
		//	return result;
		//}
	}
}