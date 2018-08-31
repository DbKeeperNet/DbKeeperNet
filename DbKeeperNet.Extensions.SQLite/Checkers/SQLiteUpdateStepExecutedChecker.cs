using DbKeeperNet.Engine;
using Microsoft.Data.Sqlite;

namespace DbKeeperNet.Extensions.SQLite.Checkers
{
    public class SQLiteUpdateStepExecutedChecker : IUpdateStepExecutedChecker
    {
        private readonly IDatabaseService<SqliteConnection> _databaseService;

        public SQLiteUpdateStepExecutedChecker(IDatabaseService<SqliteConnection> databaseService)
        {
            _databaseService = databaseService;
        }

        public bool IsExecuted(string assembly, string version, int stepNumber)
        {
            bool result = false;

            using (var stepExecutedQuery = new SqliteCommand(
                @"select count(version)  from dbkeepernet_assembly asm
                    join dbkeepernet_version ver on asm.id = ver.dbkeepernet_assembly_id
                    join dbkeepernet_step step on ver.id = step.dbkeepernet_version_id
                    where step.step = @step 
                    and ver.version = @version and 
                    asm.assembly = @assembly",
                _databaseService.GetOpenConnection()))
            {
                var assemblyParameter = new SqliteParameter("@assembly", SqliteType.Text) { Value = assembly };
                var versionParameter = new SqliteParameter("@version", SqliteType.Text) { Value = version };
                var stepParameter = new SqliteParameter("@step", SqliteType.Integer) { Value = stepNumber };

                stepExecutedQuery.Parameters.Add(assemblyParameter);
                stepExecutedQuery.Parameters.Add(versionParameter);
                stepExecutedQuery.Parameters.Add(stepParameter);

                long? count = (long?)stepExecutedQuery.ExecuteScalar();

                if ((count.HasValue) && (count.Value > 0))
                    result = true;

                return result;
            }
        }


    }
}