using bookify_data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bookify_data.Interfaces
{
    public interface IVoucherRepository
    {
        Task<IEnumerable<Voucher>> GetAllAsync();
        Task<Voucher?> GetByIdAsync(int id);
        Task<bool> InsertAsync(Voucher voucher);
        Task<bool> UpdateAsync(Voucher voucher);
        Task<bool> DeleteAsync(int id);
    }
}
