using DbKeeperNet.Engine.Windows.Extensions.DatabaseServices;
using NUnit.Framework;

namespace DbKeeperNet.Engine.Windows.Tests.Extensions.DatabaseServices
{
    [TestFixture]
    [Explicit]
    [Category("oracle")]
    public class OracleDatabaseServiceViewTests : DatabaseServiceViewTests<OracleDatabaseService>
    {
        private const string TESTING_TABLE = @"orasql_testing_vi";
        private const string APP_CONFIG_CONNECT_STRING = @"oracle";

        public OracleDatabaseServiceViewTests()
            : base(APP_CONFIG_CONNECT_STRING)
        {
        }

        protected override void CreateView(IDatabaseService connectedService, string viewName)
        {
            ExecuteSqlAndIgnoreException(connectedService, "create table \"{0}\" (c varchar(2))", TESTING_TABLE);
            ExecuteSqlAndIgnoreException(connectedService, "create view \"{0}\" as select * from \"{1}\"", viewName, TESTING_TABLE);
        }

        protected override void DropView(IDatabaseService connectedService, string viewName)
        {
            ExecuteSqlAndIgnoreException(connectedService, "drop view \"{0}\"", viewName);
            ExecuteSqlAndIgnoreException(connectedService, "drop table \"{0}\"", TESTING_TABLE);
        }
    }
}
