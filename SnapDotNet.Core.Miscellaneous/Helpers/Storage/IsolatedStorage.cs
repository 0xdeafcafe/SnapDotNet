using System;
using System.Diagnostics;
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
			try
			{
				var file =
					await ApplicationData.Current.LocalFolder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
				using (var writer = new StreamWriter(await file.OpenStreamForWriteAsync()))
					await writer.WriteAsync(content);
			}
			catch (Exception exception)
			{
				SnazzyDebug.WriteLine(exception);
			}
		}

		public static async Task WriteFileAsync(string fileName, byte[] content)
		{
			try
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
			catch (Exception exception)
			{
				SnazzyDebug.WriteLine(exception);
			}
		}

		public static async Task WriteFileAsync(StorageFile file, byte[] content)
		{
			try
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
			catch (Exception exception)
			{
				SnazzyDebug.WriteLine(exception);
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

		#region Roaming Settings

		public static void WriteRoamingSetting(string containerName, string name, string value)
		{
			WriteSetting(ApplicationData.Current.RoamingSettings, containerName, name, value);
		}

		public static string ReadRoamingSetting(string containerName, string name)
		{
			return ReadSetting(ApplicationData.Current.RoamingSettings, containerName, name);
		}



		public static int ReadRoamingSettingInt(string containerName, string name)
		{
			return int.Parse(ReadSetting(ApplicationData.Current.RoamingSettings, containerName, name));
		}

		public static void DeleteRoamingSetting(string containerName, string name)
		{
			DeleteSetting(ApplicationData.Current.RoamingSettings, containerName, name);
		}

		#endregion

		#region Local Settings

		public static void WriteLocalSetting(string containerName, string name, string value)
		{
			WriteSetting(ApplicationData.Current.LocalSettings, containerName, name, value);
		}

		public static string ReadLocalSettingString(string containerName, string name)
		{
			return ReadSetting(ApplicationData.Current.LocalSettings, containerName, name);
		}

		public static int ReadLocalSettingInt(string containerName, string name, int defualtValue)
		{
			var value = ReadSetting(ApplicationData.Current.LocalSettings, containerName, name);
			return int.Parse(value ?? defualtValue.ToString());
		}

		public static void DeleteLocalSetting(string containerName, string name)
		{
			DeleteSetting(ApplicationData.Current.LocalSettings, containerName, name);
		}

		#endregion

		#region Settings

		private static void WriteSetting(ApplicationDataContainer container, string containerName, string name, string value)
		{
			var selectedcontainer = !container.Containers.ContainsKey(containerName)
				? container.CreateContainer(containerName, ApplicationDataCreateDisposition.Always)
				: container.Containers[containerName];

			if (selectedcontainer.Values.ContainsKey(name))
				selectedcontainer.Values[name] = value;
			else
				selectedcontainer.Values.Add(name, value);
		}

		private static string ReadSetting(ApplicationDataContainer container, string containerName, string name)
		{
			if (!container.Containers.ContainsKey(containerName))
				return null;

			var selectedContainer = container.Containers[containerName];
			if (selectedContainer.Values.ContainsKey(name))
				return (string)selectedContainer.Values[name];

			return null;
		}

		private static void DeleteSetting(ApplicationDataContainer container, string containerName, string name)
		{
			if (!container.Containers.ContainsKey(containerName))
				return;

			var selectedContainer = container.Containers[containerName];
			if (selectedContainer.Values.ContainsKey(name))
				selectedContainer.DeleteContainer(name);
		}

		#endregion
	}
}
