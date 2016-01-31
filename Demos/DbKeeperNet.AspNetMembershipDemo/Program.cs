using System;
using System.Web.Security;
using DbKeeperNet.Engine;
using DbKeeperNet.Engine.Windows;

namespace DbKeeperNet.AspNetMembershipDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            // since we are using MSSQL Express user instance from a console application
            // please make sure that the connection string points to a valid absolute path
            const string connString = "default"; // MsSql connection

            try
            {
                using (UpdateContext context = new WindowsUpdateContext())
                {
                    context.LoadExtensions();
                    context.InitializeDatabaseService(connString);

                    Updater updater = new Updater(context);
                    updater.ExecuteXmlFromConfig();
                }

                int total;
                var users = Membership.Provider.GetAllUsers(0, 99, out total);
                foreach (MembershipUser user in users)
                {
                    Console.WriteLine("User: " + user.Email + " " + user.UserName);
                }

                var roles = Roles.Provider.GetAllRoles();
                foreach (var role in roles)
                {
                    Console.WriteLine("Role: " + role);
                }

                Console.WriteLine("Can login as TestUser2: " + Membership.Provider.ValidateUser("Testuser2", "SeededPassword2"));
                Console.WriteLine("Can login as TestUser2: " + Membership.Provider.ValidateUser("testuser2", "InvalidPassword"));
                Console.WriteLine("Is user testuser2 in role testrole1: " + Roles.Provider.IsUserInRole("testuser2", "testrole1"));
                Console.WriteLine("Is user testuser2 in role testrole2: " + Roles.Provider.IsUserInRole("testuser2", "testrole2"));
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex);
                throw;
            }

            Console.ReadKey();
        }
    }
}
