using DbKeeperNet.Engine.Configuration;
using DbKeeperNet.Engine.Tests;
using DbKeeperNet.Engine.Tests.Checkers;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace DbKeeperNet.Extensions.SqlServer.Tests
{
    [TestFixture]
    public class SqlServerEndToEndTest : EndToEndTestBase
    {
        protected override void Configure(IDbKeeperNetBuilder configurationBuilder)
        {
            configurationBuilder
                .UseSqlServer(ConnectionStrings.TestDatabase)
                .AddEmbeddedResourceScript(
                    "DbKeeperNet.Extensions.SqlServer.Tests.SqlServerEndToEndTest.xml,DbKeeperNet.Extensions.SqlServer.Tests")
                ;

            configurationBuilder.Services.AddLogging();
        }

    }
}