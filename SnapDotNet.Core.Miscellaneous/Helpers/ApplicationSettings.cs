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
			set
			{
				SetField(ref _snapAutoDownloadMode, value);
				IsolatedStorage.WriteLocalSetting("ApplicationSettings", "SnapAutoDownloadMode", ((int)value).ToString());
			}
		}
		private SnapAutoDownloadMode _snapAutoDownloadMode = SnapAutoDownloadMode.Wifi;

		public uint UnreadSnapCount
		{
			get { return _unreadSnapCount; }
			set
			{
				SetField(ref _unreadSnapCount, value);
				IsolatedStorage.WriteLocalSetting("ApplicationSettings", "UnreadSnapCount", value.ToString());
			}
		}
		private uint _unreadSnapCount;

		#endregion

		#region Helpers

		private void Load()
		{
			SnapAutoDownloadMode = (SnapAutoDownloadMode)LoadSettingInt("SnapAutoDownloadMode", (int)SnapAutoDownloadMode.Wifi);
			UnreadSnapCount = (uint)LoadSettingInt("UnreadSnapCount", 0);
		}

		private static string LoadSettingString(string settingName, string defaultValue)
		{
			return IsolatedStorage.ReadLocalSettingString("ApplicationSettings", settingName) ?? defaultValue;
		}

		private static int LoadSettingInt(string settingName, int defaultValue)
		{
			return IsolatedStorage.ReadLocalSettingInt("ApplicationSettings", settingName, defaultValue);
		}

		protected new void SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = "")
		{
			field = value;
			NotifyPropertyChanged(propertyName);
		}

		#endregion
	}

	public enum SnapAutoDownloadMode
	{
		Never,
		Wifi,
		Cellular,
		Always
	}
}
