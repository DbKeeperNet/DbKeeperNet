using DbKeeperNet.Engine.Windows.Extensions.DatabaseServices;
using NUnit.Framework;

namespace DbKeeperNet.Engine.Windows.Tests.Extensions.DatabaseServices
{
    [TestFixture]
    [Explicit]
    [Category("oracle")]
    public class OracleDatabaseServiceTriggerTests : DatabaseServiceTriggerTests<OracleDatabaseService>
    {
        private const string APP_CONFIG_CONNECT_STRING = @"oracle";
        private const string TESTING_TABLE = @"orasql_testing_tr";

        public OracleDatabaseServiceTriggerTests()
            : base(APP_CONFIG_CONNECT_STRING)
        {
        }

        protected override void CreateDatabaseTrigger(IDatabaseService connectedService, string triggerName)
        {
            ExecuteSqlAndIgnoreException(connectedService, "create table \"{0}\"(id numeric(9,0) not null, id2 numeric(9,0))", TESTING_TABLE);

            ExecuteSqlAndIgnoreException(connectedService, @"create trigger ""{0}"" before insert on ""{1}""              
            		  for each row 
			            begin  
                            select :NEW.ID into :NEW.ID2 from dual;
			            end;", triggerName, TESTING_TABLE
                );
        }

        protected override void DropDatabaseTrigger(IDatabaseService connectedService, string triggerName)
        {
            ExecuteSqlAndIgnoreException(connectedService, @"drop trigger ""{0}""", triggerName);
            ExecuteSqlAndIgnoreException(connectedService, @"drop table ""{0}""", TESTING_TABLE);
        }
    }
}
