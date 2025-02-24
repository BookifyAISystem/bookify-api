using bookify_data.Data;
using bookify_data.Entities;
using bookify_data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Slapper.AutoMapper;

namespace bookify_data.Repository
{
    public class VoucherRepository : IVoucherRepository
    {
        private readonly BookifyDbContext _context;
        public VoucherRepository(BookifyDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Voucher>> GetAllAsync()
        {
            return await _context.Vouchers.AsNoTracking().ToListAsync();
        }
        public async Task<Voucher?> GetByIdAsync(int id)
        {
            return await _context.Vouchers.FirstOrDefaultAsync(o => o.VoucherId == id);
        }
        public async Task<bool> InsertAsync(Voucher voucher)
        {
            voucher.CreatedDate = DateTime.UtcNow;
            voucher.LastEdited = DateTime.UtcNow;
            voucher.Status = 1;
            await _context.Vouchers.AddAsync(voucher);
            return await _context.SaveChangesAsync() > 0;

        }
        public async Task<bool> UpdateAsync(Voucher voucher)
        {
            _context.Vouchers.Update(voucher);
            return await _context.SaveChangesAsync() > 0;
        }
        public async Task<bool> DeleteAsync(int id)
        {
            var voucher = await _context.Vouchers.FindAsync(id);
            if(voucher == null)
                return false;

            _context.Vouchers.Remove(voucher);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
