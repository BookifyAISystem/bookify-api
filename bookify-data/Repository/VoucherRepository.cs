﻿using bookify_data.Data;
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
            
            await _context.Vouchers.AddAsync(voucher);
            return await _context.SaveChangesAsync() > 0;

        }
        public async Task<bool> UpdateAsync(Voucher voucher)
        {
            if (voucher.Quantity == 0) 
            {
                voucher.Status = 0;
            }
            _context.Vouchers.Update(voucher);
            return await _context.SaveChangesAsync() > 0;
        }
        
    }
}
