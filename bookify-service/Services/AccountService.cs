using bookify_data.Entities;
using bookify_data.Interfaces;
using bookify_service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bookify_service.Services
{
	public class AccountService : IAccountService
	{
		private readonly IAccountRepository _accountRepository;

		public AccountService(IAccountRepository accountRepository)
		{
			_accountRepository = accountRepository;
		}

		public async Task<Account?> GetAccountByIdAsync(int accountId)
		{
			return await _accountRepository.GetAccountByIdAsync(accountId);
		}

		public async Task UpdateAccountAsync(Account account)
		{
			// Thêm logic băm mật khẩu, validate email/phone, ... nếu cần
			account.LastEdited = DateTime.Now;
			await _accountRepository.UpdateAccountAsync(account);
		}
	}

}
