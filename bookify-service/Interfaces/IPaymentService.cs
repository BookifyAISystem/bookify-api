using bookify_data.Entities;
using bookify_data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bookify_service.Interfaces
{
    public interface IPaymentService
    {
        Task<IEnumerable<GetPaymentDTO>> GetAllPaymentAsync();
        Task<IEnumerable<GetPaymentDTO>> GetPaymentsByStatusAsync(int status);
        Task<IEnumerable<GetPaymentDTO>> GetPaymentsByAccountIdAsync(int accountId);
        Task<GetPaymentDTO?> GetPaymentByIdAsync(int id);
        Task<bool> CreateVnpayPaymentAsync(int orderId);
        Task<bool> CreateCODPaymentAsync(int orderId);
    }
}
