using System;
using System.Data;
using DbKeeperNet.Engine;

namespace DbKeeperNet.Extensions.SqlServer.Checkers
{
    public class MicrosoftSqlDatabaseServiceForeignKeyChecker : IDatabaseServiceForeignKeyChecker
    {
        private readonly IDatabaseService _databaseService;

        public MicrosoftSqlDatabaseServiceForeignKeyChecker(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public bool Exists(string foreignKeyName, string table)
        {
            if (String.IsNullOrEmpty(foreignKeyName))
                throw new ArgumentNullException("foreignKeyName");

            string[] restrictions = new string[4];

            restrictions[3] = foreignKeyName;

            DataTable schema = _databaseService.GetOpenConnection().GetSchema("ForeignKeys", restrictions);

            bool exists = (schema.Rows.Count != 0);

            return exists;
        }
    }
}