using System;
using System.Runtime.Serialization;

namespace DbKeeperNet.Engine
{
    [Serializable]
    public class DbKeeperNetException : Exception
    {
        public DbKeeperNetException()
        {
        }

        protected DbKeeperNetException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public DbKeeperNetException(string message) : base(message)
        {
        }

        public DbKeeperNetException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}