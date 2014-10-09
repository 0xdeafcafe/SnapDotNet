using Windows.Storage;

namespace ColdSnap
{
	public static class AppSettings
	{
		private static readonly ApplicationDataContainer Container;

		static AppSettings()
		{
			Container = ApplicationData.Current.RoamingSettings.CreateContainer("AppSettings", ApplicationDataCreateDisposition.Always);
		}

		public static T Get<T>(string key, T defaultValue = default(T))
		{
			if (Container.Values.ContainsKey(key))
				return (T) Container.Values[key];

			return defaultValue;
		}

		public static void Set<T>(string key, T value)
		{
			Container.Values[key] = value;
		}
	}
}