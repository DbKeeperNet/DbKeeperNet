namespace DbKeeperNet.Engine
{
    /// <summary>
    /// Database lock implementation
    /// </summary>
    public interface IDatabaseLock
    {
        /// <summary>
        /// Gets whether the database locking is already supported
        /// by database schema
        /// </summary>
        bool IsSupported { get; }

        /// <summary>
        /// Attempt to acquire the <paramref name="lockId"/>
        /// </summary>
        /// <param name="lockId">Lock id</param>
        /// <param name="ownerDescription">Owner description</param>
        /// <param name="expirationMinutes">Expiration in minutes</param>
        /// <returns><c>true</c> when lock was acquired, <c>false</c> if lock was already acquired by another process</returns>
        bool Acquire(int lockId, string ownerDescription, int expirationMinutes);

        /// <summary>
        /// Release the <paramref name="lockId"/>
        /// </summary>
        /// <param name="lockId">Lock id</param>
        void Release(int lockId);
    }
}