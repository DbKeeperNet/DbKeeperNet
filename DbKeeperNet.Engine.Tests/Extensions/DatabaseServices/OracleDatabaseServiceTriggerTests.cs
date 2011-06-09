using System;
using System.Collections.Generic;
using System.Text;
using DbKeeperNet.Engine.Extensions.DatabaseServices;
using NUnit.Framework;

namespace DbKeeperNet.Engine.Tests.Extensions.DatabaseServices
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
            ExecuteSQLAndIgnoreException(connectedService, "create table \"{0}\"(id numeric(9,0) not null, id2 numeric(9,0))", TESTING_TABLE);

            ExecuteSQLAndIgnoreException(connectedService, @"create trigger ""{0}"" before insert on ""{1}""              
            		  for each row 
			            begin  
                            select :NEW.ID into :NEW.ID2 from dual;
			            end;", triggerName, TESTING_TABLE
                );
        }

        protected override void DropDatabaseTrigger(IDatabaseService connectedService, string triggerName)
        {
            ExecuteSQLAndIgnoreException(connectedService, @"drop trigger {0}", triggerName);
            ExecuteSQLAndIgnoreException(connectedService, @"drop table {0}", TESTING_TABLE);
        }
    }
}
