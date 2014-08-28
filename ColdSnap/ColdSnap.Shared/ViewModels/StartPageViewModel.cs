using SnapDotNet;
using System.Threading.Tasks;
using SnapDotNet.Utilities;

namespace ColdSnap.ViewModels
{
	public sealed class StartPageViewModel
		: ObservableObject
    {
		public async Task<Account> LogInAsync(string username, string password)
		{
			var account = await Account.AuthenticateAsync(username, password);
			await account.UpdateAccountAsync();
			await StateManager.Local.SaveAccountStateAsync(account);
			return account;
		}
    }
}
