using System;
using DbKeeperNet.Engine;

namespace DbKeeperNet.Extensions.Firebird.Checkers
{
    public class FirebirdDatabaseServiceTriggerChecker : FirebirdDatabaseServiceCheckerBase, IDatabaseServiceTriggerChecker
    {
        public FirebirdDatabaseServiceTriggerChecker(IDatabaseService databaseService)
            : base(databaseService)
        {
        }

        public bool Exists(string name)
        {
            if (String.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            string[] restrictions = new string[4];

            restrictions[3] = name;

            return RetrieveSchemaInformationAndReturnTrueIfRowExists(@"Triggers", restrictions);

        }
    }
}