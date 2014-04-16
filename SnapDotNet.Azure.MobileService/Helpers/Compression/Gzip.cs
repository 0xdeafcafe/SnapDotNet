using System.IO;
using System.IO.Compression;
using System.Text;

namespace SnapDotNet.Azure.MobileService.Helpers.Compression
{
	public static class Gzip
	{
		public static byte[] Compress(byte[] decompressedData)
		{
			if (decompressedData == null) return null;
			var textStream = new MemoryStream();
			var zip = new GZipStream(textStream, CompressionMode.Compress);
			zip.Write(decompressedData, 0, decompressedData.Length);
			return textStream.ToArray();
		}

		public static byte[] CompressToString(string decompressedData, Encoding encoding = null)
		{
			encoding = encoding ?? Encoding.UTF8;
			var textBytes = encoding.GetBytes(decompressedData);
			return Compress(textBytes);
		}


		public static byte[] Decompress(byte[] compressedData)
		{
			if (compressedData == null) return null;
			var inputStream = new MemoryStream(compressedData);
			var outputStream = new MemoryStream();
			var zip = new GZipStream(inputStream, CompressionMode.Decompress);
			var bytes = new byte[4096];
			int n;
			while ((n = zip.Read(bytes, 0, bytes.Length)) != 0)
				outputStream.Write(bytes, 0, n);
			return outputStream.ToArray();
		}

		public static string DecompressToString(byte[] compressedData, Encoding encoding = null)
		{
			encoding = encoding ?? Encoding.UTF8;
			var decompressedData = Decompress(compressedData);
			return encoding.GetString(decompressedData, 0, decompressedData.Length);
		}
	}
}
