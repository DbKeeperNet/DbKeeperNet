using System;
using DbKeeperNet.Engine;
using Microsoft.Data.Sqlite;

namespace DbKeeperNet.Extensions.SQLite.Checkers
{
    public class SQLiteIndexChecker : IDatabaseServiceIndexChecker
    {
        private readonly IDatabaseService<SqliteConnection> _databaseService;

        public SQLiteIndexChecker(IDatabaseService<SqliteConnection> databaseService)
        {
            _databaseService = databaseService;
        }

        public bool Exists(string name, string table)
        {
            if (String.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            using (var stepExecutedQuery = new SqliteCommand(
                $"PRAGMA index_list({table})",
                _databaseService.GetOpenConnection()))
            using (var reader = stepExecutedQuery.ExecuteReader())
            {
                while (reader.Read())
                {
                    var indexName = (string)reader["name"];
                    if (name == indexName) return true;
                }


                return false;
            }
        }
    }
}