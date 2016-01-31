using System.IO;
using DbKeeperNet.Engine;

namespace DbKeeperNet.Extensions.AspNetRolesAndMembership
{
    public class AspNetRolesAndMembershipExtension : IExtension
    {
        public void Initialize(IUpdateContext context)
        {
            using (var schemaReader = new StreamReader(typeof(AspNetMembershipAdapter).GetManifestResourceFromTypeAssembly(@"DbKeeperNet.Extensions.AspNetRolesAndMembership.Resources.UpdatesAspNetRolesAndMembership-1.0.xsd")))
            {
                context.RegisterSchema("http://code.google.com/p/dbkeepernet/UpdatesAspNetRolesAndMembership-1.0.xsd", schemaReader.ReadToEnd());
            }

            var adapter = new AspNetMembershipAdapter();
            context.RegisterUpdateStepHandler(new AspNetMembershipAccountCreateUpdateStepHandlerService(adapter));
            context.RegisterUpdateStepHandler(new AspNetMembershipAccountDeleteUpdateStepHandlerService(adapter));
            context.RegisterUpdateStepHandler(new AspNetMembershipRoleCreateUpdateStepHandlerService(adapter));
            context.RegisterUpdateStepHandler(new AspNetMembershipRoleDeleteUpdateStepHandlerService(adapter));
        }
    }
}