using DbKeeperNet.Engine;
using DbKeeperNet.Engine.UpdateStepHandlers;

namespace DbKeeperNet.Extensions.AspNetRolesAndMembership
{
    public class AspNetMembershipRoleDeleteUpdateStepHandlerService : UpdateStepHandlerBase<AspNetRoleDeleteUpdateStepType>
    {
        private readonly IAspNetMembershipAdapter _aspNetMembershipAdapter;

        public AspNetMembershipRoleDeleteUpdateStepHandlerService(IAspNetMembershipAdapter aspNetMembershipAdapter)
        {
            _aspNetMembershipAdapter = aspNetMembershipAdapter;
        }

        protected override void Execute(UpdateStepContextWithPreconditions<AspNetRoleDeleteUpdateStepType> context)
        {
            var castedStep = context.Step;

        // context.Logger.TraceInformation(AspNetMembershipAdapterMessages.GoingToUseAdapter, _aspNetMembershipAdapter);
        //    context.Logger.TraceInformation(AspNetMembershipAdapterMessages.DeletingRole, castedStep.RoleName);
            _aspNetMembershipAdapter.DeleteRole(castedStep.RoleName);
        //    context.Logger.TraceInformation(AspNetMembershipAdapterMessages.DeletedRole, castedStep.RoleName);
        }
    }
}