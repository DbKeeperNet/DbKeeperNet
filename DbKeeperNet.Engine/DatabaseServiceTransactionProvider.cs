using System;
using System.Data.Common;
using Microsoft.Extensions.Logging;

namespace DbKeeperNet.Engine
{
    public class DatabaseServiceTransactionProvider<T> : 
        IDatabaseServiceTransactionProvider, 
        IDatabaseServiceTransactionProvider<T>, 
        IDisposable where T : DbTransaction
    {
        private readonly IDatabaseService _databaseService;
        private readonly ILogger<DatabaseServiceTransactionProvider<T>> _logger;
        private DbTransaction _activeTransaction;

        public DatabaseServiceTransactionProvider(IDatabaseService databaseService, ILogger<DatabaseServiceTransactionProvider<T>> logger)
        {
            _databaseService = databaseService;
            _logger = logger;
        }

        T IDatabaseServiceTransactionProvider<T>.GetTransaction()
        {
            return (T)GetTransaction();
        }

        public DbTransaction GetTransaction()
        {
            EnsureOpenTransaction();

            return _activeTransaction;
        }

        public void BeginTransaction()
        {
            if (_activeTransaction != null) throw new InvalidOperationException("A transaction is already in progress");

            _logger.LogInformation("Going to begin a new transaction");
            _activeTransaction = _databaseService.GetOpenConnection().BeginTransaction();
            _logger.LogInformation("A new transaction started");
        }

        public void CommitTransaction()
        {
            EnsureOpenTransaction();

            _logger.LogInformation("Going to commit a transaction");
            var transaction = _activeTransaction;
            _activeTransaction = null;
            transaction.Commit();
            _logger.LogInformation("Transaction commited");
        }

        public void RollbackTransaction()
        {
            EnsureOpenTransaction();

            _logger.LogInformation("Going to rollback the active transaction");

            var transaction = _activeTransaction;
            _activeTransaction = null;
            transaction.Rollback();

            _logger.LogInformation("Transaction rolled back");
        }

        private void EnsureOpenTransaction()
        {
            if (_activeTransaction == null)
            {
                throw new InvalidOperationException("There is no active transaction");
            }
        }

        public void Dispose()
        {
            if (_activeTransaction != null)
            {
                RollbackTransaction();
            }
        }
    }
}