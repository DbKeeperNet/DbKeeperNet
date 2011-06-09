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
    public class OracleDatabaseServiceViewTests : DatabaseServiceViewTests<OracleDatabaseService>
    {
        private const string TESTING_TABLE = @"orasql_testing_tr";
        private const string APP_CONFIG_CONNECT_STRING = @"oracle";

        public OracleDatabaseServiceViewTests()
            : base(APP_CONFIG_CONNECT_STRING)
        {
        }

        protected override void CreateView(IDatabaseService connectedService, string viewName)
        {
            ExecuteSQLAndIgnoreException(connectedService, "create table \"{0}\" (c varchar(2))", TESTING_TABLE);
            ExecuteSQLAndIgnoreException(connectedService, "create view \"{0}\" as select * from \"ora_testing_tv\"", viewName, TESTING_TABLE);
        }

        protected override void DropView(IDatabaseService connectedService, string viewName)
        {
            ExecuteSQLAndIgnoreException(connectedService, "create view \"{0}\"", viewName);
            ExecuteSQLAndIgnoreException(connectedService, "create table \"{0}\"", TESTING_TABLE);
        }
    }
}
