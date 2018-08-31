using DbKeeperNet.Engine;
using DbKeeperNet.Engine.UpdateStepHandlers;

namespace DbKeeperNet.Extensions.AspNetRolesAndMembership
{
    public class AspNetMembershipRoleCreateUpdateStepHandlerService : UpdateStepHandlerBase<AspNetRoleCreateUpdateStepType>
    {
        private readonly IAspNetMembershipAdapter _aspNetMembershipAdapter;

        public AspNetMembershipRoleCreateUpdateStepHandlerService(IAspNetMembershipAdapter aspNetMembershipAdapter)
        {
            _aspNetMembershipAdapter = aspNetMembershipAdapter;
        }

        protected override void Execute(UpdateStepContextWithPreconditions<AspNetRoleCreateUpdateStepType> context)
        {
            var castedStep = context.Step;

            // context.Logger.TraceInformation(AspNetMembershipAdapterMessages.GoingToUseAdapter, _aspNetMembershipAdapter);
            // context.Logger.TraceInformation(AspNetMembershipAdapterMessages.AddingRole, castedStep.RoleName);
            _aspNetMembershipAdapter.CreateRole(castedStep.RoleName);
            // context.Logger.TraceInformation(AspNetMembershipAdapterMessages.AddedRole, castedStep.RoleName);
        }
    }
}