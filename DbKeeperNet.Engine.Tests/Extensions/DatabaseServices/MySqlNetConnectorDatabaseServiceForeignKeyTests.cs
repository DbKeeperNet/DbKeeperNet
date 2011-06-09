using DbKeeperNet.Engine.Extensions.DatabaseServices;
using NUnit.Framework;

namespace DbKeeperNet.Engine.Tests.Extensions.DatabaseServices
{
    [TestFixture]
    [Explicit]
    [Category("mysql")]
    public class MySqlNetConnectorDatabaseServiceForeignKeyTests : DatabaseServiceForeignKeyTests<MySqlNetConnectorDatabaseService>
    {
        private const string APP_CONFIG_CONNECT_STRING = @"mysql";

        public MySqlNetConnectorDatabaseServiceForeignKeyTests()
            : base(APP_CONFIG_CONNECT_STRING)
        {
        }

        protected override void CreateNamedForeignKey(IDatabaseService connectedService, string tableName, string foreignKeyName)
        {
            ExecuteSqlAndIgnoreException(connectedService, "create table mysql_testing_fk(id int not null, CONSTRAINT PK_mysql_testing_fk PRIMARY KEY (id))");
            ExecuteSqlAndIgnoreException(connectedService, "CREATE TABLE {0}(rec_id int, CONSTRAINT {1} FOREIGN KEY (rec_id) REFERENCES mysql_testing_fk(id))", tableName, foreignKeyName);
        }

        protected override void DropNamedForeignKey(IDatabaseService connectedService, string tableName, string foreignKeyName)
        {
            ExecuteSqlAndIgnoreException(connectedService, "drop table {0}", tableName);
            ExecuteSqlAndIgnoreException(connectedService, "drop table mysql_testing_fk");
        }
    }
}
