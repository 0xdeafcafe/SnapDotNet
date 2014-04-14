namespace SnapDotNet.Core.Miscellaneous.Helpers
{
	public static class NetworkInformationHelper
	{
		private const int IanaInterfaceTypeWifi = 71;

		public static bool OnWifiConnection()
		{
			var internetConnectionProfile = Windows.Networking.Connectivity.NetworkInformation.GetInternetConnectionProfile();
			return internetConnectionProfile.NetworkAdapter.IanaInterfaceType == IanaInterfaceTypeWifi;
		}

		public static bool OnCellularDataConnection()
		{
			return !OnWifiConnection();
		}
	}
}
