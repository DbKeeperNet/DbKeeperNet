using System;
using DbKeeperNet.Engine;
using MySql.Data.MySqlClient;

namespace DbKeeperNet.Extensions.Mysql.Checkers
{
    public class MySqlDatabaseServiceTableChecker : IDatabaseServiceTableChecker
    {
        private readonly IDatabaseService<MySqlConnection> _databaseService;

        public MySqlDatabaseServiceTableChecker(IDatabaseService<MySqlConnection> databaseService)
        {
            _databaseService = databaseService;
        }

        public bool Exists(string name)
        {
            if (String.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            string[] restrictions = new string[4];

            restrictions[2] = name;
            restrictions[3] = "BASE TABLE";

            var schema = _databaseService.GetOpenConnection().GetSchema("Tables", restrictions);

            bool exists = (schema.Rows.Count != 0);

            return exists;


        }
    }
}