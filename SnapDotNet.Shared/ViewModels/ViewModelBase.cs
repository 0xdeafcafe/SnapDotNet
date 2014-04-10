using SnapDotNet.Apps.Common;
using SnapDotNet.Core.Snapchat.Api;
using SnapDotNet.Core.Snapchat.Models;

namespace SnapDotNet.Apps.ViewModels
{
	public class ViewModelBase
		: NotifyPropertyChangedBase
	{
		public SnapChatManager Manager
		{
			get { return App.SnapChatManager; }
		}

		public Account Account
		{
			get { return App.SnapChatManager.Account; }
		}
	}
}
