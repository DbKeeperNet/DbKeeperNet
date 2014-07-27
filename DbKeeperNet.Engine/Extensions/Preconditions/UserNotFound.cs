using System;
using System.Globalization;
using DbKeeperNet.Engine.Resources;

namespace DbKeeperNet.Engine.Extensions.Preconditions
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
    public class UserNotFound : IPrecondition
    {
        #region Private fields

        private readonly IAspNetMembershipAdapter _membershipAdapter;

        #endregion


        /// <summary>
        /// Constructor
        /// </summary>
        public UserNotFound() : this(new AspNetMembershipAdapter())
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="membershipAdapter">Membership adapter</param>
        public UserNotFound(IAspNetMembershipAdapter membershipAdapter)
        {
            _membershipAdapter = membershipAdapter;
        }

        /// <summary>
        /// Precondition name as it's available and used in
        /// installation XML document.
        /// </summary>
        public string Name { get { return @"UserNotFound"; } }

        /// <summary>
        /// Method called for precondition evaluation. All specified
        /// preconditions must be met (return true) when checking
        /// whether step should or should not be executed.
        /// </summary>
        /// <param name="context">Current update context</param>
        /// <param name="param">Optional parameters which can be passed thru
        /// installation XML document. Each parameter can be optionaly named.</param>
        /// <returns>
        /// <list type="bullet">
        /// <item><c>true</c> - condition was met, step can be executed.</item>
        /// <item><c>false</c> - prevent step from execution.</item>
        /// </list>
        /// </returns>
        public bool CheckPrecondition(IUpdateContext context, PreconditionParamType[] param)
        {
            if ((param == null) || (param.Length != 1) || (string.IsNullOrEmpty(param[0].Value)))  
                throw new ArgumentNullException(string.Format(CultureInfo.CurrentCulture, PreconditionMessages.UserNotFoundUserNotSpecified, Name));

            return !_membershipAdapter.UserExists(param[0].Value);
        }
    }
}