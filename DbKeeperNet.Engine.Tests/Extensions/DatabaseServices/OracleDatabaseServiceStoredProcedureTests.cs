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
    public class OracleDatabaseServiceStoredProcedureTests: DatabaseServiceStoredProceduresTests<OracleDatabaseService>
    {
        private const string APP_CONFIG_CONNECT_STRING = @"oracle";

        public OracleDatabaseServiceStoredProcedureTests()
            : base(APP_CONFIG_CONNECT_STRING)
        {

        }

        protected override void CreateStoredProcedure(IDatabaseService connectedService, string procedureName)
        {
            ExecuteSQLAndIgnoreException(connectedService, "create procedure \"{0}\" as select 1", procedureName);
        }

        protected override void DropStoredProcedure(IDatabaseService connectedService, string procedureName)
        {
            ExecuteSQLAndIgnoreException(connectedService, "drop procedure \"{0}\"", procedureName);
        }
    }
}
