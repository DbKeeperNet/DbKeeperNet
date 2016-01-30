using DbKeeperNet.Engine.Windows.Extensions.DatabaseServices;
using NUnit.Framework;

namespace DbKeeperNet.Engine.Windows.Tests.Extensions.DatabaseServices
{
    [TestFixture]
    [Explicit]
    [Category("mysql")]
    public class MySqlNetConnectorDatabaseServiceTableTests : DatabaseServiceTableTests<MySqlNetConnectorDatabaseService>
    {
        private const string APP_CONFIG_CONNECT_STRING = @"mysql";

        public MySqlNetConnectorDatabaseServiceTableTests()
            : base(APP_CONFIG_CONNECT_STRING)
        {
        }



        protected override void CreateTable(IDatabaseService connectedService, string tableName)
        {
            ExecuteSqlAndIgnoreException(connectedService, @"create table {0}(c char)", tableName);
        }

        protected override void DropTable(IDatabaseService connectedService, string tableName)
        {
            ExecuteSqlAndIgnoreException(connectedService, @"drop table {0}", tableName);
        }
    }
}
