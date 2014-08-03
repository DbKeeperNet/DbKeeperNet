using DbKeeperNet.Engine.Extensions.DatabaseServices;
using NUnit.Framework;

namespace DbKeeperNet.Engine.Tests.Extensions.DatabaseServices
{
    [TestFixture]
    [Explicit]
    [Category("firebird")]
    public class FirebirdDatabaseServiceViewTests: DatabaseServiceViewTests<FirebirdDatabaseService>
    {
        private const string APP_CONFIG_CONNECT_STRING = @"firebird";

        public FirebirdDatabaseServiceViewTests()
            : base(APP_CONFIG_CONNECT_STRING)
        {
        }

        protected override void CreateView(IDatabaseService connectedService, string viewName)
        {
            ExecuteSqlAndIgnoreException(connectedService, "create view \"{0}\" as select rdb$relation_name from rdb$relations where 1 = 0", viewName);
        }

        protected override void DropView(IDatabaseService connectedService, string viewName)
        {
            ExecuteSqlAndIgnoreException(connectedService, "drop view \"{0}\"", viewName);
        }
    }
}