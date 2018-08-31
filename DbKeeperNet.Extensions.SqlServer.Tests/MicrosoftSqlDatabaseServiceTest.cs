using System.Data.SqlClient;
using DbKeeperNet.Engine;
using DbKeeperNet.Engine.Configuration;
using DbKeeperNet.Engine.Tests;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace DbKeeperNet.Extensions.SqlServer.Tests
{
    [TestFixture]
    public class MicrosoftSqlDatabaseServiceTest : DatabaseServiceTests<SqlConnection>
    {
        protected override void Configure(IDbKeeperNetBuilder configurationBuilder)
        {
            configurationBuilder
                .UseSqlServer(ConnectionStrings.TestDatabase)
                ;

            configurationBuilder.Services.AddLogging();
        }
    }


    [TestFixture]
    public class MicrosoftSqlDatabaseExtensionValidatorBase : DatabaseExtensionValidatorBase
    {
        protected override void Configure(IDbKeeperNetBuilder configurationBuilder)
        {
            configurationBuilder
                .UseSqlServer(ConnectionStrings.TestDatabase)
                ;

            configurationBuilder.Services.AddLogging();
        }

    }
}