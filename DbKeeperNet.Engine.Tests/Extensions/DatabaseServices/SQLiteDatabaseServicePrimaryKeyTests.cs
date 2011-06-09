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
    public class SQLiteDatabaseServicePrimaryKeyTests : DatabaseServicePrimaryKeyTests<SQLiteDatabaseService>
    {
        private const string APP_CONFIG_CONNECT_STRING = @"sqlite";

        public SQLiteDatabaseServicePrimaryKeyTests()
            : base(APP_CONFIG_CONNECT_STRING)
        {
        }

        protected override void CreateNamedPrimaryKey(IDatabaseService connectedService, string tableName, string primaryKeyName)
        {
            ExecuteSqlAndIgnoreException(connectedService, "create table {0}(id integer not null, CONSTRAINT {1} PRIMARY KEY (id))", tableName, primaryKeyName);
        }

        protected override void DropNamedPrimaryKey(IDatabaseService connectedService, string tableName, string primaryKeyName)
        {
            ExecuteSqlAndIgnoreException(connectedService, "drop table {0}", tableName);
        }
    }
}
