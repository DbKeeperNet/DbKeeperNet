using DbKeeperNet.Engine.Configuration;
using DbKeeperNet.Engine.Tests;
using DbKeeperNet.Engine.Tests.Checkers;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace DbKeeperNet.Extensions.Pgsql.Tests
{
    [TestFixture]
    public class PgsqlEndToEndTest : EndToEndTestBase
    {
        protected override void Configure(IDbKeeperNetBuilder configurationBuilder)
        {
            configurationBuilder
                .UsePgsql(ConnectionStrings.TestDatabase)
                .AddEmbeddedResourceScript(
                    "DbKeeperNet.Extensions.Pgsql.Tests.PgsqlEndToEndTest.xml,DbKeeperNet.Extensions.Pgsql.Tests")
                ;

            configurationBuilder.Services.AddLogging();
        }

    }
}