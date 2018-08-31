using DbKeeperNet.Engine;
using DbKeeperNet.Engine.UpdateStepHandlers;

namespace DbKeeperNet.Extensions.AspNetRolesAndMembership
{
    public class AspNetMembershipAccountDeleteUpdateStepHandlerService : UpdateStepHandlerBase<AspNetAccountDeleteUpdateStepType>
    {
        private readonly IAspNetMembershipAdapter _aspNetMembershipAdapter;

        public AspNetMembershipAccountDeleteUpdateStepHandlerService(IAspNetMembershipAdapter aspNetMembershipAdapter)
        {
            _aspNetMembershipAdapter = aspNetMembershipAdapter;
        }

        protected override void Execute(UpdateStepContextWithPreconditions<AspNetAccountDeleteUpdateStepType> context)
        {
            var castedStep = context.Step;

            //context.Logger.TraceInformation(AspNetMembershipAdapterMessages.GoingToUseAdapter, _aspNetMembershipAdapter);
            //context.Logger.TraceInformation(AspNetMembershipAdapterMessages.DeletingUser, castedStep.UserName);

            if (_aspNetMembershipAdapter.DeleteUser(castedStep.UserName))
            {
            //    context.Logger.TraceInformation(AspNetMembershipAdapterMessages.DeletedUser, castedStep.UserName);
            }
            else
            {
            //    context.Logger.TraceWarning(AspNetMembershipAdapterMessages.UserNotDeleted, castedStep.UserName);
            }
        }
    }
}