using System;
using System.Data;
using DbKeeperNet.Engine;

namespace DbKeeperNet.Extensions.Mysql.Checkers
{
    public class MySqlDatabaseServiceForeignKeyChecker : IDatabaseServiceForeignKeyChecker
    {
        private readonly IDatabaseService _databaseService;

        public MySqlDatabaseServiceForeignKeyChecker(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public bool Exists(string foreignKeyName, string table)
        {
            if (string.IsNullOrEmpty(foreignKeyName))
                throw new ArgumentNullException("foreignKeyName");
            if (string.IsNullOrEmpty(table))
                throw new ArgumentNullException("table");

            DataTable schema = _databaseService.GetOpenConnection().GetSchema("Indexes");

            bool exists = false;

            foreach (DataRow row in schema.Rows)
            {
                if (foreignKeyName.Equals((string)row[2], StringComparison.OrdinalIgnoreCase)
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