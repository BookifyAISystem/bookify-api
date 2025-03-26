using bookify_data.Data;
using bookify_data.Entities;
using bookify_data.Interfaces;
using bookify_data.Model;
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
    public async Task<Account?> GetAccountByIdWithReferencesAsync(int accountId)
    {
        var account = await _context.Accounts
            .Include(a => a.NewsList)
          
            .FirstOrDefaultAsync(a => a.AccountId == accountId);

        if (account != null)
        {
            account.Notes = await _context.Notes
                .Where(n => n.AccountId == accountId)
                .ToListAsync();
        }

        return account;
    }

    // Soft Delete: chỉ set Status = 0
    public async Task DeleteAccountAsync(Account account)
	{
		account.Status = 0;         // 0 = đã bị xóa (theo yêu cầu)
		account.LastEdited = DateTime.Now;

		_context.Accounts.Update(account);
		await _context.SaveChangesAsync();
	}
	public async Task<(IEnumerable<Account> Items, int TotalCount)> GetPagedAccountsAsync(AccountQueryParameters parameters)
	{
		// Bắt đầu query
		IQueryable<Account> query = _context.Accounts.AsQueryable();

		// 1) Tìm kiếm (Username, DisplayName, Email)
		if (!string.IsNullOrEmpty(parameters.Search))
		{
			string search = parameters.Search.Trim();
			query = query.Where(a =>
				a.Username.Contains(search) ||
				a.DisplayName.Contains(search) ||
				a.Email.Contains(search));
		}

		// 2) Lọc theo Status
		if (parameters.Status.HasValue)
		{
			query = query.Where(a => a.Status == parameters.Status.Value);
		}

		// 3) Lọc theo RoleId
		if (parameters.RoleId.HasValue)
		{
			query = query.Where(a => a.RoleId == parameters.RoleId.Value);
		}

		// 4) Đếm tổng số bản ghi (sau khi lọc)
		int totalCount = await query.CountAsync();

		// 5) Áp dụng phân trang
		int skip = (parameters.Page - 1) * parameters.PageSize;
		var items = await query
			.OrderByDescending(a => a.CreatedDate) // Sắp xếp theo CreatedDate giảm dần
			.Skip(skip)
			.Take(parameters.PageSize)
			.ToListAsync();

		// Trả về (Items, TotalCount)
		return (items, totalCount);
	}
}
