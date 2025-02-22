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
		public async Task<Account?> GetAccountWithReferencesAsync(int accountId)
		{
			return await _accountRepository.GetAccountByIdWithReferencesAsync(accountId);
		}

		// Trả về true/false xem xóa thành công không
		public async Task<bool> DeleteAccountAsync(int accountId)
		{
			// 1) Lấy Account + các liên kết
			var account = await _accountRepository.GetAccountByIdWithReferencesAsync(accountId);
			if (account == null)
			{
				return false; // Account không tồn tại
			}

			// 2) Kiểm tra liên kết
			//    Ví dụ: Nếu account còn Customer, News, Note... có thể cấm xóa hoặc cho xóa tùy logic
			bool hasReferences = (account.Customers.Any()
								  || account.NewsList.Any()
								  || account.Notes.Any());
			// ... Nếu bạn có Staff:  account.Staff.Any()  (hoặc Staff == null, tùy mô hình)

			// 3) Tùy yêu cầu nghiệp vụ:
			//    - Nếu có ràng buộc, bạn có thể không cho xóa, hoặc vẫn xóa (status=0) nhưng log warning.
			if (hasReferences)
			{
				// Ví dụ: Vẫn cho xóa, nhưng log
				Console.WriteLine("Account đang được tham chiếu bởi Customer/News/Note...");
				// Hoặc return false để cấm xóa.
			}

			// 4) Thực hiện Soft Delete
			await _accountRepository.DeleteAccountAsync(account);
			return true;
		}
	}

}
