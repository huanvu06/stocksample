using System;
using System.Data;
using System.Data.Common;
using System.Data.Entity;

namespace CrossOverStock.Data.Impl
{
    public class UnitOfWork<TCtx> : IUnitOfWork where TCtx : DbContext
    {
        private readonly TCtx _context;
        private DbTransaction _transaction;
        public UnitOfWork(TCtx context)
        {
            _context = context;
        }

        public DbContext GetContext()
        {
            return _context;
        }

        public virtual IDisposable BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {

            if (_context.Database.Connection.State != ConnectionState.Open)
            {
                _context.Database.Connection.Open();
            }
            _transaction = _context.Database.Connection.BeginTransaction(isolationLevel);
            _context.Database.UseTransaction(_transaction);
            return _transaction;
        }

        /// <summary>
        /// Releases the current transaction
        /// </summary>
        private void ReleaseTransaction(DbTransaction transaction)
        {
            transaction?.Dispose();
        }

        public virtual void RollbackTransaction()
        {
            if (_transaction == null)
            {
                throw new ApplicationException("Cannot roll back a transaction while there is no transaction running.");
            }

            _transaction.Rollback();
            ReleaseTransaction(_transaction);
        }
        public virtual void CommitTransaction()
        {
            if (_transaction == null)
            {
                throw new ApplicationException("Cannot roll back a transaction while there is no transaction running.");
            }

            try
            {
                _context.SaveChanges();
                _transaction.Commit();
                ReleaseTransaction(_transaction);
            }
            catch (Exception ex)
            {

                RollbackTransaction();
                throw;
            }
        }
    }
}
