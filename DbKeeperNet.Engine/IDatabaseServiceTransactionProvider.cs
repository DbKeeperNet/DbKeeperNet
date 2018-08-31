using System.Data.Common;

namespace DbKeeperNet.Engine
{
    public interface IDatabaseServiceTransactionProvider
    {
        void BeginTransaction();

        void CommitTransaction();

        void RollbackTransaction();

        DbTransaction GetTransaction();
    }

    public interface IDatabaseServiceTransactionProvider<out T> where T : DbTransaction
    {
        T GetTransaction();
    }
}