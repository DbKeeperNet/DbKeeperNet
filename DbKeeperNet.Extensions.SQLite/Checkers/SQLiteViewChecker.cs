using System;
using DbKeeperNet.Engine;
using Microsoft.Data.Sqlite;

namespace DbKeeperNet.Extensions.SQLite.Checkers
{
    public class SQLiteViewChecker : IDatabaseServiceViewChecker
    {
        private readonly IDatabaseService<SqliteConnection> _databaseService;

        public SQLiteViewChecker(IDatabaseService<SqliteConnection> databaseService)
        {
            _databaseService = databaseService;
        }

        public bool Exists(string viewName)
        {
            if (string.IsNullOrEmpty(viewName))
                throw new ArgumentNullException(nameof(viewName));

            var command = _databaseService.GetOpenConnection().CreateCommand();
            command.CommandText = "SELECT count(name) FROM sqlite_master WHERE type='view' AND name=@tableName";
            var tableNameParameter = new SqliteParameter("@tableName", SqliteType.Text) {Value = viewName};

            command.Parameters.Add(tableNameParameter);

            long? count = (long?)command.ExecuteScalar();

            var result = (count.HasValue) && (count.Value > 0);

            return result;
        }
    }
}