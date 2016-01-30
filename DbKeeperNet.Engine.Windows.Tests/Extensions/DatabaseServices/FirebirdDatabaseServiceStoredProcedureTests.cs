using DbKeeperNet.Engine.Windows.Extensions.DatabaseServices;
using NUnit.Framework;

namespace DbKeeperNet.Engine.Windows.Tests.Extensions.DatabaseServices
{

    [TestFixture]
    [Explicit]
    [Category("firebird")]
    public class FirebirdDatabaseServiceStoredProcedureTests : DatabaseServiceStoredProceduresTests<FirebirdDatabaseService>
    {
        private const string APP_CONFIG_CONNECT_STRING = @"firebird";

        public FirebirdDatabaseServiceStoredProcedureTests()
            : base(APP_CONFIG_CONNECT_STRING)
        {

        }

        protected override void CreateStoredProcedure(IDatabaseService connectedService, string procedureName)
        {
            ExecuteSqlAndIgnoreException(connectedService, "create procedure \"{0}\" returns (r int) \nas \nbegin\n for select count(*) from rdb$relations into :r do begin suspend;end\nend", procedureName);
        }

        protected override void DropStoredProcedure(IDatabaseService connectedService, string procedureName)
        {
            ExecuteSqlAndIgnoreException(connectedService, @"drop procedure ""{0}""", procedureName);
        }

    }
}