using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bookify_data.Interfaces
{
	public interface IUnitOfWork : IDisposable
	{
        IPaymentRepository Payments { get; }
        IOrderRepository Orders { get; }
        IOrderDetailRepository OrderDetails { get; }
        IBookRepository Books { get; }
        IBookCategoryRepository BookCategories { get; }
        ICategoryRepository Categories { get; }
        void CommitTransaction();
		void RollbackTransaction();
		int SaveChanges();
		void BeginTransaction();
        Task<bool> CompleteAsync();

    }
}
