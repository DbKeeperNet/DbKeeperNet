using System;
using DbKeeperNet.Engine;

namespace DbKeeperNet.Extensions.Firebird.Checkers
{
    public class FirebirdDatabaseServiceIndexChecker : FirebirdDatabaseServiceCheckerBase, IDatabaseServiceIndexChecker, IDatabaseServicePrimaryKeyChecker
    {
        public FirebirdDatabaseServiceIndexChecker(IDatabaseService databaseService)
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