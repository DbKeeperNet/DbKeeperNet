using System.Web.Security;
using NUnit.Framework;

namespace DbKeeperNet.Engine.Tests
{
    [Explicit]
    [Category("mssql")]
    [TestFixture]
    public class AspNetMembershipAdapterTests
    {
        private const string UserName = "TestUser";
        private const string Password = "Some#StrongPassw0rd";
        private const string Email = "test@domain.com";
        private const string Role1 = "Role1";
        private const string Role2 = "Role2";
        private const string NonExistingRole = "Role3";

        [TestFixtureSetUp]
        public void TestClassSetup()
        {
            var updateContext = new UpdateContext();
            updateContext.LoadExtensions();
            updateContext.InitializeLoggingService("fx");
            updateContext.InitializeDatabaseService("mssql");

            var updater = new Updater(updateContext,
                                      new UpdateStepVisitor(updateContext, new NonSplittingSqlScriptSplitter(),
                                                            new AspNetMembershipAdapter()));

            updater.ExecuteXml(typeof(AspNetMembershipAdapterTests).Assembly.GetManifestResourceStream("DbKeeperNet.Engine.Tests.AspNetMemberShipUpdate.xml"));
        }

        [SetUp]
        public void Setup()
        {
            Membership.Provider.DeleteUser(UserName, true);

            Roles.Provider.DeleteRole(Role1, false);
            Roles.Provider.DeleteRole(Role2, false);
        }

        [Test]
        public void CreateUserWithEmailShouldWorkCorrectly()
        {
            var adapter = new AspNetMembershipAdapter();

            adapter.CreateUser(UserName, Password, Email);

            int recordCount;
            
            Membership.Provider.FindUsersByEmail(Email, 0, 1, out recordCount);
            Assert.That(recordCount, Is.EqualTo(1));

            Assert.IsTrue(Membership.Provider.ValidateUser(UserName, Password));
        }

        [Test]
        public void CreateUserWithoutEmailShouldWorkCorrectly()
        {
            var adapter = new AspNetMembershipAdapter();

            adapter.CreateUser(UserName, Password, null);

            Assert.IsTrue(Membership.Provider.ValidateUser(UserName, Password));
        }

        [Test]
        public void AddUserToRolesShouldWorkCorrectly()
        {
            var adapter = new AspNetMembershipAdapter();
            adapter.CreateUser(UserName, Password, null);

            adapter.AddUserToRoles(UserName, new[]{Role1, Role2});

            Assert.IsTrue(Roles.Provider.IsUserInRole(UserName, Role1));
            Assert.IsTrue(Roles.Provider.IsUserInRole(UserName, Role2));
            Assert.IsFalse(Roles.Provider.IsUserInRole(UserName, NonExistingRole));
        }
    }
}