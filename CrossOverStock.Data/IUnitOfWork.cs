using System;
using System.Data;
using System.Data.Entity;

namespace CrossOverStock.Data
{
    public interface IUnitOfWork
    {
        IDisposable BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);

        void RollbackTransaction();

        void CommitTransaction();

        DbContext GetContext();
    }
}
