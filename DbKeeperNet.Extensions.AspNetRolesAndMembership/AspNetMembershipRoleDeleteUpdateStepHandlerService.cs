using DbKeeperNet.Engine;
using DbKeeperNet.Extensions.AspNetRolesAndMembership.Resources;

namespace DbKeeperNet.Extensions.AspNetRolesAndMembership
{
    public class AspNetMembershipRoleDeleteUpdateStepHandlerService : UpdateStepHandlerService<AspNetRoleDeleteUpdateStepType>
    {
        private readonly IAspNetMembershipAdapter _aspNetMembershipAdapter;

        public AspNetMembershipRoleDeleteUpdateStepHandlerService(IAspNetMembershipAdapter aspNetMembershipAdapter)
        {
            _aspNetMembershipAdapter = aspNetMembershipAdapter;
        }

        protected override void Handle(AspNetRoleDeleteUpdateStepType castedStep, IUpdateContext context)
        {
            context.Logger.TraceInformation(AspNetMembershipAdapterMessages.GoingToUseAdapter, _aspNetMembershipAdapter);
            context.Logger.TraceInformation(AspNetMembershipAdapterMessages.DeletingRole, castedStep.RoleName);
            _aspNetMembershipAdapter.DeleteRole(castedStep.RoleName);
            context.Logger.TraceInformation(AspNetMembershipAdapterMessages.DeletedRole, castedStep.RoleName);
        }
    }
}