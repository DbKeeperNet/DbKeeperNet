using System;
using DbKeeperNet.Engine;
using MySql.Data.MySqlClient;

namespace DbKeeperNet.Extensions.Mysql.Checkers
{
    public class MySqlDatabaseServiceViewChecker : IDatabaseServiceViewChecker
    {
        private readonly IDatabaseService<MySqlConnection> _databaseService;

        public MySqlDatabaseServiceViewChecker(IDatabaseService<MySqlConnection> databaseService)
        {
            _databaseService = databaseService;
        }

        public bool Exists(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            var restrictions = new string[3];

            restrictions[2] = name;

            var schema = _databaseService.GetOpenConnection().GetSchema("Views", restrictions);

            var exists = (schema.Rows.Count != 0);

            return exists;

        }
    }
}