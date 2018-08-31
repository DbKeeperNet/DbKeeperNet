using System;
using System.Data;
using DbKeeperNet.Engine;

namespace DbKeeperNet.Extensions.Mysql.Checkers
{
    public class MySqlDatabaseServiceIndexChecker : IDatabaseServiceIndexChecker
    {
        private readonly IDatabaseService _databaseService;

        public MySqlDatabaseServiceIndexChecker(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public bool Exists(string name, string table)
        {
            if (String.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");
            if (String.IsNullOrEmpty(table))
                throw new ArgumentNullException("table");

            DataTable schema = _databaseService.GetOpenConnection().GetSchema("Indexes");

            bool exists = false;

            foreach (DataRow row in schema.Rows)
            {
                if (name.Equals((string)row[2], StringComparison.OrdinalIgnoreCase)
                    && table.Equals((string)row[3], StringComparison.OrdinalIgnoreCase))
                {
                    exists = true;
                    break;
                }
            }

            return exists;

        }
    }
}