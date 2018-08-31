using DbKeeperNet.Engine;
using DbKeeperNet.Engine.Configuration;
using DbKeeperNet.Engine.Tests;
using DbKeeperNet.Extensions.MsSqlMembershipAndRolesSetup;
using DbKeeperNet.Extensions.SqlServer;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace DbKeeperNet.Extensions.AspNetRolesAndMembership.Tests
{
    [TestFixture]
    public class AspNetMemberShipTest : TestBase
    {
        protected override void Configure(IDbKeeperNetBuilder configurationBuilder)
        {
            configurationBuilder
                .UseSqlServer(ConnectionStrings.TestDatabase)
                .UseAspNetRolesAndMembership()
                .UseSqlServerMembershipAndRoleSetup()
                .UseSqlServerMembershipAndRoleSetupScript()
                .AddEmbeddedResourceScript("DbKeeperNet.Extensions.AspNetRolesAndMembership.Tests.AspNetMemberShipTest.xml, DbKeeperNet.Extensions.AspNetRolesAndMembership.Tests")
                ;


            configurationBuilder.Services.AddLogging();
        }

        [Test]
        public void SqlServerIntegrationTest()
        {
            var updater = GetService<IDatabaseUpdater>();
            updater.ExecuteUpgrade();
        }
    }
}
