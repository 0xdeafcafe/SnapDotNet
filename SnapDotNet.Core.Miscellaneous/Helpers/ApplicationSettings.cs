using System.Runtime.CompilerServices;
using SnapDotNet.Core.Miscellaneous.Helpers.Storage;
using SnapDotNet.Core.Miscellaneous.Models;

namespace SnapDotNet.Core.Miscellaneous.Helpers
{
	public class ApplicationSettings 
		: NotifyPropertyChangedBase
	{
		public ApplicationSettings()
		{
			Load();
		}

		#region Settings

		public SnapAutoDownloadMode SnapAutoDownloadMode
		{
			get { return _snapAutoDownloadMode; }
			set { SetField(ref _snapAutoDownloadMode, value); }
		}
		private SnapAutoDownloadMode _snapAutoDownloadMode = SnapAutoDownloadMode.Wifi;

		public int UnreadSnapCount
		{
			get { return _unreadSnapCount; }
			set { SetField(ref _unreadSnapCount, value); }
		}
		private int _unreadSnapCount;

		#endregion

		#region Helpers

		private void Load()
		{
			SnapAutoDownloadMode = (SnapAutoDownloadMode)LoadSettingInt("SnapAutoDownloadMode", (int)SnapAutoDownloadMode.Wifi);
			UnreadSnapCount = LoadSettingInt("UnreadSnapCount", 0);
		}

		private void Save()
		{
			IsolatedStorage.WriteLocalSetting("ApplicationSettings", "UnreadSnapCount", UnreadSnapCount.ToString());
		}

		private static string LoadSettingString(string settingName, string defaultValue)
		{
			return IsolatedStorage.ReadLocalSettingString("ApplicationSettings", settingName) ?? defaultValue;
		}
		private static int LoadSettingInt(string settingName, int defaultValue)
		{
			return IsolatedStorage.ReadLocalSettingInt("ApplicationSettings", settingName, defaultValue);
		}

		#endregion

		protected new void SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = "")
		{
			field = value;
			NotifyPropertyChanged(propertyName);

			Save();
		}
	}

	public enum SnapAutoDownloadMode
	{
		Never,
		Wifi,
		Cellular,
		Always
	}
}
