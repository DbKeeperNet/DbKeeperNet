using System;
using DbKeeperNet.Engine;

namespace DbKeeperNet.Extensions.AspNetRolesAndMembership.Preconditions
{
    /// <summary>
    /// Condition verifies that user with given login name doesn't exist.
    /// </summary>
    /// <remarks>
    /// Condition reference name is <c>UserNotFound</c>.
    /// It has single parameter which should contain user's login name.
    /// </remarks>
    /// <example>
    /// Following example shows how to reference this condition in the
    /// update script XML.
    /// <code>
    /// <![CDATA[
    /// <Precondition FriendlyName="User test not found" Precondition="UserNotFound">
    ///   <Param>test</Param>
    /// </Precondition>
    /// ]]>
    /// </code>
    /// </example>
    public class UserNotFound : IPreconditionHandler
    {
        #region Private fields

        private readonly IAspNetMembershipAdapter _membershipAdapter;

        #endregion


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="membershipAdapter">Membership adapter</param>
        public UserNotFound(IAspNetMembershipAdapter membershipAdapter)
        {
            _membershipAdapter = membershipAdapter;
        }

        public bool CanHandle(UpdateStepContextPrecondition context)
        {
            return @"UserNotFound" == context.Precondition.Precondition;
        }

        public bool IsMet(UpdateStepContextPrecondition context)
        {
            var param = context.Precondition.Param;
            if ((param == null) || (param.Length != 1) || (string.IsNullOrEmpty(param[0].Value)))  
                throw new ArgumentNullException("User name must be specified as a parameter for condition UserNotFound");

            return !_membershipAdapter.UserExists(param[0].Value);
        }
    }
}