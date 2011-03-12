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
    public class MySqlNetConnectorDatabaseServiceViewTests: DatabaseServiceViewTests<MySqlNetConnectorDatabaseService>
    {
        private const string APP_CONFIG_CONNECT_STRING = @"mysql";

        public MySqlNetConnectorDatabaseServiceViewTests()
            : base(APP_CONFIG_CONNECT_STRING)
        {
        }

        protected override void CreateView(IDatabaseService connectedService, string viewName)
        {
            ExecuteSQLAndIgnoreException(connectedService, @"create view {0} as select 1 as version", viewName);
        }

        protected override void DropView(IDatabaseService connectedService, string viewName)
        {
            ExecuteSQLAndIgnoreException(connectedService, @"drop view {0}", viewName);
        }
    }
}
