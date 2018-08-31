using DbKeeperNet.Engine.Configuration;
using DbKeeperNet.Engine.Tests;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace DbKeeperNet.Extensions.SQLite.Tests
{
    [TestFixture]
    public class SQLiteDatabaseServiceTest : DatabaseServiceTests<SqliteConnection>
    {

        private const string ConnectionString = "Data source=sqlitetest.db3";
        protected override void Configure(IDbKeeperNetBuilder configurationBuilder)
        {
            configurationBuilder
                .UseSQLite(ConnectionString)
                ;

            configurationBuilder.Services.AddLogging();
        }
    }
}
