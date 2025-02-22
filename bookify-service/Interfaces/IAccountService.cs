using bookify_data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bookify_service.Interfaces
{
	public interface IAccountService
	{
		Task<Account?> GetAccountByIdAsync(int accountId);
		Task UpdateAccountAsync(Account account);
		Task<Account?> GetAccountWithReferencesAsync(int accountId);
		Task<bool> DeleteAccountAsync(int accountId);
	}
}
