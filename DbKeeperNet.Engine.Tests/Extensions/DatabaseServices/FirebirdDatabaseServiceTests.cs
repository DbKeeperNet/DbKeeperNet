using System;
using System.Data.Common;
using DbKeeperNet.Engine.Extensions.DatabaseServices;
using NUnit.Framework;

namespace DbKeeperNet.Engine.Tests.Extensions.DatabaseServices
{
    [TestFixture]
    [Explicit]
    [Category("firebird")]
    public class FirebirdDatabaseServiceTests: DatabaseServiceTests<FirebirdDatabaseService>
    {
        private const string APP_CONFIG_CONNECT_STRING = @"firebird";

        public FirebirdDatabaseServiceTests()
            : base(APP_CONFIG_CONNECT_STRING)
        {
        }    
        
        [Test]
        public void TestExecuteSql()
        {
            using (IDatabaseService connectedService = CreateConnectedDbService())
            {
                connectedService.ExecuteSql(@"select rdb$relation_name from rdb$relations where 1 = 0");
            }
        }

        [Test]
        public void TestExecuteInvalidSqlStatement()
        {
            bool success = false;

            using (IDatabaseService connectedService = CreateConnectedDbService())
            {
                try
                {
                    connectedService.ExecuteSql("selectum magicum incorectum");
                }
                catch (DbException)
                {
                    success = true;
                }
            }

            Assert.That(success, Is.True);
        }

        [Test]
        [ExpectedException(typeof(NotSupportedException))]
        public void ForeignKeyExistsShouldThrowNotSupportedException()
        {
            using (var service = CreateConnectedDbService())
            {
                service.ForeignKeyExists(null, null);
            }
        }
    }
}