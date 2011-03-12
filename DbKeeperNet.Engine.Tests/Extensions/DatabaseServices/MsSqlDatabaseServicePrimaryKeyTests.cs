using System;
using System.Collections.Generic;
using System.Text;
using DbKeeperNet.Engine.Extensions.DatabaseServices;
using NUnit.Framework;

namespace DbKeeperNet.Engine.Tests.Extensions.DatabaseServices
{
    [TestFixture]
    [Explicit]
    [Category("mssql")]
    public class MsSqlDatabaseServicePrimaryKeyTests: DatabaseServicePrimaryKeyTests<MsSqlDatabaseService>
    {
        private const string APP_CONFIG_CONNECT_STRING = @"mssql";

        public MsSqlDatabaseServicePrimaryKeyTests()
            : base(APP_CONFIG_CONNECT_STRING)
        {
        }

        protected override void CreateNamedPrimaryKey(IDatabaseService connectedService, string tableName, string primaryKeyName)
        {
            ExecuteSQLAndIgnoreException(connectedService, "create table {0}(id int not null, CONSTRAINT {1} PRIMARY KEY (id))", tableName, primaryKeyName);
        }

        protected override void DropNamedPrimaryKey(IDatabaseService connectedService, string tableName, string primaryKeyName)
        {
            ExecuteSQLAndIgnoreException(connectedService, "drop table {0}", tableName);
        }
    }
}
