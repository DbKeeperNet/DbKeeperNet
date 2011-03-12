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
    public class MsSqlDatabaseServiceStoredProcedureTests : DatabaseServiceStoredProceduresTests<MsSqlDatabaseService>
    {
        private const string APP_CONFIG_CONNECT_STRING = @"mssql";

        public MsSqlDatabaseServiceStoredProcedureTests()
            : base(APP_CONFIG_CONNECT_STRING)
        {

        }

        protected override void CreateStoredProcedure(IDatabaseService connectedService, string procedureName)
        {
            ExecuteSQLAndIgnoreException(connectedService, "create procedure {0} as select 1", procedureName);
        }

        protected override void DropStoredProcedure(IDatabaseService connectedService, string procedureName)
        {
            ExecuteSQLAndIgnoreException(connectedService, "drop procedure {0}", procedureName);
        }

    }
}
