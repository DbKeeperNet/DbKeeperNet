using System;
using System.Globalization;
using DbKeeperNet.Engine;
using Microsoft.Data.Sqlite;

namespace DbKeeperNet.Extensions.SQLite.Checkers
{
    public class SQLitePrimaryKeyChecker : IDatabaseServicePrimaryKeyChecker
    {
        private readonly IDatabaseService<SqliteConnection> _databaseService;

        public SQLitePrimaryKeyChecker(IDatabaseService<SqliteConnection> databaseService)
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

            using (var stepExecutedQuery = new SqliteCommand(
                $"PRAGMA table_info({table})",
                _databaseService.GetOpenConnection()))
            using (var reader = stepExecutedQuery.ExecuteReader())
            {
                while (reader.Read())
                {
                    var primaryKeyIndex = (long)reader["pk"];

                    // at least one column in PK means that PK exists
                    if (primaryKeyIndex != 0)
                    {
                        return true;
                    }
                }


                return false;
            }
        }
    }
}