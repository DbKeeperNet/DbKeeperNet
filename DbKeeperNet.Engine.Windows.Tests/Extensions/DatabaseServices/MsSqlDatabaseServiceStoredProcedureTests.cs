using DbKeeperNet.Engine.Windows.Extensions.DatabaseServices;
using NUnit.Framework;

namespace DbKeeperNet.Engine.Windows.Tests.Extensions.DatabaseServices
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
            ExecuteSqlAndIgnoreException(connectedService, "create procedure {0} as select 1", procedureName);
        }

        protected override void DropStoredProcedure(IDatabaseService connectedService, string procedureName)
        {
            ExecuteSqlAndIgnoreException(connectedService, "drop procedure {0}", procedureName);
        }

    }
}
