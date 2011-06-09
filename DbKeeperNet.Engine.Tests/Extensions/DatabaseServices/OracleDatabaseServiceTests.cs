using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using DbKeeperNet.Engine.Extensions.DatabaseServices;
using System.Data.Common;
using System.Diagnostics;

namespace DbKeeperNet.Engine.Tests.Extensions.DatabaseServices
{
    [TestFixture]
    [Explicit]
    [Category("oracle")]
    public class OracleDatabaseServiceTests: DatabaseServiceTests<OracleDatabaseService>
    {
        public OracleDatabaseServiceTests()
            : base(@"oracle")
        {
        }

        #region Private helpers
        protected bool TestSequenceExists(string sequence)
        {
            bool result = false;

            using (OracleDatabaseService connectedService = CreateConnectedDbService())
            {
                result = connectedService.SequenceExists(sequence);
            }
            return result;
        }
        #endregion

        [SetUp]
        public void Startup()
        {
            CleanupDatabase();
        }

        [TearDown]
        public void Shutdown()
        {
            CleanupDatabase();
        }


        private void CleanupDatabase()
        {
            using (IDatabaseService connectedService = CreateConnectedDbService())
            {
                ExecuteSQLAndIgnoreException(connectedService, "drop sequence \"ora_testing_seq\"");
            }
        }

        [Test]
        public void TestExecuteSql()
        {
            using (IDatabaseService connectedService = CreateConnectedDbService())
            {
                connectedService.ExecuteSql("select sysdate from dual");
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
        public void TestSequenceNotExists()
        {
            TestSequenceExists("asddas");
        }
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestSequenceNotExistsNullName()
        {
            TestSequenceExists(null);
        }
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestSequenceNotExistsEmptyName()
        {
            TestSequenceExists("");
        }
        [Test]
        public void TestSequenceExists()
        {
            using (IDatabaseService connectedService = CreateConnectedDbService())
            {
                CreateSequence(connectedService);
                
                Assert.That(TestSequenceExists("ora_testing_seq"), Is.True);
            }
        }

        private static void CreateSequence(IDatabaseService connectedService)
        {
            ExecuteSQLAndIgnoreException(connectedService, "create sequence \"ora_testing_seq\"");
        }
    }
}
