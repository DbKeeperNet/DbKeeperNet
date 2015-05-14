using System;
using NUnit.Framework;
using DbKeeperNet.Engine.Extensions.DatabaseServices;
using System.Data.Common;

namespace DbKeeperNet.Engine.Tests.Extensions.DatabaseServices
{
    [TestFixture]
    [Explicit]
    [Category("oracle")]
    public class OracleDatabaseServiceTests: DatabaseServiceTests<OracleDatabaseService>
    {
		private const string UNKNOWN_SEQUENCE_NAME = "ora_testing_unknown_seq";
		private const string SEQUENCE_NAME = "ora_testing_seq";
    	private const string APP_CONFIG_CONNECT_STRING = @"oracle";

        public OracleDatabaseServiceTests()
            : base(APP_CONFIG_CONNECT_STRING)
        {
        }

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
			CreateTestSequenceInDatabase();

            Assert.That(TestSequenceExists(UNKNOWN_SEQUENCE_NAME), Is.False);
        }

        [Test]
        public void TestSequenceNotExistsNullName()
        {
            Assert.Throws<ArgumentNullException>(() => TestSequenceExists(null));
        }

        [Test]
        public void TestSequenceNotExistsEmptyName()
        {
            Assert.Throws<ArgumentNullException>(() => TestSequenceExists(""));
        }

        [Test]
        public void TestSequenceExists()
        {
        	CreateTestSequenceInDatabase();

        	Assert.That(TestSequenceExists(SEQUENCE_NAME), Is.True);
        }

		#region Private helpers

		private void CreateTestSequenceInDatabase()
    	{
    		using (IDatabaseService connectedService = CreateConnectedDbService())
    		{
    			CreateSequence(connectedService);
    		}
    	}

    	private static void CreateSequence(IDatabaseService connectedService)
        {
            ExecuteSqlAndIgnoreException(connectedService, "create sequence \"{0}\"", SEQUENCE_NAME);
        }

		private bool TestSequenceExists(string sequence)
		{
			bool result;

			using (OracleDatabaseService connectedService = CreateConnectedDbService())
			{
				result = connectedService.SequenceExists(sequence);
			}
			return result;
		}

		private void CleanupDatabase()
		{
			using (IDatabaseService connectedService = CreateConnectedDbService())
			{
				ExecuteSqlAndIgnoreException(connectedService, "drop sequence \"{0}\"", SEQUENCE_NAME);
			}
		}

		#endregion
    }
}
