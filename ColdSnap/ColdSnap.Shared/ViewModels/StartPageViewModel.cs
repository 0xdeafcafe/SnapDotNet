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
#if DEBUG
			Account account;
			if (username.StartsWith("auth:"))
				account = Account.AuthenticateFromAuth(username.Replace("auth:", ""), password);
			else
				account = await Account.AuthenticateAsync(username, password);
			//await account.UpdateAccountAsync();
			await StateManager.Local.SaveAccountStateAsync(account);
			return account;
#else
			var account = await Account.AuthenticateAsync(username, password);
			await account.UpdateAccountAsync();
			await StateManager.Local.SaveAccountStateAsync(account);
			return account;
#endif
		}
    }
}
