using DbKeeperNet.Engine;
using DbKeeperNet.Extensions.AspNetRolesAndMembership.Resources;

namespace DbKeeperNet.Extensions.AspNetRolesAndMembership
{
    public class AspNetMembershipRoleCreateUpdateStepHandlerService : UpdateStepHandlerService<AspNetRoleCreateUpdateStepType>
    {
        private readonly IAspNetMembershipAdapter _aspNetMembershipAdapter;

        public AspNetMembershipRoleCreateUpdateStepHandlerService(IAspNetMembershipAdapter aspNetMembershipAdapter)
        {
            _aspNetMembershipAdapter = aspNetMembershipAdapter;
        }

        protected override void Handle(AspNetRoleCreateUpdateStepType castedStep, IUpdateContext context)
        {
            context.Logger.TraceInformation(AspNetMembershipAdapterMessages.GoingToUseAdapter, _aspNetMembershipAdapter);
            context.Logger.TraceInformation(AspNetMembershipAdapterMessages.AddingRole, castedStep.RoleName);
            _aspNetMembershipAdapter.CreateRole(castedStep.RoleName);
            context.Logger.TraceInformation(AspNetMembershipAdapterMessages.AddedRole, castedStep.RoleName);
        }
    }
}