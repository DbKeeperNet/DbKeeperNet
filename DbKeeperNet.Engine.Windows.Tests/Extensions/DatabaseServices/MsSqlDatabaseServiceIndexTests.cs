using DbKeeperNet.Engine.Windows.Extensions.DatabaseServices;
using NUnit.Framework;

namespace DbKeeperNet.Engine.Windows.Tests.Extensions.DatabaseServices
{
    [TestFixture]
    [Explicit]
    [Category("mssql")]
    public class MsSqlDatabaseServiceIndexTests: DatabaseServiceIndexTests<MsSqlDatabaseService>
    {
        private const string APP_CONFIG_CONNECT_STRING = @"mssql";

        public MsSqlDatabaseServiceIndexTests()
            : base(APP_CONFIG_CONNECT_STRING)
        {
        }


        protected override void CreateNamedIndex(IDatabaseService connectedService, string tableName, string indexName)
        {
            ExecuteSqlAndIgnoreException(connectedService, @"create table {0}(id int not null);CREATE INDEX {1} on {0} (id)", tableName, indexName);
        }

        protected override void DropNamedIndex(IDatabaseService connectedService, string tableName, string indexName)
        {
            ExecuteSqlAndIgnoreException(connectedService, @"drop table {0}", tableName);
        }
    }
}
