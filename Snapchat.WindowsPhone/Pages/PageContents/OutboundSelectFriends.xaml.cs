using System.Linq;
using Snapchat.ViewModels.PageContents;
using SnapDotNet.Core.Snapchat.Models.AppSpecific;
using SnapDotNet.Core.Snapchat.Models.New;

namespace Snapchat.Pages.PageContents
{
	public sealed partial class OutboundSelectFriends
	{
		public OutboundSelectFriendsViewModel ViewModel { get; private set; }

		public OutboundSelectFriends()
		{
			InitializeComponent();
		}

		public void Reset()
		{
			DataContext = ViewModel = null;
		}

		public void Load()
		{
			DataContext = ViewModel = new OutboundSelectFriendsViewModel();
		}

		public void SelectAllFriends()
		{
			foreach (var reciepicent in ViewModel.SelectedFriends
				.Where(reciepicent => reciepicent.Key is Friend ||
				                      reciepicent.Key is SelectedRecent))
			{
				// le code
				ViewModel.SelectedFriends[reciepicent.Key] = true;
			}
		}

		public void DeSelectAllFriends()
		{
			foreach (var reciepicent in ViewModel.SelectedFriends
				.Where(reciepicent => reciepicent.Key is Friend ||
									  reciepicent.Key is SelectedRecent))
			{
				// le code
				ViewModel.SelectedFriends[reciepicent.Key] = false;
			}
		}
	}
}
