using System;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage.Streams;

namespace SnapDotNet.Core.Miscellaneous.Extensions
{
	public static class MemoryRandomAccessStreamExtentions
	{
		public static async Task<Byte[]> ToArray(this IRandomAccessStream stream)
		{
			var buffer = new byte[stream.Size];
			await stream.AsStream().ReadAsync(buffer, 0, buffer.Length);
			return buffer;
		}
	}
}
