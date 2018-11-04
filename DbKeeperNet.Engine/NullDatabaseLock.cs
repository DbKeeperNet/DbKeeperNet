namespace DbKeeperNet.Engine
{
    /// <summary>
    /// Null implementation of <see cref="IDatabaseLock"/>
    /// </summary>
    public class NullDatabaseLock : IDatabaseLock
    {
        /// <inheritdoc />
        public bool IsSupported
        {
            get { return false; }
        }

        /// <inheritdoc />
        public bool Acquire(int lockId, string ownerDescription, int expirationMinutes)
        {
            return true;
        }

        /// <inheritdoc />
        public void Release(int lockId)
        {
        }
    }
}