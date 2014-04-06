namespace SnapDotNet.Core.Snapchat.Api
{
	public class Endpoints
	{
		private SnapchatManager _snapchatManager;
		private WebConnect _webConnect;

		public Endpoints(SnapchatManager snapchatManager)
		{
			_snapchatManager = snapchatManager;
			_webConnect = new WebConnect();
		}


	}
}
