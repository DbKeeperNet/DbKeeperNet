﻿using System.Web.Security;
using DbKeeperNet.Engine;
using DbKeeperNet.Engine.Configuration;
using DbKeeperNet.Engine.Tests;
using DbKeeperNet.Extensions.MsSqlMembershipAndRolesSetup;
using DbKeeperNet.Extensions.SqlServer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NUnit.Framework;

namespace DbKeeperNet.Extensions.AspNetRolesAndMembership.Tests
{
    [TestFixture]
    public class AspNetMembershipAdapterTests : TestBase
    {
        private const string UserName = "TestUser";
        private const string Password = "Some#StrongPassw0rd";
        private const string Email = "test@domain.com";
        private const string Role1 = "Role1";
        private const string Role2 = "Role2";
        private const string NonExistingRole = "Role3";

        protected override void Configure(IDbKeeperNetBuilder configurationBuilder)
        {
            configurationBuilder
                .UseSqlServer(ConnectionStrings.TestDatabase)
                .UseAspNetRolesAndMembership()
                .UseSqlServerMembershipAndRoleSetup()
                .UseSqlServerMembershipAndRoleSetupScript()
                ;


            configurationBuilder.Services.AddLogging(c => { c.AddConsole(); });
        }

        [SetUp]
        public override void Setup()
        {
            base.Setup();

            GetService<IDatabaseUpdater>().ExecuteUpgrade();

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
        public void DeleteUserShouldWorkCorrectly()
        {
            var adapter = new AspNetMembershipAdapter();

            adapter.CreateUser(UserName, Password, null);
            Assert.IsTrue(Membership.Provider.ValidateUser(UserName, Password));

            Assert.IsTrue(adapter.DeleteUser(UserName));
            Assert.IsFalse(adapter.DeleteUser(UserName));
        }

        [Test]
        public void AddUserToRolesShouldWorkCorrectly()
        {
            var adapter = new AspNetMembershipAdapter();
            adapter.CreateUser(UserName, Password, null);

            adapter.AddUserToRoles(UserName, new[] { Role1, Role2 });

            Assert.IsTrue(Roles.Provider.IsUserInRole(UserName, Role1));
            Assert.IsTrue(Roles.Provider.IsUserInRole(UserName, Role2));
            Assert.IsFalse(Roles.Provider.IsUserInRole(UserName, NonExistingRole));
        }

        [Test]
        public void UserExistsShouldReturnTrueForExistingUser()
        {
            MembershipCreateStatus status;
            Membership.Provider.CreateUser(UserName, Password, null, null, null, true, null, out status);

            Assert.AreEqual(MembershipCreateStatus.Success, status);

            var adapter = new AspNetMembershipAdapter();
            Assert.IsTrue(adapter.UserExists(UserName));
        }

        [Test]
        public void CreateRoleShouldWorkCorrectly()
        {
            var adapter = new AspNetMembershipAdapter();
            adapter.CreateRole(Role1);

            Assert.IsTrue(Roles.Provider.RoleExists(Role1));
        }

        [Test]
        public void DeleteRoleShouldWorkCorrectly()
        {
            Roles.Provider.CreateRole(Role1);

            var adapter = new AspNetMembershipAdapter();
            adapter.DeleteRole(Role1);

            Assert.IsFalse(Roles.Provider.RoleExists(Role1));
        }

        [Test]
        public void RoleExistsShouldReturnTrueForExistingRole()
        {
            Roles.Provider.CreateRole(Role1);

            var adapter = new AspNetMembershipAdapter();
            Assert.IsTrue(adapter.RoleExists(Role1));
        }

        [Test]
        public void RoleExistsShouldReturnFalseForNonExistingRole()
        {
            var adapter = new AspNetMembershipAdapter();
            Assert.IsFalse(adapter.RoleExists(Role1));
        }

        [Test]
        public void UserExistsShouldReturnFalseForNonExistingUser()
        {
            var adapter = new AspNetMembershipAdapter();
            Assert.IsFalse(adapter.UserExists("NonExistingUser"));
        }
    }
}