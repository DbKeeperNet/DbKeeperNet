using DbKeeperNet.Engine.Extensions.DatabaseServices;
using NUnit.Framework;

namespace DbKeeperNet.Engine.Tests.Extensions.DatabaseServices
{
	[TestFixture]
	[Explicit]
	[Category("pgsql")]
	public class PgSqlDatabaseServicePrimaryKeyTests : DatabaseServicePrimaryKeyTests<PgSqlDatabaseService>
	{
		private const string APP_CONFIG_CONNECT_STRING = @"pgsql";

		public PgSqlDatabaseServicePrimaryKeyTests()
			: base(APP_CONFIG_CONNECT_STRING)
		{
		}

		protected override void CreateNamedPrimaryKey(IDatabaseService connectedService, string tableName, string primaryKeyName)
		{
			ExecuteSqlAndIgnoreException(connectedService, @"create table ""{0}""(id int not null, CONSTRAINT ""{1}"" PRIMARY KEY (id))", tableName, primaryKeyName);
		}

		protected override void DropNamedPrimaryKey(IDatabaseService connectedService, string tableName, string primaryKeyName)
		{
			ExecuteSqlAndIgnoreException(connectedService, @"drop table ""{0}""", tableName);
		}
	}
}
