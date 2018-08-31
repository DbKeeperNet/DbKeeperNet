using System;
using System.Data.Common;

namespace DbKeeperNet.Engine
{
    public interface IDatabaseService : IDisposable
    {
        bool CanHandle(string databaseType);

        DbConnection GetOpenConnection();

        DbConnection CreateOpenConnection();
    }

    public interface IDatabaseService<out T> where T: DbConnection
    {
        T GetOpenConnection();
    }
}