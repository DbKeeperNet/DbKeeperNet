using System;
using DbKeeperNet.Engine;
using Microsoft.Data.Sqlite;

namespace DbKeeperNet.Extensions.SQLite.Checkers
{
    public class SQLiteTableChecker : IDatabaseServiceTableChecker
    {
        private readonly IDatabaseService _databaseService;

        public SQLiteTableChecker(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public bool Exists(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));
            
            var command = _databaseService.GetOpenConnection().CreateCommand();
            command.CommandText = "SELECT count(name) FROM sqlite_master WHERE type='table' AND name=@tableName";
            var tableNameParameter = new SqliteParameter("@tableName", SqliteType.Text);
            tableNameParameter.Value = name;

            command.Parameters.Add(tableNameParameter);

            long? count = (long?)command.ExecuteScalar();

            var result = (count.HasValue) && (count.Value > 0);

            return result;
        }
    }
}