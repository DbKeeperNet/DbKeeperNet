using DbKeeperNet.Engine;
using DbKeeperNet.Engine.Configuration;
using DbKeeperNet.Engine.Tests;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace DbKeeperNet.Extensions.Firebird.Tests
{
    [TestFixture]
    public class FirebirdEndToEndTest : EndToEndTestBase
    {
        protected override void Configure(IDbKeeperNetBuilder configurationBuilder)
        {
            configurationBuilder
                .UseFirebird(ConnectionStrings.TestDatabase)
                .AddEmbeddedResourceScript(
                    "DbKeeperNet.Extensions.Firebird.Tests.FirebirdEndToEndTest.xml,DbKeeperNet.Extensions.Firebird.Tests")
                ;

            configurationBuilder.Services.AddLogging();
        }

        protected override void AssertThatTableExists(IDatabaseService connection)
        {
           ExecuteCommand(connection, @"select * from ""DbKeeperNet_SimpleDemo""");
        }
    }
}