using System;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;

namespace SnapDotNet.Core.Miscellaneous.Extensions
{
	public static class ByteArrayExtensions
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="byteArray"></param>
		/// <returns></returns>
		public static BitmapImage ToBitmapImage(this byte[] byteArray)
		{
			if (byteArray == null) return null;

			using (var stream = new InMemoryRandomAccessStream())
			{
				// Writes the image byte array in an InMemoryRandomAccessStream
				// that is needed to set the source of BitmapImage.
				using (var writer = new DataWriter(stream.GetOutputStreamAt(0)))
				{
					writer.WriteBytes(byteArray);
					writer.StoreAsync().GetResults();
				}

				var image = new BitmapImage();
				image.SetSource(stream);
				return image;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="byteArray"></param>
		/// <returns></returns>
		public static async Task<InMemoryRandomAccessStream> ToInMemoryRandomAccessStream(this byte[] byteArray)
		{
			var randomAccessStream = new InMemoryRandomAccessStream();
			await randomAccessStream.WriteAsync(byteArray.AsBuffer());
			randomAccessStream.Seek(0);
			return randomAccessStream;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="byteArray"></param>
		/// <returns></returns>
		public static Stream ToStream(this byte[] byteArray)
		{
			var memoryStream = new MemoryStream();
			memoryStream.Write(byteArray, 0, byteArray.Length);
			return memoryStream;
		}
	}
}
