using System;
using DbKeeperNet.Engine.Extensions.DatabaseServices;
using NUnit.Framework;

namespace DbKeeperNet.Engine.Tests.Extensions.DatabaseServices
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
            ExecuteSQLAndIgnoreException(connectedService, "create view {0} as select 1 as version", viewName);
        }

        protected override void DropView(IDatabaseService connectedService, string viewName)
        {
            ExecuteSQLAndIgnoreException(connectedService, "drop view {0}", viewName);
        }

        #endregion
    }
}