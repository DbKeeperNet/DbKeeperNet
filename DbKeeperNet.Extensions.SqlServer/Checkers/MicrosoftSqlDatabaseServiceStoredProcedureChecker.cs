using System;
using System.Data.SqlClient;
using DbKeeperNet.Engine;

namespace DbKeeperNet.Extensions.SqlServer.Checkers
{
    public class MicrosoftSqlDatabaseServiceStoredProcedureChecker : IDatabaseServiceStoredProcedureChecker
    {
        private readonly IDatabaseService<SqlConnection> _databaseService;

        public MicrosoftSqlDatabaseServiceStoredProcedureChecker(IDatabaseService<SqlConnection> databaseService)
        {
            _databaseService = databaseService;
        }

        public bool Exists(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            var restrictions = new string[4];

            restrictions[2] = name;
            restrictions[3] = "PROCEDURE";

            var schema = _databaseService.GetOpenConnection().GetSchema("Procedures", restrictions);

            bool exists = (schema.Rows.Count != 0);

            return exists;

        }
    }
}