using System;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;

namespace SnapDotNet.Core.Miscellaneous.Helpers.Storage
{
	public static class IsolatedStorage
	{
		#region Write

		public static async void WriteFileAsync(string fileName, string content)
		{
			var file = await ApplicationData.Current.LocalFolder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
			using (var writer = new StreamWriter(await file.OpenStreamForWriteAsync()))
				await writer.WriteAsync(content);
		}

		public static async Task WriteFileAsync(string fileName, byte[] content)
		{
			var file = await ApplicationData.Current.LocalFolder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);

			using (var fileStream = await file.OpenAsync(FileAccessMode.ReadWrite))
			{
				using (var outputStream = fileStream.GetOutputStreamAt(0))
				{
					using (var dataWriter = new DataWriter(outputStream))
					{
						dataWriter.WriteBytes(content);
						await dataWriter.StoreAsync();
						dataWriter.DetachStream();
					}
					await outputStream.FlushAsync();
				}
			}
		}

		public static async Task WriteFileAsync(StorageFile file, byte[] content)
		{
			using (var fileStream = await file.OpenAsync(FileAccessMode.ReadWrite))
			{
				using (var outputStream = fileStream.GetOutputStreamAt(0))
				{
					using (var dataWriter = new DataWriter(outputStream))
					{
						dataWriter.WriteBytes(content);
						await dataWriter.StoreAsync();
						dataWriter.DetachStream();
					}
					await outputStream.FlushAsync();
				}
			}
		}

		#endregion

		#region Read

		public static async Task<string> ReadFileAsync(string fileName)
		{
			try
			{
				var file = await ApplicationData.Current.LocalFolder.GetFileAsync(fileName);
				return file == null ? null : await FileIO.ReadTextAsync(file);
			}
			catch (Exception)
			{
				return null;
			}
		}

		public static async Task<byte[]> ReadFileToBytesAsync(string fileName)
		{
			try
			{
				var file = await ApplicationData.Current.LocalFolder.GetFileAsync(fileName);
				using (var stream = await file.OpenAsync(FileAccessMode.Read))
				using (var inputStream = stream.GetInputStreamAt(0))
				using (var reader = new DataReader(inputStream))
				{
					var data = new byte[stream.Size];
					await reader.LoadAsync((uint)data.Length);
					reader.ReadBytes(data);
					return data;
				}
			}
			catch (Exception)
			{
				return null;
			}
		}

		public static async Task<byte[]> ReadFileToBytesAsync(StorageFile file)
		{
			try
			{
				using (var stream = await file.OpenAsync(FileAccessMode.Read))
				using (var inputStream = stream.GetInputStreamAt(0))
				using (var reader = new DataReader(inputStream))
				{
					var data = new byte[stream.Size];
					await reader.LoadAsync((uint)data.Length);
					reader.ReadBytes(data);
					return data;
				}
			}
			catch (Exception)
			{
				return null;
			}
		}

		#endregion

		public static async Task<bool> FileExistsAsync(string fileName)
		{
			var file = await ApplicationData.Current.LocalFolder.GetFileAsync(fileName);
			return (file != null);
		}

		#region Delete

		public static async Task DeleteFileAsync(StorageFile file)
		{
			if (file != null)
				await file.DeleteAsync();
		}

		public static async Task DeleteFileAsync(string fileName)
		{
			var file = await ApplicationData.Current.LocalFolder.GetFileAsync(fileName);
			if (file != null)
				await file.DeleteAsync();
		}

		#endregion

		public static void WriteSetting(string containerName, string name, string value)
		{
			var container = !ApplicationData.Current.RoamingSettings.Containers.ContainsKey(containerName)
				? ApplicationData.Current.RoamingSettings.CreateContainer(containerName, ApplicationDataCreateDisposition.Always)
				: ApplicationData.Current.RoamingSettings.Containers[containerName];

			if (container.Values.ContainsKey(name))
				container.Values[name] = value;
			else
				container.Values.Add(name, value);
		}

		public static string ReadSetting(string containerName, string name)
		{
			if (!ApplicationData.Current.RoamingSettings.Containers.ContainsKey(containerName))
				return null;

			var container = ApplicationData.Current.RoamingSettings.Containers[containerName];
			if (container.Values.ContainsKey(name))
				return (string) container.Values[name];
			
			return null;
		}

		public static void DeleteSetting(string containerName, string name)
		{
			if (!ApplicationData.Current.RoamingSettings.Containers.ContainsKey(containerName)) 
				return;

			var container = ApplicationData.Current.RoamingSettings.Containers[containerName];
			if (container.Values.ContainsKey(name))
				container.DeleteContainer(name);
		}
	}
}
