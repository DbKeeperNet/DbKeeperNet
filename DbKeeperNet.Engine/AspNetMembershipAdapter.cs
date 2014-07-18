using System.Web.Security;

namespace DbKeeperNet.Engine
{
    /// <summary>
    /// An implementation of ASP.NET membership providers adapter
    /// </summary>
    public class AspNetMembershipAdapter : IAspNetMembershipAdapter
    {
        #region Private fields

        private MembershipProvider _membershipProvider;
        private RoleProvider _roleProvider;

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        public AspNetMembershipAdapter()
        {
            _membershipProvider = Membership.Provider;
            _roleProvider = Roles.Provider;
        }

        /// <summary>
        /// Create a new user
        /// </summary>
        /// <param name="userName">Seeded user's name</param>
        /// <param name="password">Seeded password</param>
        /// <param name="email">Associated email address</param>
        public void CreateUser(string userName, string password, string email)
        {
            MembershipCreateStatus status;
            _membershipProvider.CreateUser(userName, password, email, null, null, true, null, out status);

            if (status != MembershipCreateStatus.Success)
            {
                throw new DbKeeperNetException(string.Format("Creating user {0} failed with error code {1}", userName, status));
            }
        }

        /// <summary>
        /// Assign user to roles
        /// </summary>
        /// <param name="userName">User's name</param>
        /// <param name="roles">Collection of roles to which user should belong</param>
        public void AddUserToRoles(string userName, string[] roles)
        {
            foreach (var role in roles)
            {
                if (_roleProvider.RoleExists(role)) continue;
                _roleProvider.CreateRole(role);
            }

            _roleProvider.AddUsersToRoles(new []{userName}, roles);
        }

        public override string ToString()
        {
            return string.Format("AspNetMembershipAdapter[membership={0}, roles={1}]", _membershipProvider.Name, _roleProvider.Name);
        }
    }
}