using DbKeeperNet.Engine;
using DbKeeperNet.Engine.UpdateStepHandlers;

namespace DbKeeperNet.Extensions.AspNetRolesAndMembership
{
    public class AspNetMembershipAccountCreateUpdateStepHandlerService : UpdateStepHandlerBase<AspNetAccountCreateUpdateStepType>
    {
        private readonly IAspNetMembershipAdapter _aspNetMembershipAdapter;

        public AspNetMembershipAccountCreateUpdateStepHandlerService(IAspNetMembershipAdapter aspNetMembershipAdapter)
        {
            _aspNetMembershipAdapter = aspNetMembershipAdapter;
        }

        protected override void Execute(UpdateStepContextWithPreconditions<AspNetAccountCreateUpdateStepType> context)
        {
            // context.Logger.TraceInformation(AspNetMembershipAdapterMessages.GoingToUseAdapter, _aspNetMembershipAdapter);

            // context.Logger.TraceInformation(AspNetMembershipAdapterMessages.AddingUser, castedStep.UserName);
            var castedStep = context.Step;

            _aspNetMembershipAdapter.CreateUser(castedStep.UserName, castedStep.Password, castedStep.Mail);
            // context.Logger.TraceInformation(AspNetMembershipAdapterMessages.AddedUser, castedStep.UserName);

            if (castedStep.Role != null && castedStep.Role.Length != 0)
            {
               // context.Logger.TraceInformation(AspNetMembershipAdapterMessages.AddingUserToRoles, castedStep.UserName, string.Join(@",", castedStep.Role));
                _aspNetMembershipAdapter.AddUserToRoles(castedStep.UserName, castedStep.Role);
              //  context.Logger.TraceInformation(AspNetMembershipAdapterMessages.UserAddedToRoles, castedStep.UserName);
            }
        }
    }
}