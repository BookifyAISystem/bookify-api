using bookify_data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bookify_data.Interfaces
{
    public interface IPaymentRepository
    {
        Task<IEnumerable<Payment>> GetAllAsync();
        Task<IEnumerable<Payment>> GetPaymentsByAccountIdAsync(int accountId);
        Task<IEnumerable<Payment>> GetPaymentsByStatus(int status);
        Task<Payment?> GetByIdAsync(int id);
        void Insert(Payment payment);
    }
}
