using System;
using System.Data.SqlClient;
using DbKeeperNet.Engine;

namespace DbKeeperNet.Extensions.SqlServer.Checkers
{
    public class MicrosoftSqlDatabaseServiceViewChecker : IDatabaseServiceViewChecker
    {
        private readonly IDatabaseService<SqlConnection> _databaseService;

        public MicrosoftSqlDatabaseServiceViewChecker(IDatabaseService<SqlConnection> databaseService)
        {
            _databaseService = databaseService;
        }

        public bool Exists(string name)
        {
            if (String.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            string[] restrictions = new string[3];

            restrictions[2] = name;

            var schema = _databaseService.GetOpenConnection().GetSchema("Views", restrictions);

            var exists = (schema.Rows.Count != 0);

            return exists;

        }
    }
}