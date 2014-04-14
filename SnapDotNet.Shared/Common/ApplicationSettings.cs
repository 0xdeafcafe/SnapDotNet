using System.Runtime.CompilerServices;
using SnapDotNet.Core.Miscellaneous.Helpers.Storage;

namespace SnapDotNet.Apps.Common
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

		#endregion

		#region Helpers

		private void Load()
		{
			SnapAutoDownloadMode = (SnapAutoDownloadMode) LoadSettingInt("SnapAutoDownloadMode", (int) SnapAutoDownloadMode.Wifi);
		}

		private static string LoadSettingString(string settingName, string defaultValue)
		{
			return IsolatedStorage.ReadLocalSettingString("ApplicationSettings", settingName) ?? defaultValue;
		}
		private static int LoadSettingInt(string settingName, int defaultValue)
		{
			return IsolatedStorage.ReadLocalSettingInt("ApplicationSettings", settingName, defaultValue);
		}

		private void Save()
		{
			IsolatedStorage.WriteLocalSetting("ApplicationSettings", "SnapAutoDownloadMode", ((int)SnapAutoDownloadMode).ToString());
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
