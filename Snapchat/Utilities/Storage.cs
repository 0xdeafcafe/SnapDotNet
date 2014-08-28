using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Storage;
using Newtonsoft.Json;
using UnicodeEncoding = Windows.Storage.Streams.UnicodeEncoding;

namespace SnapDotNet.Utilities
{
	public class StorageManager
	{
		public static readonly StorageManager Local = new StorageManager(ApplicationData.Current.LocalFolder);

		public StorageManager(StorageFolder storageFolder)
		{
			StorageFolder = storageFolder;
			LoadStorageObjects();
		}

		private const string StorageObjectsFileName = "storage-objects";
		private List<StorageObject> _storageObjects = new List<StorageObject>();
		public readonly StorageFolder StorageFolder;

		#region Storage Object Actions

		public async void LoadStorageObjects()
		{
			Debug.WriteLine("[Storage Manager] Loading Storage Objects");
			if (!await FileExistsAsync(StorageObjectsFileName)) return;
			var raw = await ReadTextAsync(await StorageFolder.GetFileAsync(StorageObjectsFileName));
			if (raw == null) return;
			_storageObjects = await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<List<StorageObject>>(raw)) ??
			                  new List<StorageObject>();
		}

		public async void SaveStorageObjects()
		{
			Debug.WriteLine("[Storage Manager] Saving Storage Objects");
			var x = new List<StorageObject>(_storageObjects);
			var json = await Task.Factory.StartNew(() => JsonConvert.SerializeObject(x));
			var file = await StorageFolder.CreateFileAsync(StorageObjectsFileName, CreationCollisionOption.ReplaceExisting);
			await WriteTextAsync(file, json, UnicodeEncoding.Utf8);
		}

		public void AddStorageObject(StorageObject storageObject)
		{
			if (_storageObjects == null)
				_storageObjects = new List<StorageObject>();

			Debug.WriteLine("[Storage Manager] Adding Storage Object with Id: {0} | Type: {1}", storageObject.SnapchatId, storageObject.StorageType);
			_storageObjects.Add(storageObject);
			SaveStorageObjects();
		}

		public void RemoveStorageObject(StorageObject storageObject)
		{
			if (_storageObjects == null)
				_storageObjects = new List<StorageObject>();

			Debug.WriteLine("[Storage Manager] Removing Storage Object with Id: {0} | Type: {1}", storageObject.SnapchatId, storageObject.StorageType);
			if (_storageObjects.Contains(storageObject))
				_storageObjects.Remove(storageObject);
			SaveStorageObjects();
		}

		public StorageObject RetrieveStorageObject(string snapchatId, StorageType storageType)
		{
			if (_storageObjects == null)
				_storageObjects = new List<StorageObject>();

			return _storageObjects.FirstOrDefault(s => s.SnapchatId == snapchatId && s.StorageType == storageType);
		}

		#endregion

		#region IO Actions

		#region Read Actions

		public async Task<string> ReadTextAsync(IStorageFile file)
		{
			try
			{
				return await FileIO.ReadTextAsync(file);
			}
			catch (Exception ex)
			{

				Debug.WriteLine("[Storage Manager] Error Reading Text: {0}", ex);
				return null;
			}
		}

		public async Task<byte[]> ReadBytesAsync(IStorageFile file)
		{
			try
			{
				var data = await FileIO.ReadBufferAsync(file);
				return data.ToArray();
			}
			catch (Exception ex)
			{

				Debug.WriteLine("[Storage Manager] Error Reading Byte[]: {0}", ex);
				return null;
			}
		}

		#endregion

		#region Write Actions

		public async Task WriteTextAsync(IStorageFile file, string content, UnicodeEncoding encoding)
		{
			try
			{
				await FileIO.WriteTextAsync(file, content, encoding);
			}
			catch (UnauthorizedAccessException)
			{
				
			}
			catch (Exception ex)
			{
				Debug.WriteLine("[Storage Manager] Error Writing Text: {0}", ex);
			}
		}

		public async Task WriteBytesAsync(IStorageFile file, byte[] data)
		{
			try
			{
				await FileIO.WriteBytesAsync(file, data);
			}
			catch (Exception ex)
			{
				Debug.WriteLine("[Storage Manager] Error Writing Byte[]: {0}", ex);
			}
		}

		#endregion

		#region Other IO Actions

		public async Task<bool> FileExistsAsync(string fileName)
		{
			try
			{
				await StorageFolder.GetFileAsync(fileName);
				return true;
			}
			catch
			{
				return false;
			}
		}

		#endregion

		#endregion
	}

	public enum StorageType
	{
		Story,
		StoryThumbnail,
		Snap,
		Chat
	}

	public class StorageObject
	{
		public StorageType StorageType { get; set; }

		public string SnapchatId { get; set; }

		public DateTime ExpiresAt { get; set; }

		public async void WriteDataAsync(byte[] data)
		{
			var fileName = GenerateFileName();
			var file = await StorageManager.Local.StorageFolder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
			await StorageManager.Local.WriteBytesAsync(file, data);
		}

		public async Task<byte[]> ReadDataAsync()
		{
			if (!await StorageManager.Local.FileExistsAsync(GenerateFileName()))
				return null;

			var file = await StorageManager.Local.StorageFolder.GetFileAsync(GenerateFileName());
			return await StorageManager.Local.ReadBytesAsync(file);
		}

		public string GenerateFileName()
		{
			return String.Format("{0}-{1}", SnapchatId, StorageType);
		}
	}
}
