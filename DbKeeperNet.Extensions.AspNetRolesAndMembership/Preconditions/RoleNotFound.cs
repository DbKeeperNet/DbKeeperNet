using System;
using DbKeeperNet.Engine;

namespace DbKeeperNet.Extensions.AspNetRolesAndMembership.Preconditions
{
    /// <summary>
    /// Condition verifies that role with given name doesn't exist.
    /// </summary>
    /// <remarks>
    /// Condition reference name is <c>RoleNotFound</c>.
    /// It has single parameter which should contain role name.
    /// </remarks>
    /// <example>
    /// Following example shows how to reference this condition in the
    /// update script XML.
    /// <code>
    /// <![CDATA[
    /// <Precondition FriendlyName="Role test not found" Precondition="RoleNotFound">
    ///   <Param>test</Param>
    /// </Precondition>
    /// ]]>
    /// </code>
    /// </example>
    public class RoleNotFound : IPreconditionHandler
    {
        #region Private fields

        private readonly IAspNetMembershipAdapter _membershipAdapter;

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="membershipAdapter">Membership adapter</param>
        public RoleNotFound(IAspNetMembershipAdapter membershipAdapter)
        {
            _membershipAdapter = membershipAdapter;
        }

        public bool CanHandle(UpdateStepContextPrecondition context)
        {
            return @"RoleNotFound" == context.Precondition.Precondition;
        }

        public bool IsMet(UpdateStepContextPrecondition context)
        {
            var param = context.Precondition.Param;

            if ((param == null) || (param.Length != 1) || (string.IsNullOrEmpty(param[0].Value)))
                throw new ArgumentNullException("Role name must be specified as a parameter for condition RoleNotFound");

            return !_membershipAdapter.RoleExists(param[0].Value);
        }
    }
}