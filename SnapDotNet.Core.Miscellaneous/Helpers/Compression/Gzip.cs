using System.IO;
using System.IO.Compression;
using System.Text;

namespace SnapDotNet.Core.Miscellaneous.Helpers.Compression
{
	public static class Gzip
	{
		public static byte[] Compress(string text, Encoding encoding = null)
		{
			if (text == null) return null;
			encoding = encoding ?? Encoding.UTF8;
			var textBytes = encoding.GetBytes(text);
			var textStream = new MemoryStream();
			var zip = new GZipStream(textStream, CompressionMode.Compress);
			zip.Write(textBytes, 0, textBytes.Length);
			return textStream.ToArray();
		}

		public static string Decompress(byte[] encrypedBytes, Encoding encoding = null)
		{
			if (encrypedBytes == null) return null;
			encoding = encoding ?? Encoding.UTF8;
			var inputStream = new MemoryStream(encrypedBytes);
			var outputStream = new MemoryStream();
			var zip = new GZipStream(inputStream, CompressionMode.Decompress);
			var bytes = new byte[4096];
			int n;
			while ((n = zip.Read(bytes, 0, bytes.Length)) != 0)
				outputStream.Write(bytes, 0, n);
			return encoding.GetString(outputStream.ToArray(), 0, (int)outputStream.Length);
		}
	}
}
