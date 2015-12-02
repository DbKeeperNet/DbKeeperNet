using DbKeeperNet.Engine.Windows.Extensions.DatabaseServices;
using NUnit.Framework;

namespace DbKeeperNet.Engine.Windows.Tests.Extensions.DatabaseServices
{
    [TestFixture]
    [Explicit]
    [Category("mssql")]
    public class MsSqlDatabaseServiceTableTests: DatabaseServiceTableTests<MsSqlDatabaseService>
    {
        private const string APP_CONFIG_CONNECT_STRING = @"mssql";

        public MsSqlDatabaseServiceTableTests()
            : base(APP_CONFIG_CONNECT_STRING)
        {
        }

        protected override void CreateTable(IDatabaseService connectedService, string tableName)
        {
            ExecuteSqlAndIgnoreException(connectedService, @"create table {0}(c varchar)", tableName);
        }

        protected override void DropTable(IDatabaseService connectedService, string tableName)
        {
            ExecuteSqlAndIgnoreException(connectedService, @"drop table {0}", tableName);
        }
    }
}
