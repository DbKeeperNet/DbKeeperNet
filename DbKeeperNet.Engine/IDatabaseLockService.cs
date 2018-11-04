using System;

namespace DbKeeperNet.Engine
{
    /// <summary>
    /// Database locking service allowing to run action
    /// mutually exclusive
    /// </summary>
    public interface IDatabaseLockService
    {
        /// <summary>
        /// Acquire lock identified by <paramref name="lockId"/>
        /// </summary>
        /// <param name="lockId">Lock id</param>
        /// <returns>Disposable object which automatically releases lock</returns>
        IDisposable AcquireLock(int lockId);
    }
}