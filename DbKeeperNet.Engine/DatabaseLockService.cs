using System;
using System.Threading;
using Microsoft.Extensions.Logging;

namespace DbKeeperNet.Engine
{
    public class DatabaseLockService : IDatabaseLockService
    {
        private readonly ILogger<DatabaseLockService> _logger;
        private readonly IDatabaseLock _databaseLock;

        public DatabaseLockService(ILogger<DatabaseLockService> logger, IDatabaseLock databaseLock)
        {
            _logger = logger;
            _databaseLock = databaseLock;
        }

        public IDisposable AcquireLock(int lockId)
        {
            if (!_databaseLock.IsSupported)
            {
                _logger.LogInformation("Database lock is not supported, assuming that lock {0} is acquired", lockId);
                return new DisposeAction(() => { });
            }

            const int expirationMinutes = 5;

            while (!_databaseLock.Acquire(lockId, Guid.NewGuid().ToString(), expirationMinutes))
            {
                _logger.LogInformation("Database lock {0} could not be acquired - going to try again after 5 seconds", lockId);
                Thread.Sleep(5000);
            }
            
            _logger.LogInformation("Database lock {0} was acquired", lockId);

            return new DisposeAction(
                () =>
                {
                    _databaseLock.Release(lockId);

                    _logger.LogInformation("Database lock {0} was released", lockId);
                }
            );
        }

        private class DisposeAction : IDisposable
        {
            private readonly Action _action;

            public DisposeAction(Action action)
            {
                _action = action;
            }

            public void Dispose()
            {
                _action();
            }
        }
    }
}