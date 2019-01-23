using System.Xml;
using DbKeeperNet.Engine;
using DbKeeperNet.Engine.Configuration;
using DbKeeperNet.Extensions.AspNetRolesAndMembership.Preconditions;
using Microsoft.Extensions.DependencyInjection;

namespace DbKeeperNet.Extensions.AspNetRolesAndMembership
{
    public static class DbKeeperNetBuilderExtensions
    {
        public static IDbKeeperNetBuilder UseAspNetRolesAndMembership(this IDbKeeperNetBuilder configuration)
        {
            configuration.Services
                .AddTransient<IAspNetMembershipAdapter, AspNetMembershipAdapter>()
                .AddTransient<IPreconditionHandler, RoleNotFound>()
                .AddTransient<IPreconditionHandler, UserNotFound>()

                .AddTransient<IUpdateStepHandler, AspNetMembershipAccountCreateUpdateStepHandlerService>()
                .AddTransient<IUpdateStepHandler, AspNetMembershipAccountDeleteUpdateStepHandlerService>()
                .AddTransient<IUpdateStepHandler, AspNetMembershipRoleCreateUpdateStepHandlerService>()
                .AddTransient<IUpdateStepHandler, AspNetMembershipRoleDeleteUpdateStepHandlerService>()

                ;

            configuration.AddSchema("http://code.google.com/p/dbkeepernet/UpdatesAspNetRolesAndMembership-1.0.xsd",
                () => XmlReader.Create(typeof(AspNetMembershipAdapter).Assembly.GetManifestResourceStream(@"DbKeeperNet.Extensions.AspNetRolesAndMembership.Resources.UpdatesAspNetRolesAndMembership-1.0.xsd")),
                typeof(AspNetAccountCreateUpdateStepType), 
                typeof(AspNetAccountDeleteUpdateStepType), 
                typeof(AspNetRoleCreateUpdateStepType), 
                typeof(AspNetRoleDeleteUpdateStepType));

            return configuration;
        }
    }
}