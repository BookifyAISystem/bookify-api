using bookify_data.Data;
using bookify_data.Entities;
using bookify_data.Interfaces;
using Microsoft.EntityFrameworkCore;

public class AccountRepository : IAccountRepository
{
	private readonly BookifyDbContext _context;

	public AccountRepository(BookifyDbContext context)
	{
		_context = context;
	}

	public async Task<Account?> GetAccountByIdAsync(int accountId)
	{
		return await _context.Accounts
			.FirstOrDefaultAsync(a => a.AccountId == accountId);
	}

	public async Task UpdateAccountAsync(Account account)
	{
		// EF Core theo dõi entity, chỉ cần đánh dấu "Modified" hoặc gọi Update
		_context.Accounts.Update(account);
		await _context.SaveChangesAsync();
	}
}
