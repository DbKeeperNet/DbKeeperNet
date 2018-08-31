using System;
using DbKeeperNet.Engine;

namespace DbKeeperNet.Extensions.Pgsql.Checkers
{
    public class PgsqlDatabaseServiceIndexChecker : PgsqlDatabaseServiceCheckerBase, IDatabaseServiceIndexChecker, IDatabaseServicePrimaryKeyChecker
    {
        public PgsqlDatabaseServiceIndexChecker(IDatabaseService databaseService)
        : base(databaseService)
        {
        }

        public bool Exists(string name, string table)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));
            if (string.IsNullOrEmpty(table))
                throw new ArgumentNullException(nameof(table));

            var restrictions = new string[4];

            restrictions[2] = table;
            restrictions[3] = name;

            return RetrieveSchemaInformationAndReturnTrueIfRowExists(@"Indexes", restrictions);
        }
    }
}