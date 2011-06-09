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
    public class SQLiteDatabaseServiceTableTests: DatabaseServiceTableTests<SQLiteDatabaseService>
    {
        private const string APP_CONFIG_CONNECT_STRING = @"sqlite";

        public SQLiteDatabaseServiceTableTests()
            : base(APP_CONFIG_CONNECT_STRING)
        {
        }

        protected override void CreateTable(IDatabaseService connectedService, string tableName)
        {
            ExecuteSqlAndIgnoreException(connectedService, "create table {0}(c text)", tableName);
        }

        protected override void DropTable(IDatabaseService connectedService, string tableName)
        {
            ExecuteSqlAndIgnoreException(connectedService, "drop table {0}", tableName);
        }
    }
}
