using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using DbKeeperNet.Engine.Extensions.DatabaseServices;

namespace DbKeeperNet.Engine.Tests.Extensions.DatabaseServices
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
            ExecuteSQLAndIgnoreException(connectedService, @"create table {0}(c varchar)", tableName);
        }

        protected override void DropTable(IDatabaseService connectedService, string tableName)
        {
            ExecuteSQLAndIgnoreException(connectedService, @"drop table {0}", tableName);
        }
    }
}
