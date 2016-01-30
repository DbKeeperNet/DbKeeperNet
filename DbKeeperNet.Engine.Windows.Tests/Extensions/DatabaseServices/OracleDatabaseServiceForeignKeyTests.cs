using DbKeeperNet.Engine.Windows.Extensions.DatabaseServices;
using NUnit.Framework;

namespace DbKeeperNet.Engine.Windows.Tests.Extensions.DatabaseServices
{
    [TestFixture]
    [Explicit]
    [Category("oracle")]
    public class OracleDatabaseServiceForeignKeyTests : DatabaseServiceForeignKeyTests<OracleDatabaseService>
    {
        private const string APP_CONFIG_CONNECT_STRING = @"oracle";

        public OracleDatabaseServiceForeignKeyTests()
            : base(APP_CONFIG_CONNECT_STRING)
        {
        }

        protected override void CreateNamedForeignKey(IDatabaseService connectedService, string tableName, string foreignKeyName)
        {
            ExecuteSqlAndIgnoreException(connectedService, "create table \"{0}\"(id numeric(9,0) not null, rec_id numeric(9,0), CONSTRAINT \"PK_ora_testing_fk\" PRIMARY KEY (id), CONSTRAINT \"{1}\" FOREIGN KEY (rec_id) REFERENCES \"{0}\"(id))", tableName, foreignKeyName);
        }

        protected override void DropNamedForeignKey(IDatabaseService connectedService, string tableName, string foreignKeyName)
        {
            ExecuteSqlAndIgnoreException(connectedService, "drop table \"{0}\"", tableName);
        }
    }
}
