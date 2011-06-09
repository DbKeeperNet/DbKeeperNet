using System;
using System.Collections.Generic;
using System.Text;
using DbKeeperNet.Engine.Extensions.DatabaseServices;
using NUnit.Framework;

namespace DbKeeperNet.Engine.Tests.Extensions.DatabaseServices
{
    [TestFixture]
    [Explicit]
    [Category("oracle")]
    public class OracleDatabaseServiceTableTests : DatabaseServiceTableTests<OracleDatabaseService>
    {
        private const string APP_CONFIG_CONNECT_STRING = @"oracle";

        public OracleDatabaseServiceTableTests()
            : base(APP_CONFIG_CONNECT_STRING)
        {
        }
        protected override void CreateTable(IDatabaseService connectedService, string tableName)
        {
            ExecuteSQLAndIgnoreException(connectedService, "create table \"{0}\"(c varchar(2))", tableName);
        }

        protected override void DropTable(IDatabaseService connectedService, string tableName)
        {
            ExecuteSQLAndIgnoreException(connectedService, "drop table \"{0}\"", tableName);
        }
    }
}
