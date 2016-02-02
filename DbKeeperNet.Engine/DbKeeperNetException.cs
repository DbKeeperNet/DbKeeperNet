using System;
using System.Runtime.Serialization;

namespace DbKeeperNet.Engine
{
    /// <summary>
    /// All exceptions thrown within library during update process should be
    /// exposed only through this class.
    /// Optional details may be available as an inner exception.
    /// </summary>
    [Serializable]
    public class DbKeeperNetException: Exception
    {
        public DbKeeperNetException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public DbKeeperNetException(string message)
            : base(message)
        {
        }

        public DbKeeperNetException()
        {
        }

#if (!_PCL)
        protected DbKeeperNetException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
#endif
    }
}
