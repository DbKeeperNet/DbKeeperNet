using System;
using System.Collections.Generic;
using System.Text;
using DbKeeperNet.Engine.Extensions.DatabaseServices;
using NUnit.Framework;

namespace DbKeeperNet.Engine.Tests.Extensions.DatabaseServices
{
    [TestFixture]
    [Explicit]
    [Category("sqlite")]
    public class SQLiteDatabaseServiceViewTests : DatabaseServiceViewTests<SQLiteDatabaseService>
    {
        private const string APP_CONFIG_CONNECT_STRING = @"sqlite";

        public SQLiteDatabaseServiceViewTests()
            : base(APP_CONFIG_CONNECT_STRING)
        {
        }

        protected override void CreateView(IDatabaseService connectedService, string viewName)
        {
            ExecuteSQLAndIgnoreException(connectedService, "create view {0} as select 1 as version", viewName);
        }

        protected override void DropView(IDatabaseService connectedService, string viewName)
        {
            ExecuteSQLAndIgnoreException(connectedService, "drop view {0}", viewName);
        }
    }
}
