using System;
using System.Globalization;
using DbKeeperNet.Engine;
using Microsoft.Data.Sqlite;

namespace DbKeeperNet.Extensions.SQLite.Checkers
{
    public class SQLiteForeignKeyChecker : IDatabaseServiceForeignKeyChecker
    {
        private readonly IDatabaseService<SqliteConnection> _databaseService;

        public SQLiteForeignKeyChecker(IDatabaseService<SqliteConnection> databaseService)
        {
            _databaseService = databaseService;
        }

        public bool Exists(string name, string table)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            if (String.IsNullOrEmpty(table))
                throw new ArgumentNullException(nameof(table));

            using (var stepExecutedQuery = new SqliteCommand(
                $"PRAGMA foreign_key_list({table})",
                _databaseService.GetOpenConnection()))
            using (var reader = stepExecutedQuery.ExecuteReader())
            {

                while (reader.Read())
                {
                    var keyName = string.Format(CultureInfo.InvariantCulture, "FK_{0}_{1}", table,
                        reader["from"]);

                    if (name == keyName) return true;
                }


                return false;
            }
        }
    }
}