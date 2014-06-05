using Snapchat.Common;
using SnapDotNet.Core.Snapchat.Models;

namespace Snapchat.ViewModels
{
	public class BaseViewModel
		: ObservableObject
	{
		public Account Account
		{
			get { return App.SnapchatManager.Account; }
		}
	}
}
