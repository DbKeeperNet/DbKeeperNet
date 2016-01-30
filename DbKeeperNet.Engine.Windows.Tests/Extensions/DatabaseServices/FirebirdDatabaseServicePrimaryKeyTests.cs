using DbKeeperNet.Engine.Windows.Extensions.DatabaseServices;
using NUnit.Framework;

namespace DbKeeperNet.Engine.Windows.Tests.Extensions.DatabaseServices
{
    [TestFixture]
    [Explicit]
    [Category("firebird")]
    public class FirebirdDatabaseServicePrimaryKeyTests: DatabaseServicePrimaryKeyTests<FirebirdDatabaseService>
    {
        private const string APP_CONFIG_CONNECT_STRING = @"firebird";

        public FirebirdDatabaseServicePrimaryKeyTests()
            : base(APP_CONFIG_CONNECT_STRING)
        {
        }

        protected override void CreateNamedPrimaryKey(IDatabaseService connectedService, string tableName, string primaryKeyName)
        {
            ExecuteSqlAndIgnoreException(connectedService, @"create table ""{0}"" (id int not null, CONSTRAINT ""{1}"" PRIMARY KEY (id))", tableName, primaryKeyName);
        }

        protected override void DropNamedPrimaryKey(IDatabaseService connectedService, string tableName, string primaryKeyName)
        {
            ExecuteSqlAndIgnoreException(connectedService, @"drop table ""{0}""", tableName);
        }
    }
}