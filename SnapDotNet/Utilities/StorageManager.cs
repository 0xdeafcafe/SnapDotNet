using System;
using System.Diagnostics.Contracts;
using System.IO;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Windows.Storage;

namespace SnapDotNet.Utilities
{
	public sealed class StorageManager
	{
		/// <summary>
		/// Gets a <see cref="StorageManager"/> instance for the local app folder.
		/// </summary>
		public static readonly StorageManager Local = new StorageManager(ApplicationData.Current.LocalFolder);

		/// <summary>
		/// Gets a <see cref="StorageManager"/> instance for the roaming app folder.
		/// </summary>
		public static readonly StorageManager Roaming = new StorageManager(ApplicationData.Current.RoamingFolder);

		/// <summary>
		/// Initializes a new instance of the <see cref="StorageManager"/> class that manages data
		/// stored at the specified <paramref name="location"/>.
		/// </summary>
		/// <param name="location">
		/// The folder where the state managed by this instance is stored.
		/// </param>
		public StorageManager(StorageFolder location)
		{
			Contract.Requires<ArgumentNullException>(location != null);
			Location = location;
		}

		/// <summary>
		/// Gets the folder where the data managed by this instance is stored.
		/// </summary>
		public StorageFolder Location { get; private set; }

		/// <summary>
		/// Deletes all files and sub-folders in the location managed by this instance.
		/// </summary>
		/// <returns></returns>
		public async Task EmptyFolderAsync()
		{
			var files = await Location.GetFilesAsync();
			foreach (var file in files)
				await file.DeleteAsync(StorageDeleteOption.PermanentDelete);

			var folders = await Location.GetFoldersAsync();
			foreach (var folder in folders)
				await folder.DeleteAsync(StorageDeleteOption.PermanentDelete);
		}

		/// <summary>
		/// Checks whether a file exists.
		/// </summary>
		/// <param name="name">The name of the file.</param>
		/// <returns><c>true</c> if it exists; otherwise, <c>false</c>.</returns>
		public async Task<bool> FileExistsAsync(string name)
		{
			Contract.Requires<ArgumentNullException>(name != null);

			try
			{
				await Location.GetFileAsync(name);
				return true;
			}
			catch (FileNotFoundException)
			{
				return false;
			}
		}
	}
}
