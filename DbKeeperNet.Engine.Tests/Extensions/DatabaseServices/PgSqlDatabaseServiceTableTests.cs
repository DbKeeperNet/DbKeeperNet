using System;
using System.Collections.Generic;
using System.Text;
using DbKeeperNet.Engine.Extensions.DatabaseServices;
using NUnit.Framework;

namespace DbKeeperNet.Engine.Tests.Extensions.DatabaseServices
{
    [TestFixture]
    [Explicit]
    [Category("pgsql")]
    public class PgSqlDatabaseServiceTableTests : DatabaseServiceTableTests<PgSqlDatabaseService>
    {
        private const string APP_CONFIG_CONNECT_STRING = @"pgsql";

        public PgSqlDatabaseServiceTableTests()
            : base(APP_CONFIG_CONNECT_STRING)
        {
        }
        protected override void CreateTable(IDatabaseService connectedService, string tableName)
        {
            ExecuteSQLAndIgnoreException(connectedService, "create table {0}(c char)", tableName);
        }

        protected override void DropTable(IDatabaseService connectedService, string tableName)
        {
            ExecuteSQLAndIgnoreException(connectedService, "drop table {0}", tableName);
        }
    }
}
