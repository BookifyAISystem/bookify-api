using bookify_data.Data;
using bookify_data.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bookify_data.Helper
{
	public class UnitOfWork : IUnitOfWork, IDisposable
	{
		private readonly BookifyDbContext _dbContext;
		private IDbContextTransaction? _transaction = null;
        public IOrderRepository Orders { get; }
        public IPaymentRepository Payments { get; }
        public IOrderDetailRepository OrderDetails { get; }

        public UnitOfWork(BookifyDbContext dbContext, IOrderRepository orders, IOrderDetailRepository orderDetails, IPaymentRepository payments)
        {
            _dbContext = dbContext;
            Orders = orders;
            OrderDetails = orderDetails;
            Payments = payments;
        }

        public int SaveChanges()
		{
			return _dbContext.SaveChanges();
		}

        public async Task<bool> CompleteAsync()
        {
            return await _dbContext.SaveChangesAsync() > 0;
        }
        public void Dispose()
		{
			_dbContext.Dispose();
			GC.SuppressFinalize(this);
		}

		public void BeginTransaction()
		{
			_transaction = _dbContext.Database.BeginTransaction();
		}

		public void CommitTransaction()
		{
			if (_transaction != null)
			{
				_transaction.Commit();
				_transaction.Dispose();
			}
		}

		public void RollbackTransaction()
		{
			if (_transaction != null)
			{
				_transaction.Rollback();
				_transaction.Dispose();
			}
		}
	}
}
