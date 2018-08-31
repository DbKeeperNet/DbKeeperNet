using DbKeeperNet.Engine.Configuration;
using DbKeeperNet.Engine.Tests;
using DbKeeperNet.Engine.Tests.Checkers;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace DbKeeperNet.Extensions.SQLite.Tests
{
    [TestFixture]
    public class SQLiteEndToEndTest : EndToEndTestBase
    {
        protected override void Configure(IDbKeeperNetBuilder configurationBuilder)
        {
            configurationBuilder
                .UseSQLite("Data Source=endtoend.db3")
                .AddEmbeddedResourceScript(
                    "DbKeeperNet.Extensions.SQLite.Tests.SQLiteEndToEndTest.xml,DbKeeperNet.Extensions.SQLite.Tests")
                ;

            configurationBuilder.Services.AddLogging();
        }
    }
}