using System;
using System.IO;
using Windows.Storage;

// ReSharper disable once CSharpWarnings::WME006
namespace SnapDotNet.Core.Miscellaneous.Helpers.Storage
{
	public static class IsolatedStorage
	{
		public static async void WriteFile(string fileName, string content)
		{
			var file = await ApplicationData.Current.LocalFolder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
			using (var writer = new StreamWriter(await file.OpenStreamForWriteAsync()))
				await writer.WriteAsync(content);
		}

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
	}
}
