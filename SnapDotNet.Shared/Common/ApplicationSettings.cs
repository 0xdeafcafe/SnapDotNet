using System.Runtime.CompilerServices;

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
		private SnapAutoDownloadMode _snapAutoDownloadMode;

		#endregion

		private void Load()
		{
			
		}

		private void Save()
		{
			
		}

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
