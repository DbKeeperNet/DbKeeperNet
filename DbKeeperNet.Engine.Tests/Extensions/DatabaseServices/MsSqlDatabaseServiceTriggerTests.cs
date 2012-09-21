using DbKeeperNet.Engine.Extensions.DatabaseServices;
using NUnit.Framework;

namespace DbKeeperNet.Engine.Tests.Extensions.DatabaseServices
{
    [TestFixture]
    [Explicit]
    [Category("mssql")]
    public class MsSqlDatabaseServiceTriggerTests: DatabaseServiceTriggerTests<MsSqlDatabaseService>
    {
        private const string APP_CONFIG_CONNECT_STRING = @"mssql";
        private const string TESTING_TABLE = @"mssql_testing_tr";

        public MsSqlDatabaseServiceTriggerTests()
            : base(APP_CONFIG_CONNECT_STRING)
        {
        }

        protected override void CreateDatabaseTrigger(IDatabaseService connectedService, string triggerName)
        {
            ExecuteSqlAndIgnoreException(connectedService, @"create table {0} (id numeric(9,0) not null, id2 numeric(9,0))", TESTING_TABLE);
            ExecuteSqlAndIgnoreException(connectedService, @"create trigger {0} on {1} after insert               
        		  as
		            update {1} set id2 = id", triggerName, TESTING_TABLE
                );
        }

        protected override void DropDatabaseTrigger(IDatabaseService connectedService, string triggerName)
        {
            ExecuteSqlAndIgnoreException(connectedService, @"drop table {0}", TESTING_TABLE);
        }
    }
}
