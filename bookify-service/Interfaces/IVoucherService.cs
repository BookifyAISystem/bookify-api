using bookify_data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bookify_service.Interfaces
{
    public interface IVoucherService
    {
        Task<IEnumerable<GetVoucherDTO>> GetAllAsync();
        Task<GetVoucherDTO?> GetByIdAsync(int id);
        Task<bool> CreateVoucherAsync(AddVoucherDTO addVoucherDto);
        Task<bool> UpdateVoucherAsync(int id, UpdateVoucherDTO updateVoucherDto);
        Task<bool> DeleteVoucherAsync(int id);
    }
}
