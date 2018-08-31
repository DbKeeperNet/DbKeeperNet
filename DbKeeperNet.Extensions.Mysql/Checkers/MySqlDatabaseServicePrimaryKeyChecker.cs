using System;
using System.Data;
using System.Globalization;
using DbKeeperNet.Engine;

namespace DbKeeperNet.Extensions.Mysql.Checkers
{
    public class MySqlDatabaseServicePrimaryKeyChecker : IDatabaseServicePrimaryKeyChecker
    {
        private readonly IDatabaseService _databaseService;

        public MySqlDatabaseServicePrimaryKeyChecker(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public bool Exists(string name, string table)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));
            if (string.IsNullOrEmpty(table))
                throw new ArgumentNullException(nameof(table));

            // PK name must be in form of PK_tablename
            var expectedName = string.Format(CultureInfo.InvariantCulture, "PK_{0}", table);
            if (expectedName != name) return false;

            var schema = _databaseService.GetOpenConnection().GetSchema("Indexes");

            bool exists = false;

            foreach (DataRow row in schema.Rows)
            {
                if ("PRIMARY".Equals((string)row[2], StringComparison.OrdinalIgnoreCase)
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