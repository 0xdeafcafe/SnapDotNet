using System;
using Windows.Storage;

namespace Snapchat.Helpers
{
    public static class AppSettings
    {
		private static readonly ApplicationDataContainer _container;

		static AppSettings()
		{
			_container = ApplicationData.Current.RoamingSettings.CreateContainer("AppSettings", ApplicationDataCreateDisposition.Always);
		}

		public static T Get<T>(string key, T defaultValue = default(T))
		{
			if (_container.Values.ContainsKey(key))
				return (T) _container.Values[key];

			return defaultValue;
		}

		public static void Set<T>(string key, T value)
		{
			_container.Values[key] = value;
		}
    }
}
