using System;
using System.Collections.Generic;
using System.Text;
using DbKeeperNet.Engine.Extensions.DatabaseServices;
using NUnit.Framework;

namespace DbKeeperNet.Engine.Tests.Extensions.DatabaseServices
{
    [TestFixture]
    [Explicit]
    [Category("mysql")]
    public class MySqlDatabaseServiceForeignKeyTests : DatabaseServiceForeignKeyTests<MySqlNetConnectorDatabaseService>
    {
        private const string APP_CONFIG_CONNECT_STRING = @"mysql";

        public MySqlDatabaseServiceForeignKeyTests()
            : base(APP_CONFIG_CONNECT_STRING)
        {
        }

        protected override void CreateNamedForeignKey(IDatabaseService connectedService, string tableName, string foreignKeyName)
        {
            ExecuteSQLAndIgnoreException(connectedService, "create table mysql_testing_fk(id int not null, CONSTRAINT PK_mysql_testing_fk PRIMARY KEY (id))");
            ExecuteSQLAndIgnoreException(connectedService, "CREATE TABLE {0}(rec_id int, CONSTRAINT {1} FOREIGN KEY (rec_id) REFERENCES mysql_testing_fk(id))", tableName, foreignKeyName);
        }

        protected override void DropNamedForeignKey(IDatabaseService connectedService, string tableName, string foreignKeyName)
        {
            ExecuteSQLAndIgnoreException(connectedService, "drop table {0}", tableName);
            ExecuteSQLAndIgnoreException(connectedService, "drop table mysql_testing_fk");
        }
    }
}
