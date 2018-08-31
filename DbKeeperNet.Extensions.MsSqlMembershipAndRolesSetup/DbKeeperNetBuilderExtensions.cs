using System.Data.SqlClient;
using System.Reflection;
using System.Xml;
using DbKeeperNet.Engine;
using DbKeeperNet.Engine.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DbKeeperNet.Extensions.MsSqlMembershipAndRolesSetup
{
    public static class DbKeeperNetBuilderExtensions
    {
        public static IDbKeeperNetBuilder UseSqlServerMembershipAndRoleSetupScript(
            this IDbKeeperNetBuilder configuration)
        {
            configuration
                .AddEmbeddedResourceScript("DbKeeperNet.Extensions.MsSqlMembershipAndRolesSetup.Setup.xml, DbKeeperNet.Extensions.MsSqlMembershipAndRolesSetup")
                ;

            return configuration;
        }

        public static IDbKeeperNetBuilder UseSqlServerMembershipAndRoleSetup(this IDbKeeperNetBuilder configuration)
        {
            configuration
                .AddSchema(@"http://code.google.com/p/dbkeepernet/MsSqlMembershipAndRolesSetup-1.0.xsd",
                    XmlReader.Create(typeof(DbKeeperNetBuilderExtensions).Assembly.GetManifestResourceStream(
                        @"DbKeeperNet.Extensions.MsSqlMembershipAndRolesSetup.Resources.MsSqlMembershipAndRolesSetup-1.0.xsd")),
                    typeof(MsSqlMembershipAndRolesSetupType))

                ;

            configuration.Services
                .AddTransient<IUpdateStepHandler, MsSqlMembershipAndRolesSetupTypeHandlerService>()
                ;

            return configuration;
        }
    }
}