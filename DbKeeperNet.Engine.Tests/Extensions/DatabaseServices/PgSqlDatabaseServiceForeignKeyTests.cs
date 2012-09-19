using DbKeeperNet.Engine.Extensions.DatabaseServices;
using NUnit.Framework;

namespace DbKeeperNet.Engine.Tests.Extensions.DatabaseServices
{
	[TestFixture]
	[Explicit]
	[Category("pgsql")]
	public class PgSqlDatabaseServiceForeignKeyTests: DatabaseServiceForeignKeyTests<PgSqlDatabaseService>
	{
		private const string APP_CONFIG_CONNECT_STRING = @"pgsql";

        public PgSqlDatabaseServiceForeignKeyTests()
            : base(APP_CONFIG_CONNECT_STRING)
        {
        }

        protected override void CreateNamedForeignKey(IDatabaseService connectedService, string tableName, string foreignKeyName)
        {
            ExecuteSqlAndIgnoreException(connectedService, @"create table ""{0}""(id int not null, rec_id int, CONSTRAINT ""PK_{0}"" PRIMARY KEY (id), CONSTRAINT ""{1}"" FOREIGN KEY (rec_id) REFERENCES ""{0}"" (id))", tableName, foreignKeyName);
        }

        protected override void DropNamedForeignKey(IDatabaseService connectedService, string tableName, string foreignKeyName)
        {
            ExecuteSqlAndIgnoreException(connectedService, @"drop table ""{0}""", tableName);
        }
	}
}