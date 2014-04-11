using System;
using SnapDotNet.Core.Miscellaneous.Crypto;
using SnapDotNet.Core.Snapchat.Api;

namespace SnapDotNet.Core.Snapchat.Helpers
{
	public static class Blob
	{
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public static bool ValidateMediaBlob(byte[] data)
		{
			if (data == null)
				return false;

			if (data.Length < 2)
				return false;

			return (data[0] == 0xFF && data[1] == 0xD8) ||
				   (data[0] == 0x00 && data[1] == 0x00);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public static byte[] DecryptBlob(byte[] data)
		{
			try
			{
				data = Aes.DecryptData(data, Convert.FromBase64String(Settings.BlobEncryptionKey));
				return ValidateMediaBlob(data) ? data : null;
			}
			catch
			{
				return null;
			}
		}
	}
}
