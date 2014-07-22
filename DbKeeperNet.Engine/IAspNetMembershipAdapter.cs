namespace DbKeeperNet.Engine
{
    /// <summary>
    /// An interface for adapter to ASP.NET membership providers
    /// </summary>
    public interface IAspNetMembershipAdapter
    {
        /// <summary>
        /// Create a new user
        /// </summary>
        /// <param name="userName">Seeded user's name</param>
        /// <param name="password">Seeded password</param>
        /// <param name="email">Associated email address</param>
        void CreateUser(string userName, string password, string email);

        /// <summary>
        /// Assign user to roles
        /// </summary>
        /// <param name="userName">User's name</param>
        /// <param name="roles">Collection of roles to which user should belong</param>
        void AddUserToRoles(string userName, string[] roles);

        /// <summary>
        /// Delete an existing user
        /// </summary>
        /// <param name="userName">Existing user's name</param>
        /// <returns><c>true</c> if user was deleted, <c>false</c> otherwise</returns>
        bool DeleteUser(string userName);
    }
}