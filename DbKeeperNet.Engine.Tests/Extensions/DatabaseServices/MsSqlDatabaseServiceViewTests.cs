using DbKeeperNet.Engine.Extensions.DatabaseServices;
using NUnit.Framework;

namespace DbKeeperNet.Engine.Tests.Extensions.DatabaseServices
{
    [TestFixture]
    [Explicit]
    [Category("mssql")]
    public class MsSqlDatabaseServiceViewTests: DatabaseServiceViewTests<MsSqlDatabaseService>
    {
        private const string APP_CONFIG_CONNECT_STRING = @"mssql";

        public MsSqlDatabaseServiceViewTests()
            : base(APP_CONFIG_CONNECT_STRING)
        {
        }

        protected override void CreateView(IDatabaseService connectedService, string viewName)
        {
            ExecuteSqlAndIgnoreException(connectedService, "create view {0} as select 1 as version", viewName);
        }

        protected override void DropView(IDatabaseService connectedService, string viewName)
        {
            ExecuteSqlAndIgnoreException(connectedService, "drop view {0}", viewName);
        }
    }
}
