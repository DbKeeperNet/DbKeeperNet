using DbKeeperNet.Engine.Configuration;
using DbKeeperNet.Engine.Tests;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace DbKeeperNet.Extensions.Mysql.Tests
{
    [TestFixture]
    public class MysqlEndToEndTest : EndToEndTestBase
    {
        protected override void Configure(IDbKeeperNetBuilder configurationBuilder)
        {
            configurationBuilder
                .UseMysql(ConnectionStrings.TestDatabase)
                .AddEmbeddedResourceScript(
                    "DbKeeperNet.Extensions.Mysql.Tests.MysqlEndToEndTest.xml,DbKeeperNet.Extensions.Mysql.Tests")
                ;

            configurationBuilder.Services.AddLogging();
        }

    }
}