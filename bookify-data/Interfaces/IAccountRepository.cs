using bookify_data.Entities;
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
	}
}
