using System.Web.Security;

namespace DbKeeperNet.Engine
{
    /// <summary>
    /// An implementation of ASP.NET membership providers adapter
    /// </summary>
    public class AspNetMembershipAdapter : IAspNetMembershipAdapter
    {
        /// <summary>
        /// Create a new user
        /// </summary>
        /// <param name="userName">Seeded user's name</param>
        /// <param name="password">Seeded password</param>
        /// <param name="email">Associated email address</param>
        public void CreateUser(string userName, string password, string email)
        {
            MembershipCreateStatus status;
            Membership.Provider.CreateUser(userName, password, email, null, null, true, null, out status);

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
            var roleProvider = Roles.Provider;

            foreach (var role in roles)
            {
                if (roleProvider.RoleExists(role)) continue;
                roleProvider.CreateRole(role);
            }

            roleProvider.AddUsersToRoles(new []{userName}, roles);
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        /// A string that represents the current object.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override string ToString()
        {
            return string.Format("AspNetMembershipAdapter[membership={0}, roles={1}]", Membership.Provider.Name, Roles.Provider.Name);
        }
    }
}