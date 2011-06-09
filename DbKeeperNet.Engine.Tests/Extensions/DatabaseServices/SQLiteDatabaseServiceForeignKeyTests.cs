using NUnit.Framework;
using DbKeeperNet.Engine.Extensions.DatabaseServices;

namespace DbKeeperNet.Engine.Tests.Extensions.DatabaseServices
{
    [TestFixture]
    [Explicit]
    [Category("sqlite")]
    public class SQLiteDatabaseServiceForeignKeyTests : DatabaseServiceForeignKeyTests<SQLiteDatabaseService>
    {
        private const string APP_CONFIG_CONNECT_STRING = @"sqlite";

        public SQLiteDatabaseServiceForeignKeyTests()
            : base(APP_CONFIG_CONNECT_STRING)
        {
        }

        protected override void CreateNamedForeignKey(IDatabaseService connectedService, string tableName, string foreignKeyName)
        {
            ExecuteSqlAndIgnoreException(connectedService, "create table {0}(id integer not null, rec_id int, CONSTRAINT PK_{0} PRIMARY KEY (id), CONSTRAINT {1} FOREIGN KEY (rec_id) REFERENCES testing_fk(id))", tableName, foreignKeyName);
        }

        protected override void DropNamedForeignKey(IDatabaseService connectedService, string tableName, string foreignKeyName)
        {
            ExecuteSqlAndIgnoreException(connectedService, "drop table {0}", tableName);
        }
    }
}
