using System;
using System.Globalization;
using System.Web.Security;

namespace DbKeeperNet.Extensions.AspNetRolesAndMembership
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
               //TODO: throw new DbKeeperNetException(string.Format(CultureInfo.CurrentCulture, AspNetMembershipAdapterMessages.CreatingUserFailed, userName, status));
               throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Creating user {0} failed with error code {1}", userName, status));
            }
        }

        /// <summary>
        /// Assign user to roles
        /// </summary>
        /// <param name="userName">User's name</param>
        /// <param name="roles">Collection of roles to which user should belong</param>
        public void AddUserToRoles(string userName, string[] roles)
        {
            if (string.IsNullOrEmpty(userName)) throw new ArgumentNullException(@"userName");
            if (roles == null) throw new ArgumentNullException(@"roles");

            var roleProvider = Roles.Provider;

            foreach (var role in roles)
            {
                if (roleProvider.RoleExists(role)) continue;
                roleProvider.CreateRole(role);
            }

            roleProvider.AddUsersToRoles(new[] { userName }, roles);
        }

        /// <summary>
        /// Delete an existing user
        /// </summary>
        /// <param name="userName">Existing user's name</param>
        /// <returns><c>true</c> if user was deleted, <c>false</c> otherwise</returns>
        public bool DeleteUser(string userName)
        {
            return Membership.Provider.DeleteUser(userName, true);
        }

        /// <summary>
        /// Checks whether the <paramref name="userName"/> exists.
        /// </summary>
        /// <param name="userName">Login name of the user to be checked</param>
        /// <returns><c>true</c> if user exists, <c>false</c> otherwise</returns>
        public bool UserExists(string userName)
        {
            var user = Membership.Provider.GetUser(userName, false);

            return (user != null);
        }

        /// <summary>
        /// Creates a new role
        /// </summary>
        /// <param name="roleName">Role name to be created</param>
        public void CreateRole(string roleName)
        {
            Roles.Provider.CreateRole(roleName);
        }

        /// <summary>
        /// Creates an existing role
        /// </summary>
        /// <remarks>The role is deleted regardless possibly existing assignments</remarks>
        /// <param name="roleName">Role name to be deleted</param>
        public void DeleteRole(string roleName)
        {
            Roles.Provider.DeleteRole(roleName, false);
        }

        /// <summary>
        /// Checkes whether the <paramref name="roleName"/> exists.
        /// </summary>
        /// <param name="roleName">Role name which presence needs to be checked</param>
        /// <returns><c>true</c> if role exists, <c>false</c> if it does not exist.</returns>
        public bool RoleExists(string roleName)
        {
            return Roles.Provider.RoleExists(roleName);
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
            return string.Format(CultureInfo.InvariantCulture, "Asp.Net membership adapter[membership={0}, roles={1}]", Membership.Provider.Name, Roles.Provider.Name);
        }
    }
}