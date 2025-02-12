using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bookify_data.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        void CommitTransaction();
        void RollbackTransaction();
        int SaveChanges();
        void BeginTransaction();
    }
}