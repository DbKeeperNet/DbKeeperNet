using DbKeeperNet.Engine.Windows.Extensions.DatabaseServices;
using NUnit.Framework;

namespace DbKeeperNet.Engine.Windows.Tests.Extensions.DatabaseServices
{
    [TestFixture]
    [Explicit]
    [Category("pgsql")]
    public class PgSqlDatabaseServiceViewTests : DatabaseServiceViewTests<PgSqlDatabaseService>
    {
        private const string APP_CONFIG_CONNECT_STRING = @"pgsql";

        public PgSqlDatabaseServiceViewTests()
            : base(APP_CONFIG_CONNECT_STRING)
        {
        }

        #region Overrides of DatabaseServiceViewTests<PgSqlDatabaseService>

        protected override void CreateView(IDatabaseService connectedService, string viewName)
        {
            ExecuteSqlAndIgnoreException(connectedService, "create view {0} as select 1 as version", viewName);
        }

        protected override void DropView(IDatabaseService connectedService, string viewName)
        {
            ExecuteSqlAndIgnoreException(connectedService, "drop view {0}", viewName);
        }

        #endregion
    }
}