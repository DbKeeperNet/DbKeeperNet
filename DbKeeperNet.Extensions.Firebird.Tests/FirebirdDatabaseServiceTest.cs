using System.Data;
using DbKeeperNet.Engine.Configuration;
using DbKeeperNet.Engine.Tests;
using FirebirdSql.Data.FirebirdClient;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NUnit.Framework;

namespace DbKeeperNet.Extensions.Firebird.Tests
{
    [TestFixture]
    public class FirebirdDatabaseServiceTest : DatabaseServiceTests<FbConnection>
    {
        protected override void Configure(IDbKeeperNetBuilder configurationBuilder)
        {
            configurationBuilder
                .UseFirebird(ConnectionStrings.TestDatabase)
                ;

            configurationBuilder.Services.AddLogging(c => { c.AddConsole(); });
        }
    }
}