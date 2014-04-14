using System.Collections.ObjectModel;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;

namespace SnapDotNet.Core.Miscellaneous.Helpers.Compression
{
	public static class Zip
	{
		public async static Task<ObservableCollection<byte[]>> ExtractAllFilesAsync(byte[] zipFile)
		{
			var files = new ObservableCollection<byte[]>();

			var stream = new MemoryStream(zipFile);
			var zip = new ZipArchive(stream, ZipArchiveMode.Update);

			foreach (var file in zip.Entries)
			{
				var buffer = new byte[file.Length];
				var fileStream = file.Open();

				await fileStream.ReadAsync(buffer, 0, buffer.Length);
				files.Add(buffer);
			}

			return files;
		}
	}
}
