using NUnit.Framework;
using Rhino.Mocks;

namespace DbKeeperNet.Engine.Tests
{
    [TestFixture]
    public class UpdateStepVisitorTests
    {
        private const string UserName = "TestUser";
        private const string Password = "TestPassword";
        private const string Email = "test@domain.com";
        private const string Role1 = "Role1";
        private const string Role2 = "Role2";

        [Test]
        public void AspNetAccountCreateUpdateStepTypeShouldBeProcessedCorrectly()
        {
            var repository = new MockRepository();
            var loggerStub = repository.Stub<ILoggingService>();
            var contextMock = repository.StrictMock<IUpdateContext>();
            var adapter = repository.StrictMock<IAspNetMembershipAdapter>();

            using (repository.Record())
            {
                SetupResult.For(contextMock.Logger).Return(loggerStub);
                Expect.Call(() => adapter.CreateUser(UserName, Password, Email));
                Expect.Call(() => adapter.AddUserToRoles(UserName, new[] { Role1, Role2 }));
            }

            var createUser = new AspNetAccountCreateUpdateStepType { UserName = UserName, Password = Password, Mail = Email, Role = new[] { Role1, Role2 } };

            using (repository.Playback())
            {
                var visitor = new UpdateStepVisitor(contextMock, null, adapter);
                createUser.Accept(visitor);
            }

            repository.VerifyAll();
        }

        [Test]
        public void AspNetAccountDeleteStepTypeShouldBeProcessedCorrectly()
        {
            var repository = new MockRepository();
            var loggerMock = repository.StrictMock<ILoggingService>();
            var contextMock = repository.StrictMock<IUpdateContext>();
            var adapter = repository.StrictMock<IAspNetMembershipAdapter>();

            using (repository.Record())
            {
                Expect.Call(() => loggerMock.TraceInformation(Arg<string>.Is.Anything, Arg<object[]>.Is.Anything)).Repeat.Times(3);
                SetupResult.For(contextMock.Logger).Return(loggerMock);
                SetupResult.For(adapter.DeleteUser(UserName)).Return(true);
            }

            var createUser = new AspNetAccountDeleteUpdateStepType { UserName = UserName };

            using (repository.Playback())
            {
                var visitor = new UpdateStepVisitor(contextMock, null, adapter);
                createUser.Accept(visitor);
            }

            repository.VerifyAll();
        }

        [Test]
        public void AspNetCreateRoleStepTypeShouldBeProcessedCorrectly()
        {
            var repository = new MockRepository();
            var loggerMock = repository.Stub<ILoggingService>();
            var contextMock = repository.StrictMock<IUpdateContext>();
            var adapter = repository.StrictMock<IAspNetMembershipAdapter>();

            using (repository.Record())
            {
                SetupResult.For(contextMock.Logger).Return(loggerMock);
                Expect.Call(() => adapter.CreateRole(Role1));
            }

            var createUser = new AspNetRoleCreateUpdateStepType { RoleName = Role1 };

            using (repository.Playback())
            {
                var visitor = new UpdateStepVisitor(contextMock, null, adapter);
                createUser.Accept(visitor);
            }

            repository.VerifyAll();
        }

        [Test]
        public void AspNetDeleteRoleStepTypeShouldBeProcessedCorrectly()
        {
            var repository = new MockRepository();
            var loggerMock = repository.Stub<ILoggingService>();
            var contextMock = repository.StrictMock<IUpdateContext>();
            var adapter = repository.StrictMock<IAspNetMembershipAdapter>();

            using (repository.Record())
            {
                SetupResult.For(contextMock.Logger).Return(loggerMock);
                Expect.Call(() => adapter.DeleteRole(Role1));
            }

            var createUser = new AspNetRoleDeleteUpdateStepType { RoleName = Role1 };

            using (repository.Playback())
            {
                var visitor = new UpdateStepVisitor(contextMock, null, adapter);
                createUser.Accept(visitor);
            }

            repository.VerifyAll();
        }

        [Test]
        public void AspNetAccountCreateDeleteStepTypeShouldLogWarningIfUserWasNotDeleted()
        {
            var repository = new MockRepository();
            var loggerMock = repository.StrictMock<ILoggingService>();
            var contextMock = repository.StrictMock<IUpdateContext>();
            var adapter = repository.StrictMock<IAspNetMembershipAdapter>();

            using (repository.Record())
            {
                Expect.Call(() => loggerMock.TraceInformation(Arg<string>.Is.Anything, Arg<object[]>.Is.Anything)).Repeat.Times(2);
                Expect.Call(() => loggerMock.TraceWarning(Arg<string>.Is.Anything, Arg<object[]>.Is.Anything));
                SetupResult.For(contextMock.Logger).Return(loggerMock);
                SetupResult.For(adapter.DeleteUser(UserName)).Return(false);
            }

            var createUser = new AspNetAccountDeleteUpdateStepType { UserName = UserName };

            using (repository.Playback())
            {
                var visitor = new UpdateStepVisitor(contextMock, null, adapter);
                createUser.Accept(visitor);
            }

            repository.VerifyAll();
        }
    }
}