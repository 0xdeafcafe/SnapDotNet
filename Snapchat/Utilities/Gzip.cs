using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Cryptography;
using Windows.Storage.Streams;

namespace SnapDotNet.Utilities
{
	/// <summary>
	/// Provides static methods for compressing and decompressing data using GZip.
	/// </summary>
	public static class GZip
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="data"></param>
		/// <returns></returns>
		public static async Task<byte[]> DecompressAsync(IBuffer data)
		{
			Contract.Requires<ArgumentNullException>(data != null);

			byte[] buffer;
			CryptographicBuffer.CopyToByteArray(data, out buffer);
			return await DecompressAsync(buffer);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="data"></param>
		/// <returns></returns>
		public static async Task<byte[]> DecompressAsync(byte[] data)
		{
			Contract.Requires<ArgumentNullException>(data != null);

			using (var stream = new MemoryStream(data))
			using (var gzip = new GZipStream(stream, CompressionMode.Decompress))
			using (var outputStream = new MemoryStream())
			{
				await gzip.CopyToAsync(outputStream);
				return outputStream.ToArray();
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="data"></param>
		/// <param name="encoding"></param>
		/// <returns></returns>
		public static async Task<string> DecompressToStringAsync(IBuffer data, Encoding encoding)
		{
			Contract.Requires<ArgumentNullException>(data != null && encoding != null);
			return encoding.GetString(await DecompressAsync(data), 0, unchecked((int) data.Length));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="data"></param>
		/// <returns></returns>
		public static async Task<byte[]> CompressAsync(byte[] data)
		{
			Contract.Requires<ArgumentNullException>(data != null);

			using (var outputStream = new MemoryStream())
			using (var gzip = new GZipStream(outputStream, CompressionMode.Compress))
			{
				await gzip.WriteAsync(data, 0, data.Length);
				return outputStream.ToArray();
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="data"></param>
		/// <param name="encoding"></param>
		/// <returns></returns>
		public static async Task<byte[]> CompressAsync(string data, Encoding encoding)
		{
			Contract.Requires<ArgumentNullException>(data != null && encoding != null);
			return await CompressAsync(encoding.GetBytes(data));
		}
	}
}
