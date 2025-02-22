using bookify_data.Entities;
using bookify_data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bookify_data.Interfaces
{
	public interface IAccountRepository
	{
		Task<Account?> GetAccountByIdAsync(int accountId);
		Task UpdateAccountAsync(Account account); 
		Task<Account?> GetAccountByIdWithReferencesAsync(int accountId);
		Task DeleteAccountAsync(Account account);
		Task<(IEnumerable<Account> Items, int TotalCount)> GetPagedAccountsAsync(AccountQueryParameters parameters);

	}
}
