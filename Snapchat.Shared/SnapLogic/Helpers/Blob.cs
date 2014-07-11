using System;
using System.Threading.Tasks;
using Windows.Foundation.Metadata;
using Windows.Storage;
using SnapDotNet.Core.Miscellaneous.Helpers.Storage;
using SnapDotNet.Core.Miscellaneous.Models;

namespace Snapchat.SnapLogic.Helpers
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

		#region Storage Save

		/// <summary>
		/// 
		/// </summary>
		/// <param name="blob"></param>
		/// <param name="blobId"></param>
		/// <param name="blobType"></param>
		public async static Task SaveBlobToStorageAsync(byte[] blob, string blobId, BlobType blobType)
		{
			await IsolatedStorage.WriteFileAsync(await GetStorageBlobAsync(blobId, blobType), blob);
		}

		#endregion

		#region Storage Read

		/// <summary>
		/// 
		/// </summary>
		/// <param name="blobId"></param>
		/// <param name="blobType"></param>
		public static async Task<byte[]> ReadBlobFromStorageAsync(string blobId, BlobType blobType)
		{
			return await IsolatedStorage.ReadFileToBytesAsync(await GetStorageBlobAsync(blobId, blobType));
		}

		#endregion

		#region Storage Delete

		/// <summary>
		/// 
		/// </summary>
		/// <param name="blobId"></param>
		/// <param name="blobType"></param>
		public static async Task DeleteBlobFromStorageAsync(string blobId, BlobType blobType)
		{
			await IsolatedStorage.DeleteFileAsync(await GetStorageBlobAsync(blobId, blobType));
		}

		#endregion

		#region Storage Contains

		/// <summary>
		/// 
		/// </summary>
		/// <param name="blobId"></param>
		/// <param name="blobType"></param>
		/// <returns></returns>
		public static async Task<bool> StorageContainsBlobAsync(string blobId, BlobType blobType)
		{
			var file = await GetStorageBlobAsync(blobId, blobType);
			if (file == null)
				return false;

			var properties = await file.GetBasicPropertiesAsync();
			return (properties.Size != 0);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="blobId"></param>
		/// <param name="blobType"></param>
		/// <returns></returns>
		[Deprecated("This is actually broken, don't use it", DeprecationType.Deprecate, 0)]
		public static bool StorageContainsBlob(string blobId, BlobType blobType)
		{
			var file = GetStorageBlobAsync(blobId, blobType).Result;
			if (file == null)
				return false;

			var properties = file.GetBasicPropertiesAsync().AsTask().Result;
			return (properties.Size != 0);
		}

		#endregion

		/// <summary>
		/// 
		/// </summary>
		/// <param name="blobId"></param>
		/// <param name="blobType"></param>
		/// <returns></returns>
		private static async Task<StorageFile> GetStorageBlobAsync(string blobId, BlobType blobType)
		{
			var blobFolder =
				await ApplicationData.Current.LocalFolder.CreateFolderAsync("BlobStorage", CreationCollisionOption.OpenIfExists);

			var mediaFolder =
				await blobFolder.CreateFolderAsync(blobType.ToString(), CreationCollisionOption.OpenIfExists);

			return await mediaFolder.CreateFileAsync(blobId + ".blob", CreationCollisionOption.OpenIfExists);
		}
	}
}
