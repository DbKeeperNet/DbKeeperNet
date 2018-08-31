using System;
using System.Data;
using DbKeeperNet.Engine;

namespace DbKeeperNet.Extensions.SqlServer.Checkers
{
    public class MicrosoftSqlDatabaseServiceIndexChecker : IDatabaseServiceIndexChecker, IDatabaseServicePrimaryKeyChecker
    {
        private readonly IDatabaseService _databaseService;

        public MicrosoftSqlDatabaseServiceIndexChecker(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public bool Exists(string name, string table)
        {
            if (String.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            string[] restrictions = new string[4];

            restrictions[3] = name;

            DataTable schema = _databaseService.GetOpenConnection().GetSchema("Indexes", restrictions);

            bool exists = (schema.Rows.Count != 0);

            return exists;
        }
    }
}