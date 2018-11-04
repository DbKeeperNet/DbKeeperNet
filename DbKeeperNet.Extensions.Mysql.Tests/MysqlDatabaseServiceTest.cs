using DbKeeperNet.Engine.Configuration;
using DbKeeperNet.Engine.Tests;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using NUnit.Framework;

namespace DbKeeperNet.Extensions.Mysql.Tests
{
    [TestFixture]
    public class MysqlDatabaseServiceTest : DatabaseServiceTests<MySqlConnection>
    {
        protected override void Configure(IDbKeeperNetBuilder configurationBuilder)
        {
            configurationBuilder
                .UseMysql(ConnectionStrings.TestDatabase)
                ;

            configurationBuilder.Services.AddLogging(c => { c.AddConsole(); });
        }
    }
}